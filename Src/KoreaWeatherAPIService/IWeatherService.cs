using System;
using System.Threading.Tasks;

using KoreaWeatherAPIService.Models;
namespace KoreaWeatherAPIService
{
    public interface IWeatherService
    {
        Task<Observation> Request_NowWeatherAsync(Location location);
        Task<Observation> Request_NowWeatherAsync((double, double) xy);
    }
}
