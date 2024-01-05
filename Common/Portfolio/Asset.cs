using System.Linq;
using System.Text.Json;

namespace Db
{
    internal class Asset : Item, IAsset
    {
        public string Name { get; set; }

        private double? _targetVolume;
        public double? TargetVolume
        {
            get => _targetVolume;
            set
            {
                if (_targetVolume != value)
                {
                    _targetVolume = value;
                    _parent.UpdateStat();
                }
            }
        }

        private double? _targetSellPrice;
        public double? TargetSellPrice
        {
            get => _targetSellPrice;
            set
            {
                if (_targetSellPrice != value)
                {
                    _targetSellPrice = value;
                    _parent.UpdateStat();
                }
            }
        }

        private double? _targetShare;
        public double? TargetShare
        {
            get => _targetShare;
            set
            {
                if (_targetShare != value)
                {
                    _targetShare = value;
                    _parent.UpdateStat();
                }
            }
        }

        public IAssetStat Stat => _stat;

        private List<Transaction> _transactions;
        private List<Target> _targets;
        private Portfolio _parent;
        private AssetStat _stat;


        internal Asset(string id, long creationDate, string parentId, Portfolio parent) : base(id, creationDate, parentId)
        {
            Level = EItemLevel.Asset;
            _transactions = new List<Transaction>();
            _parent = parent;
            _stat = new AssetStat();
        }


        #region IItem
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
        #endregion

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

        public IList<ITarget> GetTargets()
        {
            IList<ITarget> targets = new List<ITarget>();
            foreach (ITarget target in _targets)
            {
                targets.Add(target);
            }

            return targets;
        }

        public ITarget CreateTarget(double price, double volume)
        {
            var now = DateTime.Now.Ticks;
            string uid = Guid.NewGuid().ToString();

            Target target = new Target(uid, now, Id);
            target.Volume = volume;
            target.Price = price;

            _targets.Add(target);
            _parent.UpdateStat();

            return target;
        }

        public void DeleteTarget(ITarget target)
        {
            Target real = target as Target;

            if (!_targets.Contains(real))
            {
                throw new InvalidOperationException();
            }

            _targets.Remove(real);
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

        internal void AddTarget(Target target)
        {
            if (target.ParentId != Id)
            {
                throw new InvalidOperationException();
            }

            _targets.Add(target);
            _parent.UpdateStat();
        }

        internal void UpdateStat()
        {
            _parent.UpdateStat();
        }
        #endregion
    }
}
