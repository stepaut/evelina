using System.Text.Json;

namespace Db
{
    internal class Transaction : ITransaction
    {
        public long Datetime { get; internal set; }

        public ETransaction Type { get; internal set; }

        public double Price { get; internal set; }

        public double Amount { get; internal set; }

        public string Note { get; internal set; }

        public string Id { get; }

        public string ParentId { get; }

        public long CreationDate { get; }

        public EItemLevel Level { get; }


        public Transaction(string id, long creationDate, string parentId)
        {
            Id = id;
            CreationDate = creationDate;
            ParentId = parentId;
            Level = EItemLevel.Transaction;
        }


        public void FromJson(string json)
        {
            TransactionDTO dto = JsonSerializer.Deserialize<TransactionDTO>(json);

            Datetime = dto.Datetime;
            Type = dto.Type;
            Price = dto.Price;
            Amount = dto.Amount;
            Note = dto.Note;
        }

        public string ToJson()
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
