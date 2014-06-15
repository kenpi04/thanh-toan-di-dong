using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.WebPages.Html;
using System.Collections.Specialized;
using System.Drawing;

namespace CrawlTest
{
    public class NetworkHelper
    {
        string pattern = "^.+(\\d+).+";
        private static int MAX_RETRY_SEND_WHEN_WRONG_CAPTCHA = 5;
        private static int MAX_RETRY_GET_CAPTCHA = 3;
        private int remainFreeMessage = 0;
        private bool isAccountLocked = false;
        public static string cookieHeader = "";
        private CookieContainer cookies;
        public NetworkHelper()
        {
            cookies = new CookieContainer();
        }

        public CookieContainer Cookies
        {
            get { return cookies; }
            set { cookies = value; }
        }
        private String USER_AGENT = "Mozilla/5.0";
        private String sendPost(String url, string data, String referer)
        {
            var post = (HttpWebRequest)WebRequest.Create(url);

            // add header

            post.Method = "POST";
            post.Host = "vinaphone.com.vn";
            post.UserAgent = USER_AGENT;
            post.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            post.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            post.Referer = referer;
            post.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            post.KeepAlive = true;
            post.Headers.Add("Cookie", cookieHeader);
            post.AllowAutoRedirect = false;

            var dataPost = Encoding.UTF8.GetBytes(data);
            post.ContentLength = dataPost.Length;
            var stream = post.GetRequestStream();
            // Send the data.
            stream.Write(dataPost, 0, dataPost.Length);
            stream.Close();
            HttpWebResponse response = post.GetResponse() as HttpWebResponse;
            var responseCode = response.StatusCode;

            cookieHeader = response.Headers["Set-Cookie"];
            var newStream = response.GetResponseStream();
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
            String smsPage = this.getPageContent(UrlContant.SMS_URL);
            if (!smsPage.Contains(accountNumber.Substring(2)))
            {
                // string page = getPageContent(UrlContant.HOME_PAGE);
                string data = GetDataLogin(smsPage, accountNumber, password);
                string loginResult = sendPost(UrlContant.LOGIN_URL, data, UrlContant.LOGIN_REFERER_URL);
                smsPage = this.getPageContent(UrlContant.SMS_URL);
            }
            if (smsPage.Contains(accountNumber.Substring(2)))
            {
                result += "Login success!-";
                string cateCha = "";
                result += sendSMS(smsPage, "Chao ban!", "0979909863", "MP", cateCha);

            }
            return result;
        }

        private String getPageContent(String url)
        {


            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(url);
            getRequest.Method = "GET";
            getRequest.Host = "vinaphone.com.vn";
            getRequest.UserAgent = USER_AGENT;
            getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            getRequest.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            getRequest.Headers.Add("Cookie", cookieHeader);

            var response = getRequest.GetResponse() as HttpWebResponse;
            cookieHeader = response.Headers["Set-Cookie"];
            var newStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(newStream);
            string responseFromServerLogin = reader.ReadToEnd();
            reader.Close();
            response.GetResponseStream().Close();
            response.GetResponseStream().Dispose();
            response.Close();
            return responseFromServerLogin;
        }

        private string GetDataLogin(string html, string username, string pass)
        {
            string pattern2 = @"name=""lt"" value=""([^""]*)""";
            string token = Regex.Match(html, pattern2).Groups[1].Value;
            //  string postData = "username=" + username + "&password=" + pass + "&_eventId=submit&dnstb=Đăng nhập&dnstb=Hủy&lt=" + token;
            string postData = username = "username=84946866544&password=vzzgsz&lt=" + token + "&_eventId=submit&dnstb=%C4%90%C4%83ng+nh%E1%BA%ADp";
            return postData;
        }
        private string getDataSMS(string htmlContent, string content, string phone, string accountType, string captcha)
        {
            if (accountType.Equals("MP") || accountType.Equals("NO"))
            {//if send FREE SMS
                accountType = "FREE";
            }
            else if (accountType.Equals("CP"))
            {//if send HAS-FEE SMS
                accountType = "CP";
            }
            int countdown = (130 - content.Length);
            string pattenImg = @"http://vinaphone.com.vn/PassImageServlet/([\w]+)";
            string captcharSrc = Regex.Match(htmlContent, pattenImg).ToString();
            //GetCaptcha(captcharSrc+"=?");

            string catchaCoding = Regex.Match(htmlContent, @"name=""passline_enc"" value=""([^""]*)""").Groups[1].Value;
            string data = string.Format("countryCode=09&bSubs={0}&message={1}&number={2}&countdown={3}&passline_enc={4}&note={5}&submit=", phone, HttpUtility.UrlEncode(content), captcha, countdown, catchaCoding, accountType);
            return data;

        }
        private void GetCaptcha(string url)
        {
            DateTime st = new DateTime(1970, 1, 1);
            var t = (DateTime.Now.ToUniversalTime() - st).TotalMilliseconds;
            url += Math.Round(t);
            var request = HttpWebRequest.Create(url);
            request.Method = "GET";
            var stream = request.GetResponse().GetResponseStream();
            string filePath = HttpContext.Current.Server.MapPath("~/Content/imgcaptcha_" + t + ".jpg");
            Image img = Image.FromStream(stream);
            img.Save(filePath);

        }
        private string sendSMS(string pageSMS, string content, string phone, string accountType, string captcha)
        {

            if (parseRemainingFreeSms(pageSMS) > 0)
            {
                string dataSMS = getDataSMS(pageSMS, content, phone, accountType, captcha);
                string resultSendSms = sendPost(UrlContant.SEND_SMS_RESULT_URL, dataSMS, string.Format(UrlContant.SMS_REFERER_URL, phone));
                if (resultSendSms.Contains("vnp_files/image/success.png"))
                    return "VINA - Gui SMS Thanh Cong !!! - Loai SMS : " + accountType;
            }
            return "Hết tin nhắn miễn phí";

        }

        private int parseRemainingFreeSms(String htmlContent)
        {
            try
            {

                String freeSms = GetHtmlBetween(htmlContent);
                return int.Parse(freeSms);
            }
            catch
            {
                return 0;
            }
        }
        private string GetHtmlBetween(string html)
        {
            Regex r = new Regex(@"<span class=""alert"">([^""]*)</span>");
            var match = r.Match(html);
            if (match.Length > 0)
                return match.Groups[1].Value;
            return "";
        }

    }
}