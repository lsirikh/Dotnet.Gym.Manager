using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WpfApp1.AligoSMSSample
{
    class Curl_List
    {
        private void Curl_ListEx()
        {
            //최근 전송 목록 
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("user_id", ""));      //sms 아이디
                    postData.Add(new KeyValuePair<string, string>("key", ""));          //인증키
                    postData.Add(new KeyValuePair<string, string>("page", "1"));        //조회 시작번호1
                    postData.Add(new KeyValuePair<string, string>("page_size", "10"));  //출력 갯수
                    postData.Add(new KeyValuePair<string, string>("start_date", ""));   //조회일 시작
                    postData.Add(new KeyValuePair<string, string>("limit_day", "7"));   //조회 일수

                    FormUrlEncodedContent formData = new FormUrlEncodedContent(postData);

                    //var image = File.ReadAllBytes("c:\\test.png");
                    client.DefaultRequestHeaders.Add("Accept", "*/*");

                    var response = client.PostAsync("https://apis.aligo.in/list/", formData).Result;

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
