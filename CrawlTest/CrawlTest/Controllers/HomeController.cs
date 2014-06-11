using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using CrawlTest.Models;
using WatiN.Core;

namespace CrawlTest.Controllers
{
    
    public class HomeController : Controller
    {
        public static string COOKIE = "";
        IE _browser = null;
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
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            WebRequest request = (HttpWebRequest)WebRequest.Create("https://vinaphone.com.vn/auth/login?service=http%3A%2F%2Fvinaphone.com.vn%3A80%2Flogin.jsp%3Flang%3Dvi");
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";           
            string data = "username=0946866544&password=vzzgsz";
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            WebResponse response = request.GetResponse();

            CookieContainer cookies = new CookieContainer();
            var cookieHeader = response.Headers["Set-cookie"];
            //_browser.SetCookie("vinaphone.com.vn",cookieHeader);
            COOKIE = cookieHeader;
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
        public ActionResult SendSms()
        {
            
            var model = new SendSmsModel();
            ViewBag.COOKIE = COOKIE;
            return View(model);
        }
        [HttpPost]
        public ActionResult SendSms(SendSmsModel model)
        {
            if (_browser.GetCookiesForUrl(new Uri("http://vinaphone.com.vn"))==null)
                return Json("NOTLOGIN");
            WebRequest request = (HttpWebRequest)WebRequest.Create("http://vinaphone.com.vn/messaging/sms/sendSms.do");
            request.Credentials = new NetworkCredential("0946866544", "vzzgsz", "vinaphone.com.vn");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Cookie", COOKIE);
            request.Method = "POST";
            string data = string.Format("bSubs={0}&message={1}&number={2}", model.PhoneNumber, model.Content, model.Captcha);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            WebResponse response = request.GetResponse();

            CookieContainer cookies = new CookieContainer();
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
     
        public ActionResult Send()
        {
            var model = new SendSmsModel();
            _browser = new IE("http://vinaphone.com.vn/messaging/sms/sms.do");
            model.CaptchaImageUrl = _browser.Image("captchaImg1").Src;
            return View(model);
        }
        [HttpPost]
        public ActionResult Send(SendSmsModel model)
        {
           
                _browser.TextField(Find.ByName("bSubs")).TypeText(model.PhoneNumber);
                _browser.TextField(Find.ByName("message")).TypeText(model.Content);
                _browser.TextField(Find.ByName("number")).TypeText(model.Captcha);
                _browser.Button(Find.ByName("submit")).Click();
                string html = _browser.Html;
                _browser.Dispose();

                return Json(html);
        }
    }
   
}
