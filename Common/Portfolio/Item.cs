namespace Db
{
    internal class Item : IItem
    {
        public string Id { get; }

        public long CreationDate { get; }

        public string ParentId { get; }

        public EItemLevel Level { get; protected set; }


        public Item(string id, long creationDate, string parentId)
        {
            Id = id;
            CreationDate = creationDate;
            ParentId = parentId;
        }


        public virtual void FromJson(string json)
        {
            throw new NotImplementedException();
        }

        public virtual string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
