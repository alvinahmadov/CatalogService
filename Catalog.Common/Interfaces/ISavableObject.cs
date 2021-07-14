﻿using System;
using System.Collections.Generic;

namespace Catalog.Common
{
    public interface ISavableObject
    {
        void Save(bool isAddingItem = true, bool commit = false);

        bool Update(object entity, bool commit = false);

        void Delete();
        
        void Cancel();
    }
}
