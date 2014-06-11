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

namespace CrawlTest.Controllers
{
    public class HomeController : Controller
    {
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
            Json(Regex.Unescape(responseFromServer));
        }
        public ActionResult SendSms()
        {
            var model = new SendSmsModel();
            return View(model);
        }
    }
   
}
