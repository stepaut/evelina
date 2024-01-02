using Db;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class PortfolioEditingViewModel : WindowViewModelBase, IDisposable
    {
        public ICommand ApplyCommand { get; }
        public ICommand CancelCommand { get; }


        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
        
        private string _description;
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        //public string Description
        //{
        //    get => Model.Description;
        //    set
        //    {
        //        Model.ChangeDescription(value);
        //        this.RaisePropertyChanged(nameof(Description));
        //    }
        //}


        internal IPortfolio Model { get; private set; }


        public PortfolioEditingViewModel(IPortfolio model, MainViewModel main) : base(main)
        {
            Model = model;
            Name = model.Name;
            Description = model.Description;

            ApplyCommand = ReactiveCommand.Create(Apply);
            CancelCommand = ReactiveCommand.Create(Close);
        }


        public void Dispose()
        {
            Model = null;
        }


        private void Apply()
        {
            Model.Name = Name;
            Model.Description = Description;
            Close();
        }

        private void Close()
        {
            TurnBack();
        }
    }
}
