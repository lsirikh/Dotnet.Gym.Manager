using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace WpfApp1.AligoSMSSample
{
    class Curl_SMS_List
    {
        private void Curl_SMS_ListEx()
        {
            /*************  문자전송 결과 상세보기 *****************/
            /** SMS_CNT / LMS_CNT / MMS_CNT : 전송유형별 잔여건수 ***/
            /******************** 인증정보 ********************/
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("user_id", ""));                                  //sms 아이디
                    postData.Add(new KeyValuePair<string, string>("key", ""));                                      //인증키
                    postData.Add(new KeyValuePair<string, string>("mid", "1"));                                     //메세지 id
                    postData.Add(new KeyValuePair<string, string>("page", "0"));                                    //출력 갯수
                    postData.Add(new KeyValuePair<string, string>("page_size", "10"));                              //출력갯수

                    FormUrlEncodedContent formData = new FormUrlEncodedContent(postData);

                    client.DefaultRequestHeaders.Add("Accept", "*/*");

                    var response = client.PostAsync("https://apis.aligo.in/sms_list/", formData).Result;

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
