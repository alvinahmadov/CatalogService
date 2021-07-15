using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using Catalog.Common;
using Catalog.Common.Repository;
using Catalog.Common.Service;

namespace Catalog.Client
{
	using CartItems = List<ShoppingCartItem>;

	public class GridItemCollection
	{
		#region Public properties

		public Int32 Count { get => gridItems.Count; }

		public static Telerik.WinControls.UI.RadButton StatusButton { get; set; }

		#endregion

		#region Initializers

		public GridItemCollection()
		{
			InitShoppingCart();
		}

		private static void InitShoppingCart()
		{
			if (shoppingCart != null)
				return;

			try
			{
				shoppingCart = Repository.Context
										 .ShoppingCarts
										 .Include(c => c.ShoppingCartItems)
										 .SingleOrDefault();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error occurred while retrieving ShoppingCart: {ex.Message}\n" +
									$"StackTrace:\n{ex.StackTrace}");
				System.Diagnostics.Debug.WriteLine($"Detailed message:\n{ex.InnerException?.Message}");
			}
			GridItem.ShoppingCart = shoppingCart;
		}

		private static void InitShoppingCartItem(GridItem gridItem)
		{
			Int32 productID = gridItem.ProductID;
			var cartItem = shoppingCartItems.Find(si => si.ProductID == productID);

			try
			{
				if (cartItem is null)
				{
					cartItem = Repository.Context
										 .ShoppingCartItems
										 .Where(item => productID == item.ProductID)
										 .SingleOrDefault();
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error occurred while retrieving ShoppingCartItem: {ex.Message}\n" +
									$"StackTrace:\n{ex.StackTrace}");
				System.Diagnostics.Debug.WriteLine($"Detailed message:\n{ex.InnerException?.Message}");
			}

			var product = Repository.Context
									.Products
									.Where(p => p.ProductID == productID)
									.SingleOrDefault();

			if (cartItem is null)
			{
				cartItem = new ShoppingCartItem(shoppingCart, product);
				cartItem.Save();
				shoppingCartItems.Add(cartItem);
			}
			else
			{
				if (cartItem.UnitPrice != product.Price)
					cartItem.UnitPrice = product.Price;
			};

			gridItem.ShoppingCartItem = cartItem;
		}

		public void Init()
		{
			foreach (var inventory in MainRepository.InventoriesCache)
				Add(inventory.ProductID, inventory.Product.Price);
		}

		#endregion

		public bool Exists(Int32 productID)
		{
			return this.gridItems.Exists(i => productID == i.ProductID);
		}

		public GridItem Get(Int32 productID)
		{
			var gridItem = gridItems.Find(qi => productID == qi.ProductID);
			return gridItem;
		}

		public GridItem Get(Int32 productID, out Boolean exists)
		{
			var gridItem = Get(productID);
			exists = gridItem != null;
			return gridItem;
		}

		public Int32 Change(Int32 productID, Int32 newValue)
		{
			var gridItem = Get(productID, out Boolean exists);
			Int32 quantity = 0;

			if (exists)
			{
				var oldValue = gridItem.Quantity;
				if (oldValue == newValue)
					return newValue;

				var offset = Math.Abs(newValue - oldValue);

				quantity = oldValue < newValue
					? Increment(productID, offset)
					: Decrement(productID, offset);
			}
			return quantity;
		}

		public Int32 Increment(Int32 productID)
		{
			return Increment(productID, 1);
		}

		public Int32 Increment(Int32 productID, Int32 newValue = 0)
		{
			var gridItem = Get(productID, out bool exists);

			if (exists)
			{
				var oldValue = gridItem.Quantity;
				if (newValue == 0)
					newValue = -gridItem.Quantity;
				gridItem.Quantity += newValue;
				Update(gridItem, oldValue);
			}
			return gridItem.Quantity;
		}

		public Int32 Decrement(Int32 productID)
		{
			return Decrement(productID, 1);
		}

		public Int32 Decrement(Int32 index, Int32 newValue = 0)
		{
			var gridItem = Get(index, out bool exists);

			if (exists)
			{
				var oldValue = gridItem.Quantity;

				if (newValue == 0)
					newValue = gridItem.Quantity;

				gridItem.Quantity -= newValue;
				Update(gridItem, oldValue);
			}

			return gridItem.Quantity;
		}

		private void Add(Int32 productID, Decimal unitPrice)
		{
			if (!Exists(productID))
			{
				var gridItem = new GridItem(productID, unitPrice);

				this.gridItems.Add(gridItem);
				InitShoppingCartItem(gridItem);
			}
		}

		private void Update(GridItem gridItem, Int32 valueOrDiff = 0)
		{
			try
			{
				var unitQuantity = gridItem.Quantity - valueOrDiff;

				GridItem.ShoppingCart.TotalQuantity += unitQuantity;
				GridItem.ShoppingCart.TotalPrice += gridItem.UnitPrice * unitQuantity;

				if (GridItem.ShoppingCart.TotalQuantity <= 0)
				{
					GridItem.ShoppingCart.TotalQuantity = 0;
					GridItem.ShoppingCart.TotalPrice = 0.00M;
				}

				var totalPrice = GridItem.ShoppingCart.TotalPrice;
				var formattedPrice = totalPrice >= 0.00M ? totalPrice : 0.00M;

				StatusButton.Text = String.Format(
								MESSAGE.Gui.ORDERS_STATUS_TEMPLATE,
								GridItem.ShoppingCart.TotalQuantity,
								CultureConfig.CurrencyInfo.Format(formattedPrice)
							);

				gridItem.UpdateAndSave();
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
		}

		#region Fields

		public const String STRING_TEMPLATE
			= "<html>В корзине <b>{0}</b> позиций на сумму <b>{1} ₽</b>";

		public readonly List<GridItem> gridItems = new List<GridItem>();

		public static ShoppingCart shoppingCart;

		private static readonly CartItems shoppingCartItems = new CartItems();

		#endregion
	}
}