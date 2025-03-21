using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace WpfApp1.AligoSMSSample
{
    class Curl_Send_Mass
    {

        private void Curl_Send_MassEx()
        {
            /**************** 문자보내기(대량) ******************/
            /* "result_code":결과코드,"message":결과문구, */
            /* "msg_id":메세지ID,"error_cnt":에러갯수,"success_cnt":성공갯수 */
            /* 각각 다른 개별 내용 > 동시 전송용 입니다.  
            /******************** 인증정보 ********************/
            try
            {
                FileInfo imgFile = new FileInfo(@"C:\Users\Administrator\Pictures\food.jpeg");
                byte[] imgData = new byte[0];

                if (imgFile.Exists && imgFile.Length > 0)
                    imgData = File.ReadAllBytes(imgFile.FullName);

                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(""), "user_id");                                 //SMS 아이디
                    formData.Add(new StringContent(""), "key");                                     //인증 키
                    formData.Add(new StringContent(""), "sender");                                   // 발신번호//발신번호
                    formData.Add(new StringContent(""), "rdate");                                   // 예약일자 - 20161004 : 2016-10-04일기준
                    formData.Add(new StringContent(""), "rtime");                                   // 예약시간 - 1930 : 오후 7시30분
                    formData.Add(new StringContent(""), "testmode_yn");                             // Y 인경우 실제문자 전송X , 자동취소(환불) 처리
                    formData.Add(new StringContent("MMS"), "msg_type");                             // SMS(단문) , LMS(장문), MMS(그림문자)  = 필수항목

                    string sendMessage = "(광고)알리고\n회원님 알리고를 추천드려요!!\n무료거부:080xxxxxxxxx";
                    int SendCount = 2;  //max 500

                    for (int iCnt = 1; iCnt <= SendCount; iCnt++)
                    {
                        string sendMsg = ReplaceFirst(sendMessage, "회원님", string.Concat("회원", iCnt, "님"));
                        formData.Add(new StringContent("수신번호(01012341234)"), "rec_" + iCnt.ToString());
                        formData.Add(new StringContent(sendMsg), "msg_" + iCnt.ToString());
                    }

                    formData.Add(new StringContent(SendCount.ToString()), "cnt");

                    if (imgData.Length > 0)
                        formData.Add(new StreamContent(new MemoryStream(imgData)), "image", imgFile.Name);

                    client.DefaultRequestHeaders.Add("Accept", "*/*");

                    var response = client.PostAsync("https://apis.aligo.in/send_mass/", formData).Result;

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

        private string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
