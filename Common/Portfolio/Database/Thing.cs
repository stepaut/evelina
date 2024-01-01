using System.ComponentModel.DataAnnotations.Schema;

namespace Db
{
    internal class Thing
    {
        public string Id { get; set; }
        [NotMapped]
        public string ParentId { get; set; }
        public long CreationDate { get; set; }
        public string JsonValue { get; set; }
        public EItemLevel Level { get; set; }

        public Thing(IItem item)
        {
            Id = item.Id;
            ParentId = item.ParentId;
            CreationDate = item.CreationDate;
            JsonValue = item.ToJson();
            Level = item.Level;
        }

        public Thing() { }
    }
}
