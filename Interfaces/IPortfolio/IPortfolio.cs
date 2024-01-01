using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    public interface IPortfolio : IItem
    {
        string Name { get; }
        string Description { get; }


        IList<string> GetAssetNames();
        IAsset GetAsset(string assetName);
        IAsset CreateAsset(string assetName);

        Task<bool> Save();
        Task<bool> SaveAs(string path);

        void ChangeName(string newName);
        void ChangeDescription(string newDescription);
    }
}
