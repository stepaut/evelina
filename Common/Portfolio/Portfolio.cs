using System.Text.Json;
using Avalonia.Threading;

namespace Db
{
    internal class Portfolio : Item, IPortfolio
    {
        public event UpdateVisualStat UpdateVisualStatEvent;

        public string Name { get; set; }

        public string Description { get; set; }

        internal string Path { get; private set; }

        public IPortfolioStat Stat => _stat;


        private List<Asset> _assets;
        private DispatcherTimer _updateStat;
        private PortfolioStat _stat;


        internal Portfolio(string id, long creationDate, string path = null) : base(id, creationDate, "")
        {
            Path = path;
            Level = EItemLevel.Portfolio;
            _assets = new List<Asset>();
            _stat = new PortfolioStat();

            _updateStat = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 100) };
            _updateStat.Tick += _updateStat_Tick;
        }


        public async Task<bool> Save()
        {
            if (string.IsNullOrEmpty(Path))
            {
                return false;
            }

            return PortfolioFactory.SavePortfolio(this, Path);
        }

        public IAsset CreateAsset(string assetName)
        {
            var now = DateTime.Now.Ticks;
            string uid = Guid.NewGuid().ToString();

            Asset asset = new Asset(uid, now, Id, this);
            asset.Name = assetName;

            _assets.Add(asset);
            UpdateStat();

            return asset;
        }

        public void DeleteAsset(IAsset asset)
        {
            Asset real = asset as Asset;
            if (!_assets.Contains(real))
            {
                throw new InvalidOperationException();
            }

            _assets.Remove(real);
            this.UpdateStat();
        }

        public override string ToJson()
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

            bool ok = PortfolioFactory.SavePortfolio(this, Path);

            if (ok)
            {
                if (path != Path)
                {
                    Path = path;
                }
            }

            return ok;
        }

        public override void FromJson(string json)
        {
            PortfolioDTO dto = JsonSerializer.Deserialize<PortfolioDTO>(json);

            Name = dto.Name;
            Description = dto.Description;
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

        public IAsset GetAsset(string assetName)
        {
            return _assets.FirstOrDefault(x => x.Name == assetName);
        }

        #region internal
        internal void AddAsset(Asset asset)
        {
            _assets.Add(asset);
            this.UpdateStat();
        }

        internal void UpdateStat()
        {
            _updateStat.Start();
        }
        #endregion

        private void _updateStat_Tick(object sender, EventArgs e)
        {
            _updateStat.Stop();

            using (var calculator = new Calculator(this))
            {
                calculator.UpdateStat();
            }

            UpdateVisualStatEvent?.Invoke();
        }
    }
}
