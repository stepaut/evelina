using Db;
using System;

namespace evelina.ViewModels
{
    public class AssetViewModel : ViewModelBase, IDisposable
    {
        public string Name => Model?.Name;


        internal IAsset Model { get; private set; }


        public AssetViewModel(IAsset model)
        {
            Model = model;
        }


        public void Dispose()
        {
            Model = null;
        }
    }
}
