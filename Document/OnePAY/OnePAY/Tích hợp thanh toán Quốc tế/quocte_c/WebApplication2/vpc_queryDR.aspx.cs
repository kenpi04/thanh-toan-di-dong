using System;
using System.IO;
using System.Net;
using System.Text;

namespace WebApplication2
{
    public partial class vpc_queryDR : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SubButL_Click(object sender, EventArgs e)
        {
            string postData = "";
            string seperator = "";
            string resQS = "";
            int paras = 7;
            string vpcURL = virtualPaymentClientURL.Text;


            string[,] MyArray =
			{
			{"vpc_AccessCode",vpc_AccessCode.Text},
			{"vpc_Command",vpc_Command.Text	},           
			{"vpc_MerchTxnRef",vpc_MerchTxnRef.Text},			
            {"vpc_Merchant",vpc_Merchant.Text},						
            {"vpc_Password",vpc_Password.Text},
			{"vpc_User",vpc_User.Text},
			{"vpc_Version",vpc_Version.Text}							
			};
            for (int i = 0; i < paras; i++)
            {
                postData = postData + seperator + Server.UrlEncode(MyArray[i, 0]) + "=" + Server.UrlEncode(MyArray[i, 1]);
                seperator = "&";
            }

            resQS = doPost(vpcURL, postData);
            Response.Write(resQS);
        }

        /**
  * This method is for performing a Form POST operation from input data parameters.
  *
  * @param vpc_Host  - is a String containing the vpc URL
  * @param data      - is a String containing the post data key value pairs
  * @param useProxy  - is a boolean indicating if a Proxy Server is involved in the transfer
  * @param proxyHost - is a String containing the IP address of the Proxy to send the data to
  * @param proxyPort - is an integer containing the port number of the Proxy socket listener
  * @return          - is body data of the POST data response    
  */
        public static string doPost(string vpc_Host, string postData)
        {            
            string page = "Response:";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(vpc_Host);

          //  WebRequest request = WebRequest.Create(vpc_Host);
            request.Method = "POST";

            // Create POST data and convert it to a byte array.
            //string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.         
            request.UserAgent = "HTTP Client";
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
        //    WebResponse response = request.GetResponse();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            //   Response.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            page = page + responseFromServer;
            // Display the content.
            //  Response.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            return page;
        }
    }
}
