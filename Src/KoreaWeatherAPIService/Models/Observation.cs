using System;

namespace KoreaWeatherAPIService.Models
{
    public class Observation
    {
        public Weather Response { get; set; }
        public DateTime BaseTime { get; set; }
    }

    public class Weather
    {
        public Header Header { get; set; }
        public Body Body { get; set; }
    }

    public class Header
    {
        public string ResultCode { get; set; }
        public string ResultMsg { get; set; }

        public bool IsSuccess
            => ResultCode == "0000";
    }

    public class Body
    {
        public Items Items { get; set; }
        public int NumOfRows { get; set; }
        public int PageNo { get; set; }
        public int TotalCount { get; set; }
    }

    public class Items
    {
        public Item[] Item { get; set; }
    }

    public class Item
    {
        public string BaseDate { get; set; }
        public string BaseTime { get; set; }
        public Categories Category { get; set; }
        public int Nx { get; set; }
        public int Ny { get; set; }
        public float ObsrValue { get; set; }

        public DateTime FromBaseTime()
        {
            var hour = int.Parse(BaseTime) * 0.01;
            var result = DateTime.Now - TimeSpan.FromHours(DateTime.Now.Hour);
            result = result - (TimeSpan.FromMinutes(result.Minute) + TimeSpan.FromSeconds(result.Second));
            result = result.AddHours(hour);

            return result;
        }
    }
}
