using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;

namespace CrawlTest
{
    public class NetworkHelper
    {
       string pattern = "^.+(\\d+).+";
        private static int MAX_RETRY_SEND_WHEN_WRONG_CAPTCHA = 5;
        private static int MAX_RETRY_GET_CAPTCHA = 3;
        private int remainFreeMessage = 0;
        private bool isAccountLocked = false;
        private string cookies="";
         public NetworkHelper()
        {
            
        }

         public string Cookies
        {
            get { return cookies; }
            set { cookies = value; }
        }
        private String USER_AGENT = "Mozilla/5.0";
        private String sendPost(String url, string data, String referer)
        {

            var post = (HttpWebRequest)WebRequest.Create(url);

            // add header
            CookieContainer c = new CookieContainer();          
            post.Method = "POST";
            post.Host = "vinaphone.com.vn";
            post.UserAgent = USER_AGENT;
            post.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            post.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            post.CookieContainer=c;
            post.Headers.Add("Cookie", this.cookies);
            
            post.Referer = referer;
            post.ContentType= "application/x-www-form-urlencoded; charset=UTF-8";

            var dataPost = Encoding.UTF8.GetBytes(data);
            post.ContentLength = data.Length;
            var stream = post.GetRequestStream();
            // Send the data.
            stream.Write(dataPost, 0, dataPost.Length);
            stream.Close();

            HttpWebResponse response = post.GetResponse() as HttpWebResponse;
            var responseCode = response.StatusCode;
            var newStream = response.GetResponseStream();
            cookies = response.Headers["Set-Cookie"];
            var reader = new StreamReader(newStream);
            string responseFromServerLogin = reader.ReadToEnd();
            reader.Close();
            response.GetResponseStream().Close();
            response.GetResponseStream().Dispose();
            response.Close();
            return responseFromServerLogin;
        }
        public string login(string accountNumber, string password)
        {
            string result = "Start login!<br>";
            String loginAccountNumber = accountNumber + "";
            if (loginAccountNumber.StartsWith("0"))
            {
                //For Vina: replace 0 by 84
                loginAccountNumber = "84" + loginAccountNumber.Substring(1);
            }
            else if (!loginAccountNumber.StartsWith("84") || loginAccountNumber.Length < 11)
            {
                //Because the phoneNumber is imported from excel, '0' can disappear
                loginAccountNumber = "84" + loginAccountNumber;
            }
            string page = getPageContent(UrlContant.HOME_PAGE);
            string data = "username=" + loginAccountNumber + "&password=" + password;
            string loginResult = sendPost(UrlContant.LOGIN_URL, data, UrlContant.LOGIN_REFERER_URL);
            String smsPage = this.getPageContent(UrlContant.SMS_URL);
            if (smsPage.Contains(accountNumber.Substring(2)))
            {
                result = "login success!<br>";
             
            }
            return result;
        }
      
        private String getPageContent(String url)
        {

		
		    HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(url);
            getRequest.Method = "GET";
            getRequest.Headers.Add("Cookie", this.cookies);
             var response  = getRequest.GetResponse() as HttpWebResponse;
            var newStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(newStream);
             string responseFromServerLogin = reader.ReadToEnd();
             this.cookies = response.Headers["Set-Cookie"];
	       
		     reader.Close();
            response.GetResponseStream().Close();
            response.GetResponseStream().Dispose();
            response.Close();
            return responseFromServerLogin;
	}
    }
}