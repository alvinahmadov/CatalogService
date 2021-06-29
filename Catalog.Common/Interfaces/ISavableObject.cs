using System;
using System.Collections.Generic;

namespace Catalog.Common
{
    public interface ISavableObject
    {
        void Save(bool isAddingItem = true, bool commit = false);

        void Update(object entity, bool commit = false);

        void Delete();
        
        void Cancel();
    }
}
