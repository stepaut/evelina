namespace Db
{
    public interface IAsset : IItem
    {
        string Name { get; set; }


        IList<ITransaction> GetTransactions();

        ITransaction CreateTransaction(long datetime, ETransaction type, double price, double amount, string note = null);

        void DeleteTransaction(ITransaction transaction);
    }
}
