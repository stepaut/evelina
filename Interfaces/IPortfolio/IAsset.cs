using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    public interface IAsset : IItem
    {
        string Name { get; }
    }
}
