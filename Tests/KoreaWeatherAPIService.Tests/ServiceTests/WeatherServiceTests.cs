using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

using Xunit;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;

using KoreaWeatherAPIService.Models;
using KoreaWeatherAPIService;
using System.Threading;

namespace KoreaWeatherAPIService.Tests.ServiceTests
{
    public class WeatherServiceTests
    {
        const string JSON_RESULT = @"{'response':{'header':{'resultCode':'0000','resultMsg':'OK'},'body':{'items':{'item':[{'baseDate':20190516,'baseTime':2300,'category':'PTY','nx':61,'ny':125,'obsrValue':0},{'baseDate':20190516,'baseTime':2300,'category':'REH','nx':61,'ny':125,'obsrValue':45},{'baseDate':20190516,'baseTime':2300,'category':'RN1','nx':61,'ny':125,'obsrValue':0},{'baseDate':20190516,'baseTime':2300,'category':'T1H','nx':61,'ny':125,'obsrValue':23},{'baseDate':20190516,'baseTime':2300,'category':'UUU','nx':61,'ny':125,'obsrValue':0.5},{'baseDate':20190516,'baseTime':2300,'category':'VEC','nx':61,'ny':125,'obsrValue':231},{'baseDate':20190516,'baseTime':2300,'category':'VVV','nx':61,'ny':125,'obsrValue':0.4},{'baseDate':20190516,'baseTime':2300,'category':'WSD','nx':61,'ny':125,'obsrValue':0.6}]},'numOfRows':10,'pageNo':1,'totalCount':8}}}";
        const string JSON_FAIL_RESULT = @"{'response':{'header':{'resultCode':'1111','resultMsg':'FAIL'},'body':{'items':{'item':[{'baseDate':20190516,'baseTime':2300,'category':'PTY','nx':61,'ny':125,'obsrValue':0},{'baseDate':20190516,'baseTime':2300,'category':'REH','nx':61,'ny':125,'obsrValue':45},{'baseDate':20190516,'baseTime':2300,'category':'RN1','nx':61,'ny':125,'obsrValue':0},{'baseDate':20190516,'baseTime':2300,'category':'T1H','nx':61,'ny':125,'obsrValue':23},{'baseDate':20190516,'baseTime':2300,'category':'UUU','nx':61,'ny':125,'obsrValue':0.5},{'baseDate':20190516,'baseTime':2300,'category':'VEC','nx':61,'ny':125,'obsrValue':231},{'baseDate':20190516,'baseTime':2300,'category':'VVV','nx':61,'ny':125,'obsrValue':0.4},{'baseDate':20190516,'baseTime':2300,'category':'WSD','nx':61,'ny':125,'obsrValue':0.6}]},'numOfRows':10,'pageNo':1,'totalCount':8}}}";
        /// <summary>
        /// 인스턴스화에 장애가 없습니다.
        /// </summary>
        [Theory, AutoData]
        public void create_with_no_exceptions(string apiKey)
        {
            var baseRequest = new Mock<IBaseRequest>();
            var create = new Action(() => new WeatherService(apiKey, baseRequest.Object));
            create.Should().NotThrow<Exception>();
        }

        /// <summary>
        /// 제대로된 한국 좌표에는 정상적인 데이터를 반환합니다.
        /// </summary>
        [Theory]
        [InlineAutoData(37.493709, 127.053175)]
        public void reqeust_observation_weather(double lat, double lng, string apiKey)
        {
            //Arrange
            Location GangNam_gu = new Location(lat, lng);
            var testResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(JSON_RESULT) };
            var httpClient = new HttpClient(new MockMsgHandler(testResponse));
            var sut = new WeatherService(apiKey, httpClient);

            //Action
            var weather = sut.Request_NowWeatherAsync(GangNam_gu).Result;

            //Assert
            weather.Should().NotBeNull();
            weather.Response.Header.IsSuccess.Should().BeTrue();
            weather.Response.Body.Items.Item.Select(q => q.Category).Any(q => q == Categories.T1H).Should().BeTrue();
        }

        /// <summary>
        /// 기상청 API에 요청이 실패되면 장애를 발생시킵니다.
        /// </summary>
        [Theory]
        [InlineAutoData(37.493709, 127.053175)]
        public void throw_exception_when_reqeust_fail(double lat, double lng, string apiKey)
        {
            //Arrange
            Location GangNam_gu = new Location(lat, lng);
            var testResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(JSON_FAIL_RESULT) };
            var httpClient = new HttpClient(new MockMsgHandler(testResponse));
            var sut = new WeatherService(apiKey, httpClient);

            //Action
            var reqeust = new Func<Observation>(()=> sut.Request_NowWeatherAsync(GangNam_gu).Result);

            //Assert
            reqeust.Should().Throw<KoreaWeatherAPIException>();
        }
    }

    public class MockMsgHandler : HttpMessageHandler
    {
        public HttpResponseMessage TestMsg { get; set; }

        public MockMsgHandler(HttpResponseMessage testMsg)
        {
            this.TestMsg = testMsg;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(TestMsg);
        }
    }
}
