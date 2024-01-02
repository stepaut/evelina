using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    public interface IBook
    {
        string Name { get; }

        long CreateDate { get; }


        IList<IEntry> GetAllTransactions();

        IList<ICategory> GetAllCategories();
    }
}
