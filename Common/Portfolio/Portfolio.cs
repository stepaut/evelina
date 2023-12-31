using System.Text.Json;
using System.Text.Json.Serialization;

namespace Db
{
    internal class Portfolio : IPortfolio
    {
        public string Name { get; internal set; }

        public string Description { get; internal set; }

        [JsonIgnore]
        public string Id { get; }

        [JsonIgnore]
        public long CreationDate { get; }

        [JsonIgnore]
        public string ParentId => "";

        [JsonIgnore]
        public EItemLevel Level => EItemLevel.Portfolio;


        private List<Asset> _assets;
        private string _path = null;


        internal Portfolio(string id, long creationDate)
        {
            Id = id;
            CreationDate = creationDate;

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
            if (string.IsNullOrEmpty(_path))
            {
                return false;
            }

            return Save_Internal(_path);
        }

        public IAsset CreateAsset(string assetName)
        {
            var existed = GetAsset(assetName);
            if (existed != null)
            {
                return existed;
            }

            var now = DateTime.Now.Ticks;
            string uid = Guid.NewGuid().ToString();

            Asset asset = new Asset(uid, now, Id);
            asset.Name = assetName;

            _assets.Add(asset);

            return asset;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public async Task<bool> SaveAs(string path)
        {
            if (File.Exists(path))
            {
                return false;
            }

            return Save_Internal(path);
        }

        private bool Save_Internal(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            try
            {
                using (PortfolioContext db = new PortfolioContext(path))
                {
                    Thing thisThing = new Thing(this);
                    db.Things.Add(thisThing);

                    foreach (Asset asset in _assets)
                    {
                        Thing thing = new Thing(asset);
                        db.Things.Add(thing);
                    }

                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void FromJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}
