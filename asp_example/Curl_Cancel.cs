using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace WpfApp1.AligoSMSSample
{
    class Curl_Cancel
    {
        private void Curl_CancelEx()
        {
            /**************** 예약문자전송취소 ******************/
            /* "result_code":결과코드,"message":결과문구, */
            /** cancel_date : 취소일자  ***/
            /******************** 인증정보 ********************/
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("user_id", ""));                                  //sms 아이디
                    postData.Add(new KeyValuePair<string, string>("key", ""));                                      //인증키
                    postData.Add(new KeyValuePair<string, string>("mid", ""));                                      //취소할 메세지ID (필수 입력)

                    FormUrlEncodedContent formData = new FormUrlEncodedContent(postData);

                    client.DefaultRequestHeaders.Add("Accept", "*/*");

                    var response = client.PostAsync("https://apis.aligo.in/cancel/", formData).Result;

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
