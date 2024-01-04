using System.Text.Json;

namespace Db
{
    internal class Asset : Item, IAsset
    {
        public string Name { get; set; }
        public double? TargetVolume { get; set; }
        public double? TargetSellPrice { get; set; }
        public double? TargetShare { get; set; }

        private List<Transaction> _transactions;


        internal Asset(string id, long creationDate, string parentId) : base(id, creationDate, parentId)
        {
            Level = EItemLevel.Asset;
            _transactions = new List<Transaction>();
        }


        public override string ToJson()
        {
            AssetDTO dto = new AssetDTO()
            {
                Name = Name,
            };

            return JsonSerializer.Serialize(dto);
        }

        public override void FromJson(string json)
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

        public ITransaction CreateTransaction(long datetime, ETransaction type, double price, double amount)
        {
            var now = DateTime.Now.Ticks;
            string uid = Guid.NewGuid().ToString();

            Transaction transaction = new Transaction(uid, now, Id);
            transaction.Datetime = datetime;
            transaction.Type = type;
            transaction.Price = price;
            transaction.Amount = amount;

            _transactions.Add(transaction);
            return transaction;
        }

        public void DeleteTransaction(ITransaction transaction)
        {
            Transaction real = transaction as Transaction;

            if (!_transactions.Contains(real))
            {
                throw new InvalidOperationException();
            }

            _transactions.Remove(real);
        }

        internal void AddTransaction(Transaction transaction)
        {
            if (transaction.ParentId != Id)
            {
                throw new InvalidOperationException();
            }

            _transactions.Add(transaction);
        }
    }
}
