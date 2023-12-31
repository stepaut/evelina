using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    public static class PortfolioCreator
    {
        public static async Task<IPortfolio> CreatePortfolio(string name)
        {
            var now = DateTime.Now.Ticks;
            var uid = Guid.NewGuid().ToString();

            Portfolio portfolio = new Portfolio(uid, now);

            portfolio.Name = name;

            return portfolio;
        }
    }
}
