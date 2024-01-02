using Db;
using System;

namespace evelina.ViewModels
{
    public class TransactionViewModel : ViewModelBase, IDisposable
    {
        public string DisplayName => $"{Model?.Type}: {Model?.Amount}";

        public string DisplayDesctiption => $"{(Model != null ? new DateTime(Model.Datetime) : 0)}";


        internal ITransaction Model { get; private set; }


        public TransactionViewModel(ITransaction model)
        {
            Model = model;
        }


        public void Dispose()
        {
            Model = null;
        }
    }
}
