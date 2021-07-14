using System;
using System.Diagnostics;

namespace Catalog.Common.Service
{
	public abstract class EntityModel : ISavableObject
	{
		public static void Commit(bool async = false)
		{
			Debug.WriteLine("Committing");
			if (async)
				Repository.Repository.SaveChangesAsync();
			else
				Repository.Repository.SaveChanges();
		}

		public virtual void Cancel()
		{
		}

		public virtual void Delete()
		{
			throw new NotImplementedException();
		}

		public virtual void Save(bool isAddingItem = true, bool commit = false)
		{
			throw new NotImplementedException();
		}

		public virtual bool Update(object entity, bool commit = false)
		{
			throw new NotImplementedException();
		}
	}
}