namespace Db
{
    public static class PortfolioFactory
    {
        public static IPortfolio CreatePortfolio(string name)
        {
            var now = DateTime.Now.Ticks;
            var uid = Guid.NewGuid().ToString();

            Portfolio portfolio = new Portfolio(uid, now);

            portfolio.Name = name;

            return portfolio;
        }

        public static IPortfolio ReadPortfolio(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            try
            {
                using (PortfolioContext db = new PortfolioContext(path))
                {
                    List<Thing> things = db.Things.ToList();
                    Portfolio portfolio = ReadThings(things, path);

                    return portfolio;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static Portfolio ReadThings(List<Thing> things, string path)
        {
            Thing portfolioThing = null;
            List<Thing> assetThings = new();
            List<Thing> transactionThings = new();
            List<Thing> targetThings = new();

            foreach (Thing thing in things)
            {
                if (thing.Level == EItemLevel.Portfolio)
                {
                    if (portfolioThing != null)
                    {
                        throw new Exception();
                    }
                    portfolioThing = thing;
                }
                else if (thing.Level == EItemLevel.Asset)
                {
                    assetThings.Add(thing);
                }
                else if (thing.Level == EItemLevel.Transaction)
                {
                    transactionThings.Add(thing);
                }
                else if (thing.Level == EItemLevel.Target)
                {
                    targetThings.Add(thing);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            Portfolio portfolio = new Portfolio(portfolioThing.Id, portfolioThing.CreationDate, path);
            portfolio.FromJson(portfolioThing.JsonValue);

            foreach (Thing thing in assetThings)
            {
                Asset asset = new Asset(thing.Id, thing.CreationDate, thing.ParentId, portfolio);
                asset.FromJson(thing.JsonValue);
                portfolio.AddAsset(asset);
            }

            var assets = portfolio.GetAssets();

            foreach (Thing thing in transactionThings)
            {
                Asset parent = assets.FirstOrDefault(x => x.Id == thing.ParentId) as Asset;

                if (parent is null)
                {
                    throw new Exception();//TODO
                }

                Transaction transaction = new Transaction(thing.Id, thing.CreationDate, thing.ParentId, parent);
                transaction.FromJson(thing.JsonValue);

                parent.AddTransaction(transaction);
            }

            foreach (Thing thing in targetThings)
            {
                Asset parent = assets.FirstOrDefault(x => x.Id == thing.ParentId) as Asset;

                if (parent is null)
                {
                    throw new Exception();//TODO
                }

                Target target = new Target(thing.Id, thing.CreationDate, thing.ParentId);
                target.FromJson(thing.JsonValue);

                parent.AddTarget(target);
            }

            return portfolio;
        }

        internal static bool SavePortfolio(IPortfolio portfolio, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            try
            {
                using (PortfolioContext db = new PortfolioContext(path))
                {
                    Dictionary<string, Thing> oldThings = db.Things.ToDictionary(x => x.Id, x => x);
                    Dictionary<string, Thing> newThings = new();

                    Thing thisThing = new Thing(portfolio);
                    newThings[thisThing.Id] = thisThing;

                    foreach (Asset asset in portfolio.GetAssets())
                    {
                        Thing thing = new Thing(asset);
                        newThings[thing.Id] = thing;

                        foreach (Transaction transaction in asset.GetTransactions())
                        {
                            Thing thingTr = new Thing(transaction);
                            newThings[thingTr.Id] = thingTr;
                        }
                    }

                    foreach (var newThing in newThings.Values)
                    {
                        if (oldThings.TryGetValue(newThing.Id, out var old))
                        {
                            old.JsonValue = newThing.JsonValue;
                            db.Things.Update(old);
                        }
                        else
                        {
                            db.Things.Add(newThing);
                        }
                    }

                    foreach (var oldThing in oldThings.Values)
                    {
                        if (!newThings.TryGetValue(oldThing.Id, out _))
                        {
                            db.Things.Remove(oldThing);
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
