using System;
using System.Drawing;
using System.Threading.Tasks;
using KoreaWeatherAPIService.Models;
namespace KoreaWeatherAPIService
{
    /// <summary>
    /// 유효한 한국 대지 점검 기능을 갖춘 추상 개체입니다.
    /// </summary>
    public abstract class BaseKoreaWeatherService : IWeatherService
    {
        //유효한 한국 대지 면적 범위 입니다.
        public static readonly RectangleF ValidateRect_Korea = new RectangleF(27, 7, 118, 141);

        #region Abstract
        public abstract Task<Observation> Request_NowWeatherAsync(Location location);
        public abstract Task<Observation> Request_NowWeatherAsync((double, double) xy);
        #endregion

        protected (double,double) Guard_ValidateXY(Location geo)
        {
            var xy = ToXY(geo.Latitude, geo.Longitude);
            return Guard_ValidateXY(xy);
        }
        protected (double, double) Guard_ValidateXY((double,double) xy)
        {
            if (IsValidateXY(xy) == false)
                throw new KoreaWeatherAPIException("Guard_ValidateXY", new IndexOutOfRangeException("한국의 대지 면적을 벗어났습니다."));

            return xy;
        }

        protected bool IsValidateXY((double,double) xy)
        {
            return IsValidateXY(xy.Item1, xy.Item2);
        }
        protected bool IsValidateXY(double x, double y)
        {
            return ValidateRect_Korea.Contains(new PointF((float)x, (float)y));
        }

        protected (double,double) ToXY(double lat, double log)
        {
            float RE = 6371.00877f; // 지구 반경(km)
            float GRID = 5.0f; // 격자 간격(km)
            float SLAT1 = 30.0f; // 투영 위도1(degree)
            float SLAT2 = 60.0f; // 투영 위도2(degree)
            float OLON = 126.0f; // 기준점 경도(degree)
            float OLAT = 38.0f; // 기준점 위도(degree)
            float XO = 43; // 기준점 X좌표(GRID)
            float YO = 136; // 기1준점 Y좌표(GRID)

            (double,double) result = (0, 0);
            var DEGRAD = Math.PI / 180.0;
            var RADDEG = 180.0 / Math.PI;

            var re = RE / GRID;
            var slat1 = SLAT1 * DEGRAD;
            var slat2 = SLAT2 * DEGRAD;
            var olon = OLON * DEGRAD;
            var olat = OLAT * DEGRAD;

            var sn = Math.Tan(Math.PI * 0.25 + slat2 * 0.5) / Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
            sn = Math.Log(Math.Cos(slat1) / Math.Cos(slat2)) / Math.Log(sn);
            var sf = Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
            sf = Math.Pow(sf, sn) * Math.Cos(slat1) / sn;
            var ro = Math.Tan(Math.PI * 0.25 + olat * 0.5);
            ro = re * sf / Math.Pow(ro, sn);

            var ra = Math.Tan(Math.PI * 0.25 + (lat) * DEGRAD * 0.5);
            ra = re * sf / Math.Pow(ra, sn);
            var theta = log * DEGRAD - olon;
            if (theta > Math.PI) theta -= 2.0 * Math.PI;
            if (theta < -Math.PI) theta += 2.0 * Math.PI;
            theta *= sn;

            result = (
                Math.Floor(ra * Math.Sin(theta) + XO + 0.5),
                Math.Floor(ro - ra * Math.Cos(theta) + YO + 0.5)
                );

            return result;
        }
    }
}
