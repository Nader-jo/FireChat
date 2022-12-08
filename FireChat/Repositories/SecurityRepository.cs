using FireChat.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FireChat.Repositories
{
	internal class SecurityRepository
	{
		private static string API_KEY = ConfigurationManager.AppSettings.Get("fireBaseApiKey");
		private static string SIGNUP_ENDPOINT = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={API_KEY}";
		private static string LOGIN_ENDPOINT = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={API_KEY}";

		public static async Task<bool> Register(User user)
		{
			using (HttpClient client = new HttpClient())
			{
				var body = new
				{
					email = user.Email,
					password = user.Password,
					returnSecureToken = true
				};

				string bodyJson = JsonConvert.SerializeObject(body);
				var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

				var response = await client.PostAsync(SIGNUP_ENDPOINT, data);
				if (response.IsSuccessStatusCode)
				{
					string resultJson = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<AuthResult>(resultJson);
					App.UserId = result.localId;
					App.IdToken = result.idToken;

					return true;
				}
				else
				{
					string errorJson = await response.Content.ReadAsStringAsync();
					var error = JsonConvert.DeserializeObject<Error>(errorJson);
					MessageBox.Show(error.error.message);

					return false;
				}

			}
		}
		public static async Task<bool> Login(User user)
		{
			using (HttpClient client = new HttpClient())
			{
				var body = new
				{
					email = user.Email,
					password = user.Password,
					returnSecureToken = true
				};

				string bodyJson = JsonConvert.SerializeObject(body);
				var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

				var response = await client.PostAsync(LOGIN_ENDPOINT, data);
				if (response.IsSuccessStatusCode)
				{
					string resultJson = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<AuthResult>(resultJson);
					App.UserId = result.localId;
					App.IdToken = result.idToken;

					return true;
				}
				else
				{
					string errorJson = await response.Content.ReadAsStringAsync();
					var error = JsonConvert.DeserializeObject<Error>(errorJson);
					MessageBox.Show(error.error.message);

					return false;
				}

			}
		}
	}
}
