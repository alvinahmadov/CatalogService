using System;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

using Telerik.WinControls.Data;

using ProductInventoryDataQuery = System.Data.Entity.Infrastructure.DbQuery<Catalog.Common.Service.ProductInventory>;

namespace Catalog.Client
{
	public enum ProductInventorySortMode
	{
		None = 0,
		ArticleNumber,
		Name,
		StockLevel1,
		StockLevel2,
		Price
	}

	public static class SortHelper
	{
		public static DbQuery<T> Sort<T>(IQueryable<T> queryable, SortDescriptorCollection sortDescriptors)
		{
			bool isFirst = true;

			if (sortDescriptors != null)
			{
				foreach (var descriptor in sortDescriptors)
				{
					string methodName = string.Empty;

					var property = queryable.ElementType.GetProperty(descriptor.PropertyName);
					var parameter = Expression.Parameter(queryable.ElementType, "srt");
					var propertyAccess = Expression.MakeMemberAccess(parameter, property);
					var expression = Expression.Lambda(propertyAccess, parameter);

					if (isFirst)
					{
						methodName = descriptor.Direction == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";
						isFirst = false;
					}
					else
					{
						methodName = descriptor.Direction == ListSortDirection.Ascending ? "ThenBy" : "ThenByDescending";
					}

					var exp = Expression.Call(typeof(Queryable), methodName, new[] { queryable.ElementType, expression.Body.Type }, queryable.Expression, Expression.Quote(expression));
					queryable = queryable.Provider.CreateQuery(exp) as DbQuery<T>;
				}
			}
			return (DbQuery<T>) queryable;
		}

		public static ProductInventoryDataQuery
		ProductInventorySort(
			ProductInventoryDataQuery queryable,
			ProductInventorySortMode sortMode,
			SortDescriptorCollection sortDescriptors
		)
		{
			if (sortMode == ProductInventorySortMode.None)
				queryable = Sort(queryable, sortDescriptors);
			else
			{
				foreach (var descriptor in sortDescriptors)
				{
					switch (sortMode)
					{
						case ProductInventorySortMode.Name:
							return SortOrder(queryable, srt => srt.Product.Name, descriptor.Direction);
						case ProductInventorySortMode.Price:
							return SortOrder(queryable, srt => srt.Product.Price, descriptor.Direction);
						case ProductInventorySortMode.ArticleNumber:
							return SortOrder(queryable, srt => srt.Product.ArticleNumber, descriptor.Direction);
					}
				}
			}
			return queryable;
		}

		private static DbQuery<TSource>
		SortOrder<TSource, TKey>(
			IQueryable<TSource> query,
			Expression<Func<TSource, TKey>> expression,
			ListSortDirection direction
		)
		{
			if (direction == ListSortDirection.Ascending)
				return query.OrderBy(expression) as DbQuery<TSource>;
			else
				return query.OrderByDescending(expression) as DbQuery<TSource>;
		}
	}
}
