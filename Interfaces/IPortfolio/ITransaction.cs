namespace Db
{
    public interface ITransaction : IItem
    {
        long Datetime { get; }

        ETransaction Type { get; }

        double Price { get; }

        double Amount { get; }

        string Note { get; }
    }
}
