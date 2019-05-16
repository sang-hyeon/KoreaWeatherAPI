using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

using KoreaWeatherAPIService.Models;
namespace KoreaWeatherAPIService
{
    public class WeatherService : BaseKoreaWeatherService
    {
        const string REQUEST_URL = "http://newsky2.kma.go.kr/service/SecndSrtpdFrcstInfoService2";
        const string FORECAST_GRIB = "/ForecastGrib";
        const string FORECAST_SPACE_DATA = "/ForecastSpaceData";

        readonly string API_KEY;
        readonly IBaseRequest _baseRequest;

        string ForecastGribUrl => $"{REQUEST_URL}{FORECAST_GRIB}?ServiceKey={API_KEY}";
        string ForecastSpaceDataUrl => $"{REQUEST_URL}{FORECAST_SPACE_DATA}?ServiceKey={API_KEY}";
        

        public WeatherService(string ApiKey)
            : this(ApiKey, new BaseRequest(new HttpClient()))
        {

        }

        public WeatherService(string ApiKey, HttpClient httpClient)
            : this(ApiKey, new BaseRequest(httpClient))
        {

        }

        public WeatherService(string ApiKey, IBaseRequest baseRequest)
        {
            if (string.IsNullOrEmpty(ApiKey))
                throw new ArgumentNullException(nameof(ApiKey));


            this.API_KEY = ApiKey;
            this._baseRequest = baseRequest;
        }

        public override Task<Observation> Request_NowWeatherAsync(Location location)
        {
            var xy = ToXY(location.Latitude, location.Longitude);
            return Request_NowWeatherAsync(xy);
        }

        public override async Task<Observation> Request_NowWeatherAsync((double,double) xy)
        {
            Guard_ValidateXY(xy);

            try
            {
                var date = DateTime.Now - TimeSpan.FromMinutes(30);
                var dates = new DateTime[] { date, date - TimeSpan.FromHours(1), date + TimeSpan.FromHours(1) };

                Observation response = null;
                foreach (var h in dates)
                {
                    var hour = h.Hour;
                    string hourStr;
                    if (hour < 10)
                        hourStr = "0" + hour + "00";
                    else hourStr = hour + "00";

                    var requestUrl = $"{ForecastGribUrl}&_type=json&nx={xy.Item1}&ny={xy.Item2}&base_date={date.ToString("yyyyMMdd")}&base_time={hourStr}";

                    response = await _baseRequest.GetAsync<Observation>(requestUrl);


                    if (response.Response.Header.IsSuccess)
                    {
                        if (response.Response.Body.Items != null) break;
                        else response = null;
                    }
                    else
                    {
                        throw new KoreaWeatherAPIException($"동네 웨더 - {response.Response.Header.ResultMsg}");
                    }
                }

                if (response == null)
                    throw new KoreaWeatherAPIException("동네 웨더 - 서버 응답 실패입니다.");

                return response;
            }
            catch (Exception e)
            {
                if (e is KoreaWeatherAPIException)
                    throw e;
                else throw new KoreaWeatherAPIException("", e);
            }
        }
    }
}