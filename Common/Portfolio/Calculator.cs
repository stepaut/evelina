namespace Db
{
    internal class Calculator : IDisposable
    {
        private IPortfolio _portfolio;
        private IList<IAsset> _assets;


        public Calculator(IPortfolio portfolio)
        {
            _portfolio = portfolio;
            _assets = portfolio.GetAssets();
        }


        public void UpdateStat()
        {
            foreach (var asset in _assets)
            {
                CalcVolume(asset);
            }

            CalcTotalVolume();

            foreach (var asset in _assets)
            {
                CalcShare(asset);
            }
        }

        public void Dispose()
        {
            _portfolio = null;
            _assets = null;
        }

        private void CalcTotalVolume()
        {
            double res = 0;

            foreach(var asset in _assets)
            {
                res += asset.Volume;
            }

            _portfolio.Volume = res;
        }

        private void CalcVolume(IAsset asset)
        {
            double res = 0;
            foreach (var tr in asset.GetTransactions())
            {
                double val = tr.Price * tr.Amount;
                if (tr.Type == ETransaction.Sell)
                {
                    val *= -1;
                }

                res += val;
            }
            asset.Volume = res;
        }
        
        private void CalcShare(IAsset asset)
        {
            asset.Share = asset.Volume / _portfolio.Volume * 100;
        }
    }
}
