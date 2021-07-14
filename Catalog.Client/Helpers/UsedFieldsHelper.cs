using System;
using System.Drawing;

using StringList = System.Collections.Generic.List<System.String>;

namespace Catalog.Client
{
	static class FieldsHelper
	{
		public static readonly StringList AddressFields = new StringList() { "AddressLine1", "AddressLine2", "City", "StateProvince", "PostalCode", "ModifiedDate" };

		public static readonly StringList TransactionHistoryFields = new StringList() { "TransactionDate", "TransactionID", "Quantity", "TotalCost" };

		public static readonly StringList VendorFields = new StringList { "AccountNumber", "Name", "CreditRating", "PreferredVendorStatus", "ActiveFlag", "PurchasingWebServiceURL", "ModifiedDate" };

		public static readonly StringList ShoppingCartFields = new StringList() { "Status", "TotalQuantity", "TotalPrice", "ModifiedDate", "DateCreated", "Delete" };

		public static readonly StringList ShoppingCartItemsFields = new StringList() { "Name", "Quantity", "UnitPrice", "ModifiedDate", "DateCreated", "Delete" };

		public static readonly StringList InventoriesFields = new StringList { "Photo", "ArticleNumber", "Name", "StockLevel1", "StockLevel2", "Price", "Quantity" };

		public static readonly StringList WorkOrdersFields = new StringList { "Product", "OrderQty", "StockedQty", "ScrappedQty", "StartDate", "EndDate", "DueDate", "ModifiedDate" };

		public static readonly StringList BillOfMaterialsFields = new StringList { "Product", "Child Product", "StartDate", "EndDate", "BOMLevel", "UnitMeasure", "PerAssemblyQty", "ModifiedDate" };

		public static readonly StringList OrdersFields = new StringList { "SalesOrderNumber", "Customer", "DueDate", "OnlineOrderFlag", "AccountNumber", "SubTotal", "TaxAmt", "Freight", "TotalDue", "ShipMethod" };

		public static readonly StringList StoresFields = new StringList { "AccountNumber", "CompanyName", "StoreContact", "EmailAddress", "Phone", "AddressLine1", "City", "State", "ModifiedDate" };

		public static readonly StringList IndividualsFields = new StringList { "AccountNumber", "FirstName", "LastName", "EmailAddress", "Phone", "AddressLine1", "City", "State", "ModifiedDate" };
	}

	static class ColumnNamesHelper
	{
		public static readonly StringList ProductInventory =
			new StringList {
				"Фото",
				"Артикул",
				"Наименование товара",
				"НН",
				"МСК",
				"Цена",
				"Количество"
			};

		public static readonly StringList ShoppingCartItem =
			new StringList {
				"Фото",
				"Артикул",
				"Код",
				"Бренд",
				"Наименование товара",
				"Цена за единицу",
				"Дата изменения",
				"Дата добавления",
				"Количество",
				"Удалить"
			};

		public static readonly StringList ShoppingCart =
			new StringList {
				"Статус",
				"Общее количество",
				"Общая сумма",
				"Дата изменения",
				"Удалить все"
			};

		public static readonly StringList Transaction =
			new StringList {
				"Дата и время заказа",
				"Номер заказа",
				"Товары",
				"Cумма"
			};
	}

	static class ColumnTypesHelper
	{
		public static readonly Type[] ProductInventory =
			new Type[] {
				typeof(Image),
				typeof(String),
				typeof(String),
				typeof(String),
				typeof(String),
				typeof(Decimal),
				typeof(Int32)
			};

		public static readonly Type[] ShoppingCart =
			new Type[] {
				typeof(Byte),
				typeof(Int32),
				typeof(Decimal),
				typeof(DateTime),
				typeof(DateTime)
			};

		public static readonly Type[] Transaction =
			new Type[] {
				typeof(DateTime),
				typeof(Int32),
				typeof(Int32),
				typeof(Decimal)
			};
	}
}
