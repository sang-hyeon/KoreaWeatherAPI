using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace KoreaWeatherAPIService
{
    internal class BaseRequest : IBaseRequest
    {
        readonly HttpClient _httpClient;
        readonly JsonSerializer _serializer = new JsonSerializer();

        public HttpClient Client => _httpClient;

        public BaseRequest(HttpClient client)
        {
            this._httpClient = client ?? new HttpClient();
        }

        public async Task<string> GetStringAsync(string api)
        {
            return await _httpClient.GetStringAsync(api);
        }

        public Task<T> GetAsync<T>(string url, CancellationToken token = default(CancellationToken))
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return SendAsync<T>(request, token);
        }

        public async Task<T> SendAsync<T>(HttpRequestMessage requestMsg, CancellationToken token = default(CancellationToken))
        {
            var response = await _httpClient.SendAsync(requestMsg, token);
            return await DeserializeContentAsync<T>(response.Content);
        }

        protected async Task<T> DeserializeContentAsync<T>(HttpContent content)
        {
            try
            {
                T Result = default(T);
                Stream stream = await content.ReadAsStreamAsync();

                using (stream)
                {
                    using (var reader = new StreamReader(stream))
                    using (var json = new JsonTextReader(reader))
                    {
                        Result = _serializer.Deserialize<T>(json);
                    }
                }

                return Result;
            }
            catch (Exception e)
            {
                e.Data.Add("DeserializeContentAsync", await content.ReadAsStringAsync());
                throw e;
            }
        }
    }
}
