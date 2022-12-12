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
    internal class MessagesRepository : BaseRepository, IMessagesRepository
    {
        private static readonly string DbPath = ConfigurationManager.AppSettings.Get("fireBaseDb");

        public async Task<bool> Create(Message item)
        {
            return await base.Create(item);
        }

        public async Task<List<Message>> Read(string fromUserEmail, string toUserEmail)
        {
            return (await Read<Message>()).FindAll(
                m => m.FromUserEmail == fromUserEmail && m.ToUserEmail == toUserEmail);
        }

        public async Task<bool> Update(Message item)
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result =
                    await client.PutAsync(
                        $"{DbPath}{item.GetType().Name.ToLower()}-from-{item.FromUserEmail.ToLower()}-to-{item.ToUserEmail.ToLower()}/{item.Id}.json?auth={App.IdToken}",
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