using System;
using System.Threading.Tasks;

using Xunit;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;

using KoreaWeatherAPIService.Models;
namespace KoreaWeatherAPIService.Tests.ServiceTests
{
    public class BaseKoreaWeatherServiceTests
    {
        /// <summary>
        /// 인스턴스화에 장애가 없습니다.
        /// </summary>
        [Theory,AutoData]
        public void create_with_no_exceptions(Observation obsWeather)
        {
            var create = new Action(() => new StubService(obsWeather));
            create.Should().NotThrow<Exception>();
        }

        /// <summary>
        /// 제대로된 한국 좌표에는 장애를 발생시키지 않습니다.
        /// </summary>
        [Theory]
        [InlineAutoData(37.493709, 127.053175)]
        public void return_weather_if_range_of_korea(double lat, double lng, Observation observationWeather)
        {
            //Arrange
            Location GangNam_gu = new Location(lat, lng);
            var sut = new StubService(observationWeather);

            //Action
            var Reqeust = new Func<Observation>(()=> sut.Request_NowWeatherAsync(GangNam_gu).Result );

            //Assert
            Reqeust.Should().NotThrow<Exception>();
        }

        /// <summary>
        /// 한국 좌표가 아닌 지역에 대해 OutOfRange Exception을 발생시킵니다.
        /// </summary>
        [Theory]
        [InlineAutoData(51.969868, 6.819718)]
        public void Guard_throw_exception_if_out_of_range_korea(double lat, double lng, Observation observationWeather)
        {
            //Arrange
            Location location = new Location(lat, lng);
            var sut = new StubService(observationWeather);

            //Action
            var Reqeust = new Func<Observation>(() => sut.Request_NowWeatherAsync(location).Result);

            //Assert
            Reqeust.Should().Throw<KoreaWeatherAPIException>();
        }

        public class StubService : BaseKoreaWeatherService
        {
            public Observation TestData { get; set; }

            public StubService(Observation testData)
            {
                this.TestData = testData;
            }

            public override Task<Observation> Request_NowWeatherAsync(Location location)
            {
                Guard_ValidateXY(location);
                return Task.FromResult(TestData);
            }

            public override Task<Observation> Request_NowWeatherAsync((double, double) xy)
            {
                Guard_ValidateXY(xy);
                return Task.FromResult(TestData);
            }
        }
    }
}
