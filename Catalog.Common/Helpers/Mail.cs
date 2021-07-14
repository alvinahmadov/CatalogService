using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace Catalog.Common
{
	public static class Mail
	{
		public static void Send(Byte[] file, String filename, String message)
		{
			Task.Run(() => DoSend(file, filename, message, API.SAVE_ORDER));
		}

		private static async void DoSend(Byte[] file, String filename, String message, String path)
		{
			using (var content = new MultipartFormDataContent(API.Mail.BOUNDARY))
			{
				var messageContent = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(message)));
				var fileContent = new StreamContent(new MemoryStream(file), file.Length);

				content.Headers.ContentType.MediaType = API.Mail.MEDIA_TYPE;
				content.Add(messageContent, API.Mail.MESSAGE_KEY);
				content.Add(fileContent, API.Mail.FILE_KEY, filename);

				using (var httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept
													.Add(new MediaTypeWithQualityHeaderValue(API.Mail.MEDIA_TYPE));

					string responseMessage = "";

					try
					{
						var response = await httpClient.PostAsync(API.URL + path, content);
						responseMessage = await response.Content.ReadAsStringAsync();
					}
					catch (Exception e)
					{
						Debug.WriteLine(string.Format(MESSAGE.Mail.ERROR, $"{API.URL}{path}", e.Message, responseMessage));
					}
				}
			}
		}
	}
}