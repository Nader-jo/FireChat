using FireChat.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FireChat.Repositories
{
    public class BaseRepository
    {
        private static readonly string DbPath = ConfigurationManager.AppSettings.Get("fireBaseDb");

        public async Task<bool> Create<T>(T item) where T : IHasId
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var result = await client.PostAsync($"{DbPath}{item.GetType().Name.ToLower()}.json?auth={App.IdToken}",
                content);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<List<T>> Read<T>() where T : IHasId
        {
            List<T> items = new List<T>();

            if (App.UserId != string.Empty)
            {
                using (var client = new HttpClient())
                {
                    var path = $"{DbPath}{typeof(T).Name.ToLower()}.json?auth={App.IdToken}";
                    var result = await client.GetAsync(path);
                    var jsonResult = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode)
                    {
                        var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonResult);
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

        public async Task<bool> Update<T>(T item) where T : IHasId
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result =
                    await client.PutAsync($"{DbPath}{item.GetType().Name.ToLower()}/{item.Id}.json?auth={App.IdToken}",
                        content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> Delete<T>(T item) where T : IHasId
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result =
                    await client.DeleteAsync(
                        $"{DbPath}{item.GetType().Name.ToLower()}/{item.Id}.json?auth={App.IdToken}");

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
    }
}