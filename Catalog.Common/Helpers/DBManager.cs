using System;

using Catalog.Common.Service;

namespace Catalog.Common.Repository
{
	public static class ConnectionManager
	{
		public static ShopEntities Context 
		{
			get {
				if (dbContext == null)
					dbContext = new ShopEntities();
				Reconnect();
				return dbContext;
			}
		}

		static ConnectionManager()
		{
			dbContext = new ShopEntities();
		}

		private static void Reconnect()
		{
			if (dbContext != null)
			{
				if (dbContext.Database.Connection.State != System.Data.ConnectionState.Open)
					dbContext.Database.Connection.Open();
			}
		}

		private static ShopEntities dbContext;
	}

}