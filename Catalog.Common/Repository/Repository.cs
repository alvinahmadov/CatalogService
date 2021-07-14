using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

using Catalog.Common.Service;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace Catalog.Common.Repository
{
	public abstract class Repository
	{
		public static ShopEntities Context => DatabaseManager.Context; 

		static Repository()
		{
		}

		public static int DropConstraint(string constraintName, string tableName)
		{
			return Context.Database.ExecuteSqlCommand(
				$"IF OBJECT_ID(N'[dbo].[{constraintName}]', 'F') IS NOT NULL " +
					$"ALTER TABLE[dbo].[{tableName}] DROP CONSTRAINT[{constraintName}]"
				);
		}

		public static void DropTable(string tableName)
		{
			Context.Database.ExecuteSqlCommand($"DROP TABLE [dbo].[{tableName}];");
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
				Context.SaveChanges();
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

		public static void SaveChanges(Action callback = null)
		{
			try
			{
				Context.SaveChanges();
				callback?.Invoke();
			} catch (DbUpdateException dbe)
			{
				Debug.WriteLine($"1. Error occurred while saving changes: {dbe.Message}\n" +
								$"StackTrace:\n{dbe.StackTrace}\n" +
								$"Detailed message:\n{dbe.InnerException?.Message}");
			}
			catch (DbEntityValidationException ev)
			{
				foreach (var eve in ev.EntityValidationErrors)
				{
					Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);
					}
				}
				throw;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"2. Error occurred while saving changes: {ex.Message}\n" +
								$"StackTrace:\n{ex.StackTrace}\n" +
								$"Detailed message:\n{ex.InnerException?.Message}");

			}

		}
	}
}
