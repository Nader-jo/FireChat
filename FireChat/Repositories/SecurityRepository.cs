using FireChat.Interfaces;
using FireChat.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FireChat.Repositories
{
    public class SecurityRepository : ISecurityRepository
    {
        private static readonly string ApiKey = ConfigurationManager.AppSettings.Get("fireBaseApiKey");

        private static readonly string SignUpEndpoint =
            $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}";

        private static readonly string LoginEndpoint =
            $"https://identitytoolkit.googleapis.com/v1/accounts:RegisterWithPassword?key={ApiKey}";

        public async Task<bool> Register(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    email = user.Credential.Email,
                    userName = user.Username,
                    password = user.Credential.Password,
                    returnSecureToken = true
                };

                string bodyJson = JsonConvert.SerializeObject(body);
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(SignUpEndpoint, data);
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AuthResult>(resultJson);
                    App.UserId = result.localId;
                    App.IdToken = result.idToken;

                    return true;
                }

                string errorJson = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<Error>(errorJson);
                MessageBox.Show(error.error.message);

                return false;
            }
        }

        public async Task<bool> Login(Credential credential)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    email = credential.Email,
                    password = credential.Password,
                    returnSecureToken = true
                };

                string bodyJson = JsonConvert.SerializeObject(body);
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(LoginEndpoint, data);
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AuthResult>(resultJson);
                    App.UserId = result.localId;
                    App.IdToken = result.idToken;

                    return true;
                }

                string errorJson = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<Error>(errorJson);
                MessageBox.Show(error.error.message);

                return false;
            }
        }
    }
}