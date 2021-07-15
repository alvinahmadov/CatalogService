using Catalog.Common.Service;

namespace Catalog.Common.Repository
{
	public static class DatabaseManager
	{
		public static CatalogContext Context 
		{
			get {
				if (dbContext == null)
					dbContext = new CatalogContext();
				return dbContext;
			}
		}

		static DatabaseManager()
		{
			dbContext = new CatalogContext();
		}

		private static CatalogContext dbContext;
	}

}