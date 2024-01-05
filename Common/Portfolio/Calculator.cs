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
                CalcStatus(asset);
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
                res += asset.Stat.Volume;
            }

            _portfolio.Stat.Volume = res;
        }

        private void CalcVolume(IAsset asset)
        {
            double volume = 0;
            double buyedVolume = 0;

            foreach (var tr in asset.GetTransactions())
            {
                double val = tr.Price * tr.Amount;
                if (tr.Type == ETransaction.Sell)
                {
                    volume -= val;
                }
                else
                {
                    volume += val;
                    buyedVolume += val;
                }
            }

            asset.Stat.Volume = volume;
            asset.Stat.BuyedVolume = buyedVolume;
        }

        private void CalcShare(IAsset asset)
        {
            asset.Stat.Share = asset.Stat.Volume / _portfolio.Stat.Volume * 100;

            double? targetPortolioVolume = (asset.TargetVolume / asset.TargetShare) * 100;

            if (targetPortolioVolume.HasValue)
            {
                asset.Stat.BuyedShare = asset.Stat.BuyedVolume / targetPortolioVolume.Value * 100;
            }
        }

        private void CalcStatus(IAsset asset)
        {
            if (asset.Stat.BuyedVolume == 0)
            {
                asset.Stat.Status = EAssetStatus.Waiting;
                return;
            }

            asset.Stat.Status = EAssetStatus.Buyed;

            if (asset.Stat.Volume <= IPortfolio.POSSIBLE_DELTA)
            {
                asset.Stat.Status = EAssetStatus.Free;
                return;
            }

            if (!asset.TargetVolume.HasValue)
            {
                return;
            }


            if (Math.Abs(asset.Stat.BuyedVolume - asset.TargetVolume.Value) >= IPortfolio.POSSIBLE_DELTA)
            {
                return;
            }

            asset.Stat.Status = EAssetStatus.Buyed_fully;
        }
    }
}
