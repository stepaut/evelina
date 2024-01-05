using System.Text.Json;

namespace Db
{
    internal class Target : Item, ITarget
    {
        public double Price { get; set; }
        public double Volume { get; set; }


        public Target(string id, long creationDate, string parentId) : base(id, creationDate, parentId)
        {
            Level = EItemLevel.Target;
        }


        #region IItem
        public override string ToJson()
        {
            TargetDTO dto = new TargetDTO()
            {
                Price = Price,
                Volume = Volume,
            };

            return JsonSerializer.Serialize(dto);
        }

        public override void FromJson(string json)
        {
            TargetDTO dto = JsonSerializer.Deserialize<TargetDTO>(json);

            Price = dto.Price; 
            Volume = dto.Volume;
        }
        #endregion
    }
}
