namespace Db
{
    public interface ITransaction : IItem
    {
        long Datetime { get; set; }
        ETransaction Type { get; set; }
        double Price { get; set; }
        double Amount { get; set; }
        string Note { get; set; }
    }
}
