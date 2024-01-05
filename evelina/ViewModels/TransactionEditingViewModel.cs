using Db;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class TransactionEditingViewModel : WindowViewModelBase, IDisposable
    {
        public ICommand ApplyCommand { get; }
        public ICommand CancelCommand { get; }


        private DateTimeOffset _datetime;
        public DateTimeOffset Datetime
        {
            get => _datetime;
            set => this.RaiseAndSetIfChanged(ref _datetime, value);
        }
        
        private ETransaction _type;
        public ETransaction Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        public IEnumerable<ETransaction> Types
        {
            get => Enum.GetValues(typeof(ETransaction)).Cast<ETransaction>();
        }

        private double? _price;
        public double? Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value);
        }

        private double? _amount;
        public double? Amount
        {
            get => _amount;
            set => this.RaiseAndSetIfChanged(ref _amount, value);
        }

        private string _mote;
        public string Note
        {
            get => _mote;
            set => this.RaiseAndSetIfChanged(ref _mote, value);
        }


        private AssetViewModel _assetVM;
        private ITransaction _transaction;


        public TransactionEditingViewModel(AssetViewModel assetVM, MainViewModel main) : this(main)
        {
            _assetVM = assetVM;
            _transaction = null;

            Datetime = new DateTimeOffset(DateTime.Now);
            Type = ETransaction.Buy;
        }

        public TransactionEditingViewModel(ITransaction transaction, AssetViewModel assetVM, MainViewModel main) : this(main)
        {
            _assetVM = assetVM;
            _transaction = transaction;

            Datetime = new DateTimeOffset(new DateTime(transaction.Datetime));
            Type = transaction.Type;
            Amount = transaction.Amount;
            Price = transaction.Price;
            Note = transaction.Note;
        }

        private TransactionEditingViewModel(MainViewModel main) : base(main)
        {
            ApplyCommand = ReactiveCommand.Create(Apply);
            CancelCommand = ReactiveCommand.Create(Close);
        }


        public void Dispose()
        {
            _assetVM = null;
            _transaction = null;
        }

        private async void Apply()
        {
            if (!Price.HasValue || !Amount.HasValue)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Warning", "Fill Amount and Price!", ButtonEnum.Ok);
                
                await box.ShowAsync();
                return;
            }

            if (_transaction is null)
            {
                ITransaction transaction = _assetVM.Model.CreateTransaction(Datetime.Ticks, Type, Price.Value, Amount.Value);
                transaction.Note = Note;
                _assetVM.AddTransaction(transaction);
            }
            else
            {
                _transaction.Datetime = Datetime.Ticks;
                _transaction.Type = Type;
                _transaction.Price = Price.Value;
                _transaction.Amount = Amount.Value;
                _transaction.Note = Note;
            }

            _assetVM.RefreshTransactions();

            Close();
        }

        private void Close()
        {
            TurnBack();
        }
    }
}
