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
                res += asset.Volume;
            }

            _portfolio.Volume = res;
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

            asset.Volume = volume;
            asset.BuyedVolume = buyedVolume;
        }

        private void CalcShare(IAsset asset)
        {
            asset.Share = asset.Volume / _portfolio.Volume * 100;

            double? targetPortolioVolume = (asset.TargetVolume / asset.TargetShare) * 100;

            if (targetPortolioVolume.HasValue)
            {
                asset.BuyedShare = asset.BuyedVolume / targetPortolioVolume.Value * 100;
            }
        }

        private void CalcStatus(IAsset asset)
        {
            if (asset.BuyedVolume == 0)
            {
                asset.Status = EAssetStatus.Waiting;
                return;
            }

            asset.Status = EAssetStatus.Buyed;

            if (asset.Volume <= IPortfolio.POSSIBLE_DELTA)
            {
                asset.Status = EAssetStatus.Free;
                return;
            }

            if (!asset.TargetVolume.HasValue)
            {
                return;
            }


            if (Math.Abs(asset.BuyedVolume - asset.TargetVolume.Value) >= IPortfolio.POSSIBLE_DELTA)
            {
                return;
            }

            asset.Status = EAssetStatus.Buyed_fully;
        }
    }
}
