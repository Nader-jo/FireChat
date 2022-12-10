using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FireChat.Interfaces;
using FireChat.Models;
using Newtonsoft.Json;

namespace FireChat.Repositories
{
	internal class MessagesRepository : IMessagesRepository
	{
		private static readonly string DbPath = ConfigurationManager.AppSettings.Get("fireBaseDb");

		public async Task<bool> Create(Message item)
		{
			var body = JsonConvert.SerializeObject(item);
			var content = new StringContent(body, Encoding.UTF8, "application/json");

			using var client = new HttpClient();
			var result = await client.PostAsync($"{DbPath}{item.GetType().Name.ToLower()}-from-{item.FromUserEmail.ToLower()}-to-{item.ToUserEmail.ToLower()}.json?auth={App.IdToken}",
				content);

			if (result.IsSuccessStatusCode)
			{
				return true;
			}

			return false;
		}

		public async Task<List<Message>> Read(string fromUserEmail, string toUserEmail)
		{
			List<Message> items = new List<Message>();

			if (App.UserId != string.Empty)
			{
				using (var client = new HttpClient())
				{
					var path = $"{DbPath}{typeof(Message).Name.ToLower()}-from-{fromUserEmail.ToLower()}-to-{toUserEmail.ToLower()}.json?auth={App.IdToken}";
					var result = await client.GetAsync(path);
					var jsonResult = await result.Content.ReadAsStringAsync();

					if (result.IsSuccessStatusCode)
					{
						var objects = JsonConvert.DeserializeObject<Dictionary<string, Message>>(jsonResult);
						if (objects == null) return items;
						foreach (var o in objects)
						{
							o.Value.Id = o.Key;
							items.Add(o.Value);
						}

						return items;
					}

					return items;
				}
			}

			return items;
		}

		public async Task<bool> Update(Message item)
		{
			var body = JsonConvert.SerializeObject(item);
			var content = new StringContent(body, Encoding.UTF8, "application/json");

			using (var client = new HttpClient())
			{
				var result =
					await client.PutAsync($"{DbPath}{item.GetType().Name.ToLower()}-from-{item.FromUserEmail.ToLower()}-to-{item.ToUserEmail.ToLower()}/{item.Id}.json?auth={App.IdToken}",
						content);

				if (result.IsSuccessStatusCode)
				{
					return true;
				}

				return false;
			}
		}

		public async Task<bool> Delete(Message item)
		{
			var body = JsonConvert.SerializeObject(item);
			var content = new StringContent(body, Encoding.UTF8, "application/json");

			using (var client = new HttpClient())
			{
				var result =
					await client.DeleteAsync(
						$"{DbPath}{item.GetType().Name.ToLower()}-from-{item.FromUserEmail.ToLower()}-to-{item.ToUserEmail.ToLower()}/{item.Id}.json?auth={App.IdToken}");

				if (result.IsSuccessStatusCode)
				{
					return true;
				}

				return false;
			}
		}

		public async Task<bool> Send(Message message)
		{
			try
			{
				await Create(message);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}