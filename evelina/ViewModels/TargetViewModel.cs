using Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evelina.ViewModels
{
    public class TargetViewModel : ViewModelBase, IDisposable
    {
        internal ITarget Model { get; private set; }


        public TargetViewModel(ITarget model)
        {
            Model = model;
        }


        public void Dispose()
        {
            Model = null;
        }
    }
}
