using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Client
{
    public class DataDialogCommandEventArgs : EventArgs
    {
        public bool SaveChanges { get; private set; }

        public DataDialogCommandEventArgs(bool saveChanges)
        {
            this.SaveChanges = saveChanges;
        }
    }
}
