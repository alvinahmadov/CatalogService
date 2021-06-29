using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

using Catalog.Common.Service;

namespace Catalog.Common.Repository
{
	public abstract class Repository
	{
		public static ShopEntities Context { get => ConnectionManager.Context; }

		static Repository()
		{
		}

		protected static async void ExecuteQueryAsync<T>(
			Task<IEnumerable<T>> task,
			Action<IEnumerable<T>> callback
		)
		{
			var result = await task;
			callback?.Invoke(result);
		}

		protected static async void ExecuteQueryAsync<T>(
			Task<T> task,
			Action<T> callback
		)
		{
			var result = await task;
			callback?.Invoke(result);
		}

		public static void Update<TEntity>(in TEntity entity)
		{
			try
			{
				//Context.UpdateObject(entity);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: Update => ", ex.Message);
			}
		}

		public async static void SaveChangesAsync(Action callback = null)
		{
			await Task.Run(() => SaveChanges(callback));
		}

		public static void UpdateAndSaveAsync(object entity)
		{
			try
			{
				Update(entity);
				SaveChangesAsync();
			}
			catch (Exception ex)
			{

				Debug.WriteLine("Exception", ex.Message);
			}
		}

		public static void SaveChanges(Action callback = null)
		{
			try
			{
				Context.SaveChanges();
				callback?.Invoke();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error occurred while saving changes: {ex.Message}\n" +
								$"StackTrace:\n{ex.StackTrace}\n" +
								$"Detailed message:\n{ex.InnerException?.Message}");

			}

		}
	}
}
