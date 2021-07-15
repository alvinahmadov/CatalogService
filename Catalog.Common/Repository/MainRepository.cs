using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

using Catalog.Common.Service;

namespace Catalog.Common.Repository
{
	public partial class MainRepository : Repository
	{
		public static DbQuery<ShoppingCart> GetShoppingCarts()
		{
			var shoppingCarts =
				Context.ShoppingCarts.Include(c => c.ShoppingCartItems);

			return shoppingCarts as DbQuery<ShoppingCart>;
		}

		public static IQueryable<TransactionHistory> GetTransactionHistories()
		{
			var transactionHistories =
				Context.TransactionHistories;

			return transactionHistories;
		}

		public static void GetProductInventories(Action<DbQuery<Inventory>> action)
		{
			ExecuteQueryAsync(GetProductInventoriesAsync(), action);
		}

		private static async Task<DbQuery<Inventory>> GetProductInventoriesAsync()
		{
			return await Task.Run(() =>
				Context.Inventories
				.Include(p => p.Product) as DbQuery<Inventory>);
		}


		public static Product GetProduct(int id)
		{
			return Context.Products.Find(id);
			//return ProductsCache.FirstOrDefault(p => p.ProductID == id);
		}
	}
}
