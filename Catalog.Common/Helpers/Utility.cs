using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

using JsonToken = Newtonsoft.Json.Linq.JToken;


namespace Catalog.Common.Utils
{
	public delegate void EventAction<TEvent>(Object sender, TEvent eventArgs);

	public struct OrderSave
	{
		public int userid;
		public int ordernumber;
		public int product;
		public int quantity;

		public OrderSave(int userId, int orderNumber, int productId, int productQuantity) 
		{
			userid = userId;
			ordernumber = orderNumber;
			product = productId;
			quantity = productQuantity;
		}
	}

	#region Globalization

	public static class DateTimeMoscow
	{
		public static DateTime UtcNow
		{
			get => DateTime.UtcNow.AddHours(3);
		}
	}

	public enum CultureType
	{
		RU = 0,
		US = 1,
		GB = 2
	}

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
					CurrentCulture = CultureInfo.GetCultureInfo("en-US");
					break;
				case CultureType.GB:
					CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
					break;
				case CultureType.RU:
					CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
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
	#endregion

	public static class Configuration
	{
		public static string Get(string key)
		{
			string value = string.Empty;
			try
			{
				var configurationManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				var appSettings = configurationManager.AppSettings.Settings;
				value = appSettings[key].Value;
			}
			catch (Exception)
			{

			}

			return value;
		}
	}

	public struct Base64
	{
		public enum OpMode
		{
			ENCODE,
			DECODE
		}

		/// <summary>
		/// Decodes givent string to base64
		/// </summary>
		/// <param name="plainText">Text to be encoded</param>
		/// <returns>Encoded text</returns>
		public static string Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		/// <summary>
		/// Decodes given token
		/// </summary>
		/// <param name="base64EncodedData">Data to be decoded</param>
		/// <returns>Decoded string</returns>
		public static string Decode(string base64EncodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		public static bool Equal(string text1, string text2) => Equal(text1, text2, OpMode.ENCODE);

		public static bool Equal(string text1, string text2, OpMode op)
		{
			if (op.Equals(OpMode.ENCODE))
				return Encode(text1) == Encode(text2);
			else
				return Decode(text1) == Decode(text2);
		}
	}

	public struct Converter
	{
		public static Decimal ToDecimal(string token)
		{
			token = token.Trim();
			if (token.Length == 0)
				return 0.00M;
			if (token.EndsWith(",") || token.EndsWith("."))
				token = token.Substring(0, token.Length - 1);

			if (Regex.Match(token, @"\s", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Success)
				token = Regex.Replace(token, @"\s", "");


			Decimal.TryParse(
				token,
				NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol,
				CultureInfo.InvariantCulture,
				out Decimal value
			);

			return value;
		}

		public static Double ToDouble(JsonToken token)
		{
			return ToFloatingPoint<Double>(token);
		}

		private static dynamic ToFloatingPoint<T>(JsonToken token)
		{
			var str = token.ToObject<string>();

			str = str.Replace('.', ',');
			str = Regex.Replace(str, @"\s", string.Empty);

			if (str.Length == 0)
				str = "0,0";
			else if (!str.Contains(","))
				str = $"{str},0";


			if (typeof(T) == typeof(Decimal))
			{
				return Convert.ToDecimal(str);
			}
			else if (typeof(T) == typeof(Double))
			{
				return Convert.ToDouble(str);
			}

			return default;
		}
	}

	public class DownloadManager : IDisposable
	{
		public WebClient WebClient { get; private set; }
		public Uri BaseAddress { get; private set; }

		public DownloadManager(string baseAddress)
		{
			this.BaseAddress = new Uri(baseAddress);
			this.WebClient = new WebClient()
			{
				BaseAddress = baseAddress,
				//Encoding = System.Text.Encoding.UTF8,
			};

			this.WebClient.Headers.Add(HttpRequestHeader.Accept, "image/jpg");
			this.WebClient.Headers.Add(HttpRequestHeader.KeepAlive, "10");
		}

		public void DownloadAsBytes(string filename, Action<Byte[]> callback)
		{
			try
			{
				var data = this.WebClient.DownloadData($"{this.BaseAddress}{filename}");
				callback(data);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"DownloadAsBytes: Thrown exception when executing task for {this.BaseAddress}{filename}");
				System.Diagnostics.Debug.WriteLine($"Details: {ex.Message} {ex.InnerException?.Message}");
			}
		}

		public async void DownloadAsBytesAsync(string filename, Action<Byte[]> callback, bool save = false)
		{
			try
			{
				var data = await this.WebClient.DownloadDataTaskAsync($"{this.BaseAddress}{filename}");
				callback(data);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"DownloadAsBytesAsync: Thrown exception when executing task for {this.BaseAddress}{filename}");
				Debug.WriteLine($"Details: {ex.Message} {ex.InnerException?.Message}");
			}
		}

		public void Dispose()
		{
			WebClient.Dispose();
		}
	}
}
