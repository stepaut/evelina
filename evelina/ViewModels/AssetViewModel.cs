using Db;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class AssetViewModel : WindowViewModelBase, IDisposable
    {
        public ICommand CreateTransactionCommand { get; }
        public ICommand EditCommand { get; }


        public string Name => Model?.Name;

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
        }


        public void Dispose()
        {
            foreach (var transaction in Transactions)
            {
                transaction.Dispose();
            }
            Transactions.Clear();

            Model = null;
        }

        internal void AddTransaction(ITransaction transaction)
        {
            TransactionViewModel vm = new TransactionViewModel(transaction);
            Transactions.Add(vm);
        }

        private void CreateTransaction()
        {
            TransactionEditingViewModel editorVM = new TransactionEditingViewModel(this, _main);
            _main.ActiveVM = editorVM;
        }

        private void Edit()
        {
            //TODO
        }
    }
}
