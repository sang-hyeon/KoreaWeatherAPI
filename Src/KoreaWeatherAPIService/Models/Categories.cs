
namespace KoreaWeatherAPIService.Models
{
    public enum Categories
    {
        POP,    //	강수확률	,	1%
        PTY,    //	강수형태	,	코드값
        R06,    //	6시간 강수량	,	범주 (1 mm)
        REH,    //	습도	,	1%
        S06,    //	6시간 신적설	,	범주(1 cm)
        SKY,    //	하늘상태	,	코드값
        T3H,    //	3시간 기온	,	0.1 ℃
        TMN,    //	일최저기온	,	0.1 ℃
        TMX,    //	일최고기온	,	0.1 ℃
        UUU,    //	풍속(동서성분)	,	0.1 m/s
        VVV,    //	풍속(남북성분)	,	0.1 m/s
        WAV,    //	파고	,	0.1 m
        VEC,    //	풍향	,	0
        WSD,    //	풍속	,	1
        T1H,    //	기온	,	0.1 ℃
        RN1,    //	1시간 강수량	,	범주 (1 mm)
        LGT,    //	낙뢰	,	코드값
    }
}
