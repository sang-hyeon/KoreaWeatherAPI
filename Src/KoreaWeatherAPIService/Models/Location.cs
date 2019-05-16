
namespace KoreaWeatherAPIService.Models
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location(double lat, double lng)
        {
            this.Latitude = lat;
            this.Longitude = lng;
        }
    }
}
