using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    public class Asset : IAsset
    {
        public string Name { get; internal set; }

        public string Id { get; }

        public long CreationDate { get; }

        internal Asset(string id, long creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
