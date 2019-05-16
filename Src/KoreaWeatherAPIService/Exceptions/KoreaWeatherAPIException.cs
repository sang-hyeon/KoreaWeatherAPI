using System;

namespace KoreaWeatherAPIService
{
    public class KoreaWeatherAPIException : Exception
    {
        public KoreaWeatherAPIException(string msg) : base(msg) { }
        public KoreaWeatherAPIException(string msg, Exception e) : base(msg, e) { }
    }
}
