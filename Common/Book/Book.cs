using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    internal class Book : IBook
    {
        public string Name => throw new NotImplementedException();

        public long CreateDate => throw new NotImplementedException();

        public IList<ICategory> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public IList<IEntry> GetAllTransactions()
        {
            throw new NotImplementedException();
        }
    }
}
