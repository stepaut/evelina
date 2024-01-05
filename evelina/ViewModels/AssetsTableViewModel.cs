using ReactiveUI;
using System;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class AssetsTableViewModel : WindowViewModelBase, IDisposable
    {
        public ICommand BackCommand { get; }

        public PortfolioViewModel Portfolio { get; private set; }

        public AssetsTableViewModel(PortfolioViewModel portfolio, MainViewModel main) : base(main)
        {
            Portfolio = portfolio;

            BackCommand = ReactiveCommand.Create(TurnBack);
        }

        public void Dispose()
        {
            Portfolio = null;
        }
    }
}
