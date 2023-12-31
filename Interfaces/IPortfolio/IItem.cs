namespace Db
{
    public interface IItem
    {
        string Id { get; }

        long CreationDate { get; }

        string ToJson();
    }
}
