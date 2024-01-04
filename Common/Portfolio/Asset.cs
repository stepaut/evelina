using System.Text.Json;

namespace Db
{
    internal class Asset : Item, IAsset
    {
        public string Name { get; set; }
        public double? TargetVolume { get; set; }
        public double? TargetSellPrice { get; set; }
        public double? TargetShare { get; set; }
        public double Volume { get; set; }
        public double SellPrice { get; set; }
        public double Share { get; set; }

        private List<Transaction> _transactions;
        private Portfolio _parent;


        internal Asset(string id, long creationDate, string parentId, Portfolio parent) : base(id, creationDate, parentId)
        {
            Level = EItemLevel.Asset;
            _transactions = new List<Transaction>();
            _parent = parent;
        }


        public override string ToJson()
        {
            AssetDTO dto = new AssetDTO()
            {
                Name = Name,
                TargetVolume = TargetVolume,
                TargetSellPrice = TargetSellPrice,
                TargetShare = TargetShare,
            };

            return JsonSerializer.Serialize(dto);
        }

        public override void FromJson(string json)
        {
            AssetDTO dto = JsonSerializer.Deserialize<AssetDTO>(json);

            Name = dto.Name;
            TargetVolume = dto.TargetVolume;
            TargetSellPrice = dto.TargetSellPrice;
            TargetShare = dto.TargetShare;
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

            Transaction transaction = new Transaction(uid, now, Id, this);
            transaction.Datetime = datetime;
            transaction.Type = type;
            transaction.Price = price;
            transaction.Amount = amount;

            _transactions.Add(transaction);
            _parent.UpdateStat();

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
            _parent.UpdateStat();
        }

        #region internal
        internal void AddTransaction(Transaction transaction)
        {
            if (transaction.ParentId != Id)
            {
                throw new InvalidOperationException();
            }

            _transactions.Add(transaction);
            _parent.UpdateStat();
        }

        internal void UpdateStat()
        {
            _parent.UpdateStat();
        }
        #endregion
    }
}
