using Db;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class PortfolioViewModel : WindowViewModelBase, IDisposable
    {
        public ICommand CloseCommand { get; }
        public ICommand EditCommand { get; }


        public string Name => Model?.Name;


        internal IPortfolio Model { get; private set; }


        public PortfolioViewModel(IPortfolio model, MainViewModel main) : base(main)
        {
            Model = model;

            CloseCommand = ReactiveCommand.Create(Close);
            EditCommand = ReactiveCommand.Create(EditPortfoliInfo);
        }


        private void Close()
        {
            Model.Save();
            TurnBack();
        }

        private void EditPortfoliInfo()
        {
            var editorVM = new PortfolioEditingViewModel(Model, _main);
            editorVM.ReturnBackEvent += () => { _main.ActiveVM = this; };
            _main.ActiveVM = editorVM;
        }

        public void Dispose()
        {
            Model = null;
        }
    }
}
