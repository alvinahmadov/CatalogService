using System;
using System.Net;
using System.Diagnostics;

namespace Catalog.Common
{
	public class DownloadManager : IDisposable
	{
		public WebClient WebClient { get; private set; }

		public Uri BaseAddress { get; private set; }

		public DownloadManager(String baseAddress)
		{
			this.BaseAddress = new Uri(baseAddress);
			this.WebClient = new WebClient()
			{
				BaseAddress = baseAddress,
				Encoding = System.Text.Encoding.UTF8,
			};

			this.WebClient.Headers.Add(HttpRequestHeader.Accept, "image/jpg");
			this.WebClient.Headers.Add(HttpRequestHeader.KeepAlive, "10");
		}

		public void DownloadAsBytes(String filename, Action<Byte[]> callback, out String errorMessage)
		{
			try
			{
				errorMessage = "";
				var data = this.WebClient.DownloadData($"{this.BaseAddress}{filename}");
				callback(data);
			}
			catch (WebException we)
			{
				errorMessage = String.Format(MESSAGE.Mail.ERROR, filename, we.Message, we.Response);
				Debug.WriteLine(errorMessage);
			}
			catch (Exception ex)
			{
				errorMessage = String.Format(MESSAGE.Mail.ERROR, filename, ex.Message, ex.StackTrace);
				Debug.WriteLine(errorMessage);
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