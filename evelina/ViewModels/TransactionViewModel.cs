using Db;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class TransactionViewModel : ViewModelBase, IDisposable
    {
        internal delegate void DeleteMe(TransactionViewModel transactionVM);
        internal event DeleteMe DeleteMeEvent;
        
        internal delegate void EditMe(TransactionViewModel transactionVM);
        internal event EditMe EditMeEvent;

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }


        public string DisplayName => $"{Model?.Type}: {Model?.Amount}";

        public string DisplayDesctiption => $"{(Model != null ? new DateTime(Model.Datetime) : 0)}";

        internal long? Datetime => Model?.Datetime;


        internal ITransaction Model { get; private set; }


        public TransactionViewModel(ITransaction model)
        {
            Model = model;

            EditCommand = ReactiveCommand.Create(Edit);
            DeleteCommand = ReactiveCommand.Create(Delete);
        }


        public void Dispose()
        {
            Model = null;
        }

        private void Edit()
        {
            EditMeEvent?.Invoke(this);
        }

        private void Delete()
        {
            DeleteMeEvent?.Invoke(this);
        }
    }
}
