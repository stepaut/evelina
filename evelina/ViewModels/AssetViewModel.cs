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
        public double? Volume
        {
            get
            {
                if (Model?.Volume < IPortfolio.POSSIBLE_DELTA)
                {
                    return null;
                }

                return Model?.Volume;
            }
        }
        public double? SellPrice => Model?.SellPrice;
        public double? Share
        {
            get
            {
                if (Model?.Share < IPortfolio.POSSIBLE_DELTA / 100)
                {
                    return null;
                }

                return Model?.Share;
            }
        }
        public double? BuyedVolume => Model?.BuyedVolume;
        public double? BuyedShare => Model?.BuyedShare;
        public EAssetStatus? Status => Model?.Status;
        public bool? IsFree => Status is EAssetStatus.Free;

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
    }
}
