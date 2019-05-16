using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace KoreaWeatherAPIService
{
    public interface IBaseRequest
    {
        Task<string> GetStringAsync(string url);

        Task<T> GetAsync<T>(string url, CancellationToken token = default(CancellationToken));
        Task<T> SendAsync<T>(HttpRequestMessage content, CancellationToken token = default(CancellationToken));
    }
}
