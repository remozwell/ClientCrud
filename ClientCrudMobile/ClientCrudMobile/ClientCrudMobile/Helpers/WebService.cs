using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClientCrudMobile.Helpers
{
    public class WebService
    {
        public string baseUrl { get; set; }
        const string BearerToken = "bearerToken";
        public WebService()
        {
            baseUrl = "https://my-json-server.typicode.com/remozwell/Clientesdb/";
        }

        private HttpClient getClient()
        {

            HttpClient client = new HttpClient(); 
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
            return client;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            HttpClient client = getClient();
            var response = await client.GetAsync(url);
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
            return default(T);

        }

        public async Task<T> PostAsync<T>(string url, string parameters)
        {
            HttpClient client = getClient();

            var buffer = System.Text.Encoding.UTF8.GetBytes(parameters);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PostAsync(url, byteContent);
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
            return default(T);

        }
        public async Task<T> PuttAsync<T>(string url, string parameters)
        {
            HttpClient client = getClient();

            var buffer = System.Text.Encoding.UTF8.GetBytes(parameters);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(url, byteContent);
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
            return default(T);

        }
    }
}
