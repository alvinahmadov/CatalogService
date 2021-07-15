using System;
using System.Diagnostics;

namespace Catalog.Common.Service
{
	public abstract class Entity : IEntity
	{
		public static void Commit(Boolean async = false)
		{
			if (async)
				Repository.Repository.SaveChangesAsync();
			else
				Repository.Repository.SaveChanges();
		}

		public virtual void Save(Boolean commit = false)
		{
			throw new NotImplementedException();
		}

		public virtual Boolean Update(in Object entity)
		{
			throw new NotImplementedException();
		}

		public virtual void Cancel()
		{
			throw new NotImplementedException();
		}

		public virtual void Delete()
		{
			throw new NotImplementedException();
		}
	}
}