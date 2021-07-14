using System;
using System.Globalization;

namespace Catalog.Common
{
	public abstract class Culture
	{
		public CultureInfo CurrentCulture { get; private set; }
		public static string FormatString { get; set; }
		public static string FormatKey { get; set; }

		public Culture(CultureType type = CultureType.RU)
		{
			FormatString = "";

			switch (type)
			{
				case CultureType.US:
					CurrentCulture = CultureInfo.GetCultureInfo(CULTURE.US);
					break;
				case CultureType.GB:
					CurrentCulture = CultureInfo.GetCultureInfo(CULTURE.GB);
					break;
				case CultureType.RU:
					CurrentCulture = CultureInfo.GetCultureInfo(CULTURE.RU);
					break;
			}
		}
	}

	public class CurrencyInfo : Culture
	{
		public static new string FormatString { get => "{0:C}"; }
		public static new string FormatKey { get => "C"; }

		public CurrencyInfo(CultureType type) : base(type)
		{ }

		public string Format(Decimal value, bool useBold = true)
		{
			string formatString = useBold ? $"<html><b>{FormatString}</b>" : FormatString;
			return String.Format(formatString, value.ToString(FormatKey, CurrentCulture));
		}
	}

	public class DateTimeInfo : Culture
	{
		public static new string FormatString { get => "{0:f}"; }

		public static new string FormatKey { get => "f"; }

		public DateTimeInfo(CultureType type) : base(type)
		{ }

		public string Format(DateTime value)
		{
			return String.Format(FormatString, value.ToString(CurrentCulture));
		}
	}

	public static class CultureConfig
	{
		public static CultureType CurrentCultureType
		{ get => CultureType.RU; }

		public static DateTimeInfo DateTimeInfo
		{
			get
			{
				if (dateTimeInfo == null)
					dateTimeInfo = new DateTimeInfo(CurrentCultureType);
				return dateTimeInfo;
			}
		}

		public static CurrencyInfo CurrencyInfo
		{
			get
			{
				if (currencyInfo == null)
					currencyInfo = new CurrencyInfo(CurrentCultureType);
				return currencyInfo;
			}
		}

		private static DateTimeInfo dateTimeInfo = null;
		private static CurrencyInfo currencyInfo = null;
	}
}