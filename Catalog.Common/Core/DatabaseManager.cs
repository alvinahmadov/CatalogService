using Catalog.Common.Service;

namespace Catalog.Common.Repository
{
	public static class DatabaseManager
	{
		public static ShopEntities Context 
		{
			get {
				if (dbContext == null)
					dbContext = new ShopEntities();
				return dbContext;
			}
		}

		static DatabaseManager()
		{
			dbContext = new ShopEntities();
		}

		private static ShopEntities dbContext;
	}

}