using System.Text.Json;

namespace Db
{
    internal class Transaction : Item, ITransaction
    {
        public long Datetime { get; set; }

        public ETransaction Type { get; set; }

        public double Price { get; set; }

        public double Amount { get; set; }

        public string Note { get; set; }


        public Transaction(string id, long creationDate, string parentId) : base(id, creationDate, parentId)
        {
            Level = EItemLevel.Transaction;
        }


        public override void FromJson(string json)
        {
            TransactionDTO dto = JsonSerializer.Deserialize<TransactionDTO>(json);

            Datetime = dto.Datetime;
            Type = dto.Type;
            Price = dto.Price;
            Amount = dto.Amount;
            Note = dto.Note;
        }

        public override string ToJson()
        {
            TransactionDTO dto = new TransactionDTO()
            {
                Datetime = Datetime,
                Type = Type,
                Price = Price,
                Amount = Amount,
                Note = Note,
            };

            return JsonSerializer.Serialize(dto);
        }
    }
}
