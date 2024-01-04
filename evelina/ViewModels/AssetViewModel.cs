using Db;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class AssetViewModel : WindowViewModelBase, IDisposable
    {
        internal delegate void DeleteMe(AssetViewModel vm);
        internal event DeleteMe DeleteMeEvent;


        public ICommand CreateTransactionCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }


        public string Name => Model?.Name;
        public double? TargetVolume => Model?.TargetVolume;
        public double? TargetSellPrice => Model?.TargetSellPrice;
        public double? TargetShare => Model?.TargetShare;

        private double? _Volume;
        public double? Volume
        {
            get => _Volume;
            set => this.RaiseAndSetIfChanged(ref _Volume, value);
        }

        private double? _SellPrice;
        public double? SellPrice
        {
            get => _SellPrice;
            set => this.RaiseAndSetIfChanged(ref _SellPrice, value);
        }

        private double? _Share;
        public double? Share
        {
            get => _Share;
            set => this.RaiseAndSetIfChanged(ref _Share, value);
        }

        public ObservableCollection<TransactionViewModel> Transactions { get; } = new();


        internal IAsset Model { get; private set; }


        public AssetViewModel(IAsset model, MainViewModel main) : base(main)
        {
            Model = model;

            foreach (ITransaction transaction in model.GetTransactions())
            {
                AddTransaction(transaction);
            }

            CreateTransactionCommand = ReactiveCommand.Create(CreateTransaction);
            EditCommand = ReactiveCommand.Create(Edit);
            DeleteCommand = ReactiveCommand.Create(Delete);

            UpdateValues();
        }


        public void Dispose()
        {
            foreach (var transaction in Transactions)
            {
                transaction.DeleteMeEvent -= DeleteTransaction;
                transaction.EditMeEvent -= EditTransaction;
                transaction.Dispose();
            }
            Transactions.Clear();

            Model = null;
        }

        internal void AddTransaction(ITransaction transaction)
        {
            TransactionViewModel vm = new TransactionViewModel(transaction);
            vm.DeleteMeEvent += DeleteTransaction;
            vm.EditMeEvent += EditTransaction;
            Transactions.Add(vm);
        }

        private void EditTransaction(TransactionViewModel vm)
        {
            TransactionEditingViewModel editorVM = new TransactionEditingViewModel(vm.Model, _main);
            _main.ActiveVM = editorVM;
        }

        private async void DeleteTransaction(TransactionViewModel vm)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Deleting",
                $"Are you sure to delete {vm.DisplayName}",
                ButtonEnum.YesNo);

            var res = await box.ShowAsync();

            if (res != ButtonResult.Yes)
            {
                return;
            }

            Model.DeleteTransaction(vm.Model);

            vm.EditMeEvent -= EditTransaction;
            vm.DeleteMeEvent -= DeleteTransaction;
            Transactions.Remove(vm);
        }

        private void CreateTransaction()
        {
            TransactionEditingViewModel editorVM = new TransactionEditingViewModel(this, _main);
            _main.ActiveVM = editorVM;
        }

        private void Edit()
        {
            AssetEditingViewModel editorVM = new AssetEditingViewModel(Model, _main);
            _main.ActiveVM = editorVM;
        }

        private void Delete()
        {
            DeleteMeEvent?.Invoke(this);
        }

        private void UpdateValues()
        {
            double volume = 0;
            foreach (var tr in Transactions)
            {
                double val = tr.Model.Price * tr.Model.Amount;
                if (tr.Model.Type == ETransaction.Sell)
                {
                    val *= -1;
                }

                volume += val;
            }
            Volume = volume;
        }
    }
}
