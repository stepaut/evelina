using System.Text.Json;

namespace Db
{
    internal class Asset : IAsset
    {
        public string Name { get; internal set; }

        public string Id { get; }

        public long CreationDate { get; }

        public string ParentId { get; }

        public EItemLevel Level => EItemLevel.Asset;


        internal Asset(string id, long creationDate, string parentId)
        {
            Id = id;
            CreationDate = creationDate;
            ParentId = parentId;
        }

        public Asset() { }


        public string ToJson()
        {
            AssetDTO dto = new AssetDTO()
            {
                Name = Name,
            };

            return JsonSerializer.Serialize(dto);
        }

        public void FromJson(string json)
        {
            AssetDTO dto = JsonSerializer.Deserialize<AssetDTO>(json);

            Name = dto.Name;
        }
    }
}
