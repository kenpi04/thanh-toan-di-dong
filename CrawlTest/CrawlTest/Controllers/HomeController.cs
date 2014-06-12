using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CrawlTest.Models;
using System.Windows.Forms;


namespace CrawlTest.Controllers
{
    
    public class HomeController : Controller
    {
        public static string COOKIE = "";
        public static string hidden = "";
        CookieCollection cookies = new CookieCollection();
        NetworkHelper net = new NetworkHelper();
        CookieContainer cookieContainer = new CookieContainer();
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult GetData()
        {
            var model = new GetDataModel();
            return View(model);
        }
       
        [HttpPost]
        public ActionResult GetData(GetDataModel model)
        {
            string url = "http://chonso.vinaphone.com.vn/numstore/dwr/exec/DataRemoting.getDoc.dwr";
            string pattern = "<table.*?>(.*?)<\\/table>";           
           string func = string.Format("'{0}','{1}','{2}','{3}','{4}',{5}",model.SoDT,model.NumberType,model.DauSo,model.StatusId,model.PageIndex,10);

           string callFunc = "neo.numstore.doc_layds_thuebao_1";
            if (model.StatusId == "1")
                callFunc = "neo.numstore.doc_layds_thuebao_dk";
            int random= new Random().Next(1,1001);
            DateTime st=new DateTime(1970,1,1);
            TimeSpan t= (DateTime.Now.ToUniversalTime()-st);
            string id = (random + "_" + t.TotalMilliseconds);
            string data = "callCount=1&c0-scriptName=DataRemoting&c0-methodName=getDoc&c0-id="+id;
                     data+="&c0-param0=string:"+callFunc+"("+func+")";
                     data += "&c0-param1=boolean:false";
                     data+="&xml=true";
                 string dataRes = GetDataFromSever(data);
             data = "callCount=1&c0-scriptName=DataRemoting&c0-methodName=getDoc&c0-id=" + id;
                     data += "&c0-param0=string:neo.numstore.doc_layth_thuebao('','','" + model.PageIndex + "','10')";
                     data += "&c0-param1=boolean:false";
                     data += "&xml=true";
                     string dataPage = GetDataFromSever(data);
            return Json(dataRes+dataPage,JsonRequestBehavior.AllowGet);
              
         
         
          
            
        }
        protected string PostData(string url, string method, string data)
        {
            
            string pattern = "<table.*?>(.*?)<\\/table>";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = method;

            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            WebResponse response = request.GetResponse();

            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            MatchCollection matches = Regex.Matches(responseFromServer, pattern);
            if (matches.Count > 0)
                responseFromServer = matches[0].Value;
            return Regex.Unescape(responseFromServer);
        }
        protected string GetDataFromSever(string data)
        {
            string url = "http://chonso.vinaphone.com.vn/numstore/dwr/exec/DataRemoting.getDoc.dwr";
            return PostData(url, "POST", data);
           
        }
      
        public ActionResult SendSms()
        {
            var model = new SendSmsModel();

            #region open login page
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create("https://vinaphone.com.vn/auth/login?service=http%3A%2F%2Fvinaphone.com.vn%3A80%2Flogin.jsp%3Flang%3Dvi");
            getRequest.Method = "GET";
             var response  = getRequest.GetResponse() as HttpWebResponse;
            var newStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(newStream);
             string responseFromServerLogin = reader.ReadToEnd();
            string pattern2 = @"name=""lt"" value=""([^""]*)""";
            hidden = Regex.Match(responseFromServerLogin, pattern2).Groups[1].Value;
          
            reader.Close();
            response.GetResponseStream().Close();
            response.GetResponseStream().Dispose();
            response.Close();
            #endregion
            #region Login
            getRequest = (HttpWebRequest)WebRequest.Create("https://vinaphone.com.vn/auth/login?service=http%3A%2F%2Fvinaphone.com.vn%3A80%2Flogin.jsp%3Flang%3Dvi");
            getRequest.CookieContainer = cookieContainer;          
            getRequest.Method = WebRequestMethods.Http.Post;
            getRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            getRequest.AllowWriteStreamBuffering = true;
            getRequest.ProtocolVersion = HttpVersion.Version11;
            getRequest.AllowAutoRedirect = true;
            getRequest.ContentType = "application/x-www-form-urlencoded";
            string postData = "username=0946866544&password=vzzgsz&_eventId=submit&lt=" + hidden;
            var data = Encoding.UTF8.GetBytes(postData);
            getRequest.ContentLength = data.Length;
             newStream = getRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
             response = getRequest.GetResponse() as HttpWebResponse;         
            newStream = response.GetResponseStream();
            cookieContainer.Add(new Uri("http://vinaphone.com.vn"),response.Cookies);
             reader = new StreamReader(newStream);
             responseFromServerLogin = reader.ReadToEnd();
             getRequest.Abort();
            reader.Close();
            response.GetResponseStream().Close();
            response.GetResponseStream().Dispose();
            response.Close();

    #endregion
            //Get login page
            getRequest = (HttpWebRequest)WebRequest.Create("http://vinaphone.com.vn/messaging/sms/sendSms.do");
            getRequest.Method = "GET";
            getRequest.CookieContainer = cookieContainer;
            response = getRequest.GetResponse() as HttpWebResponse;
            //textBox4.Text=(((HttpWebResponse)response).StatusDescription);
            newStream = response.GetResponseStream();
            reader = new StreamReader(newStream);
            string responseFromServerCatcha = reader.ReadToEnd();
            reader.Close();
            response.GetResponseStream().Close();
            response.GetResponseStream().Dispose();
            response.Close();
            //string pattern2 = @"id=""captchaImg1"" src=""([^""]*)""";
            //model.Captcha = Regex.Match(responseFromServerCatcha, pattern2).Groups[1].Value;
            ViewBag.Login = responseFromServerCatcha;
            return View(model);
        }
        [HttpPost]
        public ActionResult SendSms(SendSmsModel model)
        {
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://vinaphone.com.vn/messaging/sms/sendSms.do");
            request.Credentials = new NetworkCredential("84946866544", "vzzgsz", "vinaphone.com.vn");
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookieContainer;
            request.Method = "POST";
            string data = string.Format("bSubs=0979909863&message={1}&number={2}", model.PhoneNumber, model.Content, model.Captcha);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            WebResponse response = request.GetResponse();

            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();           
            return Json(responseFromServer);
        
        }

        public string Demologin()
        {
            return net.login("0946866544", "vzzgsz");
        }

     
    }
   
}
