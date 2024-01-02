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

        private List<Transaction> _transactions;


        internal Asset(string id, long creationDate, string parentId)
        {
            Id = id;
            CreationDate = creationDate;
            ParentId = parentId;

            _transactions = new List<Transaction>();
        }


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

        public IList<ITransaction> GetTransactions()
        {
            IList<ITransaction> transactions = new List<ITransaction>();
            foreach (var transaction in _transactions)
            {
                transactions.Add(transaction);
            }
            return transactions;
        }

        public ITransaction CreateTransaction(long datetime, ETransaction type, double price, double amount, string note = null)
        {
            var now = DateTime.Now.Ticks;
            string uid = Guid.NewGuid().ToString();

            Transaction transaction = new Transaction(uid, now, Id);
            transaction.Datetime = datetime;
            transaction.Type = type;
            transaction.Price = price;
            transaction.Amount = amount;
            transaction.Note = note;

            _transactions.Add(transaction);
            return transaction;
        }
    }
}
