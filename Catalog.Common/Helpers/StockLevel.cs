using System;
using System.Collections.Generic;

namespace Catalog.Common
{
	public class StockLevel
	{
		public const string TOO_MANY = "***";

		public const string MANY = "**";

		public const string FEW = "*";

		public const string NONE = "-";

		public String First
		{
			get => first;
			private set => first = value.Length > 0 ? value : NONE;
		}

		public String Second 
		{ 
			get => second; 
			private set => second = value.Length > 0 ? value : NONE; 
		}

		public StockLevel(String stock) : this(stock, stock)
		{ }

		public StockLevel(String stock1, String stock2)
		{
			First = stock1.Trim();
			Second = stock2.Trim();
		}

		public static int Parse(string stockLevel)
		{
			Int32 result;

			if (stockLevel == TOO_MANY)
				result = 1000;
			else if (stockLevel == MANY)
				result = 500;
			else if (stockLevel == FEW)
				result = 250;
			else if (stockLevel == NONE)
				result = 0;
			else
				Int32.TryParse(stockLevel, out result);

			return result;
		}

		private string first;
		private string second;
	}
}
