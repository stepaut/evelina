using Db;
using ReactiveUI;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class PortfolioViewModel : WindowViewModelBase
    {
        public ICommand CloseCommand { get; }


        public string Name => Model?.Name;


        internal IPortfolio Model { get; }


        public PortfolioViewModel(IPortfolio model, MainViewModel main) : base(main)
        {
            Model = model;

            CloseCommand = ReactiveCommand.Create(() => Close());
        }


        private void Close()
        {
            Model.Save();
            TurnBack();
        }
    }
}
