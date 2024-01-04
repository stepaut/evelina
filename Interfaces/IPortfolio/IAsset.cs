namespace Db
{
    public interface IAsset : IItem
    {
        #region required
        string Name { get; set; }
        #endregion

        #region additional
        double? TargetVolume { get; set; }
        double? TargetSellPrice { get; set; }
        double? TargetShare { get; set; }
        #endregion


        IList<ITransaction> GetTransactions();

        ITransaction CreateTransaction(long datetime, ETransaction type, double price, double amount);

        void DeleteTransaction(ITransaction transaction);
    }
}
