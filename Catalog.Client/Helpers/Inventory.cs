﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Entity;

using Telerik.WinControls.UI;

using Catalog.Common.Repository;
using Catalog.Common.Service;
using Catalog.Common.Utils;

namespace Catalog.Client
{
	using CartItems = List<ShoppingCartItem>;

	public enum ShoppingCartStatus
	{
		Pending = 0,
		Ordered = 1
	}

	public class Inventory
	{
		public static ShoppingCart ShoppingCart { get; set; }

		public ShoppingCartItem ShoppingCartItem { get; set; }

		public int ProductID { get; private set; }

		public decimal UnitPrice { get; private set; }

		public int Quantity
		{
			get => ShoppingCartItem.Quantity;
			set
			{
				if (value >= 0)
					ShoppingCartItem.Quantity = value;
			}
		}

		public Inventory(int productID, decimal unitPrice)
		{
			this.ProductID = productID;
			this.UnitPrice = unitPrice;
		}

		public void UpdateAndSave()
		{
			Update(false);
		}

		public void UpdateAndSaveAsync()
		{
			Update(true);
		}

		private void Update(bool async)
		{
			ShoppingCartItem.ModifiedDate = DateTime.Now;
			ShoppingCart.ModifiedDate = DateTime.Now;
			EntityModel.Commit(async);
		}
	}

	public class InventoryCollection
	{
		#region Fields

		public const String STRING_TEMPLATE
			= "<html>В корзине <b>{0}</b> позиций на сумму <b>{1} ₽</b>";

		public readonly List<Inventory> inventories = new List<Inventory>();

		public static ShoppingCart shoppingCart;

		private static readonly CartItems shoppingCartItems = new CartItems();

		#endregion

		#region Public properties

		public int Count { get => inventories.Count; }

		public static RadButton StatusButton { get; set; }

		#endregion

		#region Initializers

		public InventoryCollection()
		{
			InitializeShoppingCart();
		}

		private static void InitializeShoppingCart()
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
				Debug.WriteLine($"Error occurred while retrieving ShoppingCart: {ex.Message}\n" +
									$"StackTrace:\n{ex.StackTrace}");
				Debug.WriteLine($"Detailed message:\n{ex.InnerException?.Message}");
			}
			Inventory.ShoppingCart = shoppingCart;
		}

		private static void InitializeShoppingCartItem(int productID, Inventory inventory)
		{
			var cartItem = shoppingCartItems.Find(si => si.ProductID == productID);

			try
			{
				if (cartItem is null)
					cartItem = Repository.Context.ShoppingCartItems
												 .Where(ci => productID == ci.ProductID)
												 .SingleOrDefault();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error occurred while retrieving ShoppingCartItem: {ex.Message}\n" +
									$"StackTrace:\n{ex.StackTrace}");
				Debug.WriteLine($"Detailed message:\n{ex.InnerException?.Message}");
			}

			if (cartItem is null)
			{
				Decimal unitPrice = 0M;
				var product = Repository.Context.Products
												.Where(p => p.ProductID == productID)
												.SingleOrDefault();
				if (product?.Price != null)
					unitPrice = product.Price;

				cartItem = new ShoppingCartItem
				{
					ProductID = productID,
					ShoppingCart = shoppingCart,
					Quantity = 0,
					UnitPrice = unitPrice,
					rowguid = Guid.NewGuid(),
					DateCreated = DateTime.Now,
					ModifiedDate = DateTime.Now
				};
				cartItem.Save();
			}
			shoppingCart.ShoppingCartItems.Add(cartItem);
			shoppingCart.Save(false);

			shoppingCartItems.Add(cartItem);

			inventory.ShoppingCartItem = cartItem;
		}

		public void Init()
		{
			foreach (var inventory in MainRepository.ProductInventoriesCache)
				Add(inventory.ProductID, inventory.Product.Price);
		}

		#endregion

		public bool Exists(int productID)
		{
			return this.inventories.Exists(i => productID == i.ProductID);
		}

		public Inventory Get(int productID)
		{
			return inventories.Find(qi => productID == qi.ProductID);
		}

		public Inventory Get(int productID, out bool exists)
		{
			var inventory = Get(productID);
			exists = inventory != null;
			return inventory;
		}

		public int Change(int productID, int newValue)
		{
			var inventory = Get(productID, out bool exists);
			int quantity = 0;

			if (exists)
			{
				var oldValue = inventory.Quantity;
				if (oldValue == newValue)
					return newValue;

				var offset = Math.Abs(newValue - oldValue);

				quantity = oldValue < newValue
					? Increment(productID, offset)
					: Decrement(productID, offset);
			}
			return quantity;
		}

		public int Increment(int productID)
		{
			return Increment(productID, 1);
		}

		public int Increment(int productID, int newValue = 0)
		{
			var inventory = Get(productID, out bool exists);

			if (exists)
			{
				var oldValue = inventory.Quantity;
				if (newValue == 0)
					newValue = -inventory.Quantity;
				inventory.Quantity += newValue;
				Update(inventory, oldValue);
			}
			return inventory.Quantity;
		}

		public int Decrement(int productID)
		{
			return Decrement(productID, 1);
		}

		public int Decrement(int index, int newValue = 0)
		{
			var inventory = Get(index, out bool exists);

			if (exists)
			{
				var oldValue = inventory.Quantity;

				if (newValue == 0)
					newValue = inventory.Quantity;

				inventory.Quantity -= newValue;
				Update(inventory, oldValue);
			}

			return inventory.Quantity;
		}

		private void Add(int productID, decimal unitPrice)
		{
			if (!Exists(productID))
			{
				var inventory = new Inventory(productID, unitPrice);
				InitializeShoppingCartItem(productID, inventory);
				this.inventories.Add(inventory);
			}
		}

		private void Update(Inventory inventory, int valueOrDiff = 0)
		{
			try
			{
				var unitQuantity = inventory.Quantity - valueOrDiff;

				Inventory.ShoppingCart.TotalQuantity += unitQuantity;
				Inventory.ShoppingCart.TotalPrice += inventory.UnitPrice * unitQuantity;

				if (Inventory.ShoppingCart.TotalQuantity <= 0)
				{
					Inventory.ShoppingCart.TotalQuantity = 0;
					Inventory.ShoppingCart.TotalPrice = 0.00M;
				}

				var totalPrice = Inventory.ShoppingCart.TotalPrice;
				var formattedPrice = totalPrice >= 0.00M ? totalPrice : 0.00M;

				StatusButton.Text
					= String.Format(
								Properties.StatusTemplate.ORDERS_STATUS_TEMPLATE,
								Inventory.ShoppingCart.TotalQuantity,
								CultureConfig.CurrencyInfo.Format(formattedPrice)
							);

				inventory.UpdateAndSave();
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}
	}
}