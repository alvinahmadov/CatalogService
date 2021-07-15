using System;

namespace Catalog.Common
{
	public interface IEntity
    {
        void Save(Boolean commit = false);

        Boolean Update(in Object entity);

        void Delete();
        
        void Cancel();
    }
}
