using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Client
{
    static class FieldsHelper
    {
        public static List<string> AddressFields = new List<string>() { "AddressLine1", "AddressLine2", "City", "StateProvince", "PostalCode", "ModifiedDate" };
        public static List<string> TransactionHistoryFields = new List<string>() { "TransactionDate", "TransactionID", "Quantity", "TotalCost" };
        public static List<string> VendorFields = new List<string> { "AccountNumber", "Name", "CreditRating", "PreferredVendorStatus", "ActiveFlag", "PurchasingWebServiceURL", "ModifiedDate" };
		public static List<string> ShoppingCartFields = new List<string>() { "Status", "TotalQuantity", "TotalPrice", "ModifiedDate", "DateCreated", "Delete" };
		public static List<string> ShoppingCartItemsFields = new List<string>() { "Name", "Quantity", "UnitPrice", "ModifiedDate", "DateCreated", "Delete" };
		public static List<string> InventoriesFields = new List<string> { "Photo", "ArticleNumber", "Name", "StockLevel1", "StockLevel2", "Price", "Quantity"};
        public static List<string> WorkOrdersFields = new List<string> { "Product", "OrderQty", "StockedQty", "ScrappedQty", "StartDate", "EndDate", "DueDate", "ModifiedDate" };
        public static List<string> BillOfMaterialsFields = new List<string> { "Product", "Child Product", "StartDate", "EndDate", "BOMLevel", "UnitMeasure", "PerAssemblyQty", "ModifiedDate" };
        public static List<string> OrdersFields = new List<string> { "SalesOrderNumber", "Customer", "DueDate", "OnlineOrderFlag", "AccountNumber", "SubTotal", "TaxAmt", "Freight", "TotalDue", "ShipMethod" };
        public static List<string> StoresFields = new List<string> { "AccountNumber", "CompanyName", "StoreContact", "EmailAddress", "Phone", "AddressLine1", "City", "State", "ModifiedDate" };
        public static List<string> IndividualsFields = new List<string> { "AccountNumber", "FirstName", "LastName", "EmailAddress", "Phone", "AddressLine1", "City", "State", "ModifiedDate" };
    }
}
