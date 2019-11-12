# KoreaWeatherAPIService
기상청에서 제공하는 '동네 예보' 날씨 API는 기능상 '위도,경도'로의 조회가 불가능합니다.

해당 라이브러리는 '위도, 경도'를 통해 날씨 예보를 요청하는 예제를 포함합니다.

C#이며 .Net Standard로 작성되었습니다.

'동네 예보'를 사용하시는분은 참고 하시기 바랍니다.
('지금 날씨' 요청만 가능합니다.)

## 샘플 스크린샷
<img src="/SampleScreen/scr_run.png" width="150" height="170">
<img src="/SampleScreen/scr_fail.png" width="150" height="170">


## 사용시
 - 샘플 실행 시 MainViewModel 에 API Key를 넣어주세요.
 
## 특징
 - 기상청에서 규정한대로 한국 지역만 요청이 가능합니다. 한국이 아닌 지역에 대하여 KoreaWeatherAPIException을 발생시킵니다.
