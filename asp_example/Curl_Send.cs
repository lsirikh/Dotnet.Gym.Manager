using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace WpfApp1.AligoSMSSample
{
    class Curl_Send
    {
        private Curl_Send()
        { }
        private void Curl_SendEx()
        {
            /**************** 문자전송하기 예제 ******************/
            /* "result_code":결과코드,"message":결과문구, */
            /* "msg_id":메세지ID,"error_cnt":에러갯수,"success_cnt":성공갯수 */
            /* 동일내용 > 전송용 입니다.  
            /******************** 인증정보 ********************/
            //문자보내기
            try
            {
                FileInfo imgFile = new FileInfo(@"C:\Users\Administrator\Pictures\food.jpeg");
                byte[] imgData = new byte[0];

                if (imgFile.Exists && imgFile.Length > 0)
                    imgData = File.ReadAllBytes(imgFile.FullName);

                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(""), "user_id");                                                 //SMS 아이디
                    formData.Add(new StringContent(""), "key");                                                     //인증키
                    formData.Add(new StringContent("%고객명%님. 안녕하세요. API TEST SEND"), "msg");                // 메세지 내용
                    formData.Add(new StringContent("01111111111,01111111112"), "receiver");                         // 수신번호
                    formData.Add(new StringContent("01111111111|담당자,01111111112|홍길동"), "destination");        // 수신인 %고객명% 치환      
                    formData.Add(new StringContent(""), "sender");                                                  // 발신번호
                    formData.Add(new StringContent(""), "rdate");                                                   // 예약일자 - 20161004 : 2016-10-04일기준
                    formData.Add(new StringContent(""), "rtime");                                                   // 예약시간 - 1930 : 오후 7시30분
                    formData.Add(new StringContent(""), "testmode_yn");                                            // Y 인경우 실제문자 전송X , 자동취소(환불) 처리
                    formData.Add(new StringContent("제목입력"), "title");                                           //  LMS, MMS 제목 (미입력시 본문중 44Byte 또는 엔터 구분자 첫라인)

                    if (imgData.Length > 0)
                        formData.Add(new StreamContent(new MemoryStream(imgData)), "image", imgFile.Name);

                    client.DefaultRequestHeaders.Add("Accept", "*/*");

                    var response = client.PostAsync("https://apis.aligo.in/send/", formData).Result;

                    if (!response.IsSuccessStatusCode)
                        Console.WriteLine(response.StatusCode);
                    else
                    {
                        var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        Console.WriteLine(content);
                    }
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }
    }
}
