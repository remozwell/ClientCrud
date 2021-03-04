using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using ClientCrudMobile.Model;

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
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
            return client;
        }

        public async Task<RequestHandler<T>> GetAsync<T>(string url)
        {
            HttpClient client = getClient();
            var response = await client.GetAsync(url);
            RequestHandler<T> _return = new RequestHandler<T>();
            _return.isSuccessRequest = response.IsSuccessStatusCode;
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                _return.RequestResult = JsonConvert.DeserializeObject<T>(result);
            }
            return _return;

        }

        public async Task<RequestHandler<T>> PostAsync<T>(string url, string parameters)
        {
            HttpClient client = getClient();

            var content = new StringContent(parameters.ToString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            RequestHandler<T> _return = new RequestHandler<T>();
            _return.isSuccessRequest = response.IsSuccessStatusCode;
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                _return.RequestResult = JsonConvert.DeserializeObject<T>(result);
            }
            return _return;

        }
        public async Task<RequestHandler<T>> PutAsync<T>(string url, string parameters)
        {
            HttpClient client = getClient();

            var content = new StringContent(parameters.ToString(), Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, content);
            RequestHandler<T> _return = new RequestHandler<T>();
            _return.isSuccessRequest = response.IsSuccessStatusCode;
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<T>(result);
                _return.RequestResult =item;
            }
            return _return;

        }
        public async Task<RequestHandler<T>> DeleteAsync<T>(string url)
        {
            HttpClient client = getClient();

            var response = await client.DeleteAsync(url);
            client.Dispose();

            RequestHandler<T> _return = new RequestHandler<T>();
            _return.isSuccessRequest = response.IsSuccessStatusCode;
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                _return.RequestResult = JsonConvert.DeserializeObject<T>(result);
            }
            return _return;

        }
    }
}
