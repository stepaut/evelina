namespace Db
{
    public interface ITarget : IItem
    {
        double Price { get; set; }
        double Volume { get; set; }

        double Amount => Volume / Price;
    }
}
