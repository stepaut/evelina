namespace Db
{
    internal class Thing
    {
        public string Id { get; set; }
        public long CreationDate { get; set; }
        public string JsonValue { get; set; }

        public Thing(IItem item)
        {
            Id = item.Id;
            CreationDate = item.CreationDate;
            JsonValue = item.ToJson();
        }

        public Thing() { }
    }
}
