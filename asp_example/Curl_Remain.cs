using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace WpfApp1.AligoSMSSample
{
    class Curl_Remain
    {
        private void Curl_RemainEx()
        {
            /**************** 발송 가능 건수******************/
            /* "result_code":결과코드,"message":결과문구, */
            /** list : 전송된 목록 배열 ***/
            /******************** 인증정보 ********************/
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("user_id", ""));                          //sms 아이디
                    postData.Add(new KeyValuePair<string, string>("key", ""));                              //인증키

                    FormUrlEncodedContent formData = new FormUrlEncodedContent(postData);

                    client.DefaultRequestHeaders.Add("Accept", "*/*");

                    var response = client.PostAsync("https://apis.aligo.in/remain/", formData).Result;

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
