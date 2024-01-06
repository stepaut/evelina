using System;

namespace evelina.ViewModels
{
    public class AssetsTableViewModel : WindowViewModelBase, IDisposable, IMenuCompatible
    {
        public PortfolioViewModel Portfolio { get; private set; }

        public AssetsTableViewModel(PortfolioViewModel portfolio, MainViewModel main) : base(main)
        {
            Portfolio = portfolio;
        }

        public void Dispose()
        {
            Portfolio = null;
        }
    }
}
