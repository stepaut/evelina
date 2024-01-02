﻿using System.Text.Json;

namespace Db
{
    internal class Portfolio : IPortfolio
    {
        public string Name { get; internal set; }

        public string Description { get; internal set; }

        public string Id { get; }

        public long CreationDate { get; }

        public string ParentId => "";

        public EItemLevel Level => EItemLevel.Portfolio;

        internal string Path { get; private set; } = null;


        private List<Asset> _assets;


        private Portfolio(string id, long creationDate, string path = null)
        {
            Id = id;
            CreationDate = creationDate;
            Path = path;

            _assets = new List<Asset>();
        }


        public IAsset GetAsset(string assetName)
        {
            return _assets.FirstOrDefault(x => x.Name.ToLower() == assetName.ToLower());
        }

        public IList<string> GetAssetNames()
        {
            return _assets.Select(x => x.Name).ToList();
        }

        public async Task<bool> Save()
        {
            if (string.IsNullOrEmpty(Path))
            {
                return false;
            }

            return Save_Internal(Path);
        }

        public IAsset CreateAsset(string assetName)
        {
            var now = DateTime.Now.Ticks;
            string uid = Guid.NewGuid().ToString();

            Asset asset = new Asset(uid, now, Id);
            asset.Name = assetName;

            _assets.Add(asset);

            return asset;
        }

        public string ToJson()
        {
            PortfolioDTO dto = new PortfolioDTO()
            {
                Name = Name,
                Description = Description,
            };

            return JsonSerializer.Serialize(dto);
        }

        public async Task<bool> SaveAs(string path)
        {
            if (File.Exists(path))
            {
                return false;
            }

            return Save_Internal(path);
        }

        public void FromJson(string json)
        {
            PortfolioDTO dto = JsonSerializer.Deserialize<PortfolioDTO>(json);

            Name = dto.Name;
            Description = dto.Description;
        }

        public void ChangeName(string newName)
        {
            Name = newName;
        }

        public void ChangeDescription(string newDescription)
        {
            Description = newDescription;
        }

        #region internal
        public void AddAsset(Asset asset)
        {
            _assets.Add(asset);
        }
        #endregion

        #region private
        private bool Save_Internal(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            try
            {
                using (PortfolioContext db = new PortfolioContext(path))
                {
                    Dictionary<string, Thing> oldThings = db.Things.ToDictionary(x => x.Id, x => x);
                    List<Thing> newThings = new();

                    Thing thisThing = new Thing(this);
                    newThings.Add(thisThing);

                    foreach (Asset asset in _assets)
                    {
                        Thing thing = new Thing(asset);
                        newThings.Add(thing);
                    }

                    foreach (var newThing in newThings)
                    {
                        if (oldThings.TryGetValue(newThing.Id, out var old))
                        {
                            old.JsonValue = newThing.JsonValue;
                            db.Things.Update(old);
                        }
                        else
                        {
                            db.Things.Add(newThing);
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }

            if (path != Path)
            {
                Path = path;
            }

            return true;
        }
        #endregion

        #region static
        public static Portfolio CreatePortfolio(string name)
        {
            var now = DateTime.Now.Ticks;
            var uid = Guid.NewGuid().ToString();

            Portfolio portfolio = new Portfolio(uid, now);

            portfolio.Name = name;

            return portfolio;
        }

        public static Portfolio ReadPortfolio(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            try
            {
                using (PortfolioContext db = new PortfolioContext(path))
                {
                    List<Thing> things = db.Things.ToList();
                    Portfolio portfolio = ReadThings(things, path);

                    return portfolio;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static Portfolio ReadThings(List<Thing> things, string path)
        {
            Thing portfolioThing = null;
            List<Thing> assetThings = new();

            foreach (Thing thing in things)
            {
                if (thing.Level == EItemLevel.Portfolio)
                {
                    if (portfolioThing != null)
                    {
                        throw new Exception();
                    }
                    portfolioThing = thing;
                }
                else if (thing.Level == EItemLevel.Asset)
                {
                    assetThings.Add(thing);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            Portfolio portfolio = new Portfolio(portfolioThing.Id, portfolioThing.CreationDate, path);
            portfolio.FromJson(portfolioThing.JsonValue);

            foreach (Thing thing in assetThings)
            {
                Asset asset = new Asset(thing.Id, thing.CreationDate, thing.ParentId);
                asset.FromJson(thing.JsonValue);
                portfolio.AddAsset(asset);
            }

            return portfolio;
        }

        public IList<IAsset> GetAssets()
        {
            IList<IAsset> assets = new List<IAsset>();
            foreach (IAsset asset in _assets)
            {
                assets.Add(asset);
            }
            return assets;
        }
        #endregion
    }
}
