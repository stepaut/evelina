using System.Text.Json;
using System.Text.Json.Serialization;

namespace Db
{
    public class Asset : IAsset
    {
        public string Name { get; internal set; }

        [JsonIgnore]
        public string Id { get; }

        [JsonIgnore]
        public long CreationDate { get; }

        [JsonIgnore]
        public string ParentId { get; }

        [JsonIgnore]
        public EItemLevel Level => EItemLevel.Asset;


        internal Asset(string id, long creationDate, string parentId)
        {
            Id = id;
            CreationDate = creationDate;
            ParentId = parentId;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void FromJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}
