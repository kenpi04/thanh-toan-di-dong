using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net;
using System.Configuration;

namespace VUDemo
{
    public class GetTransactionDetailIn
    {
        public long? AgentID;
        public long? OrderMasterID;
        public string OrderNo;
        public long PaySiteID;
        public int TransactionTypeId;
    }
    public class GetTransactionDetailRequest : GetTransactionDetailIn
    {
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Create new instance.
            VUService.UniGWS uniGW = new VUService.UniGWS();

            // Set UniGWS Request
            VUService.UniGWSRequest request = new VUService.UniGWSRequest();

            request.ClientId = ConfigurationManager.AppSettings["ClientId"];
            string clientPassword = ConfigurationManager.AppSettings["ClientPassword"];
            string privateKeyUrl = ConfigurationManager.AppSettings["PrivateKeyUrl"];
            string passwordForPrivateKey = ConfigurationManager.AppSettings["PasswordForPrivateKey"];
            string publicKey = ConfigurationManager.AppSettings["PublicKeyUrl"];
            // End
            
            
            System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

            string data = string.Empty;


            

            try
            {
                // Demo for QueryBillEx
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                // Checksum = MD5(RequestTime + ClientPassword)
                request.Checksum = GetMD5Hash(strRequestTime + clientPassword);
                // End

                request.Operation = "MMS_QueryBillEx"; // Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.
                
                QueryBillExRequest objQueryBill = SetQueryBillEx();

                data = XmlSerializeLib.XmlSerialize(objQueryBill);
                //Sign data
                request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

                request.RequestData = data;

                //bool isCorrect = Verify(data, request.Signature, publicKey);

                string demo = XmlSerializeLib.XmlSerialize(request);

                // Call to VU Service

                Console.WriteLine("Processing !!!!!!!!!!!!!");
                VUService.UniGWSResponse objResponse = uniGW.Execute2(request);

                // End

                // Verify signature.

                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
                if (!isCorrect)
                {
                    Console.WriteLine("Verify sign data is not correct.");
                    Console.ReadLine();
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    QueryBillExResult objQueryBillResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(QueryBillExResult))
                        as QueryBillExResult;

                    if (objQueryBillResult.ReturnCode == 0) // Sucessfully
                    {
                        // Do something



                        Console.WriteLine(objResponse.ResponseData);
                    }
                    else
                    {
                        // Do something



                        Console.WriteLine("The processing is error.! ReturnCode = " + objQueryBillResult.ReturnCode);
                    }
                }
                // End
                Console.ReadLine();
                // End

            }
            catch (Exception objEx)
            {
                throw objEx;
            }
        }

        private static GetServicesRequest SetGetService()
        {
            GetServicesRequest getservice = new GetServicesRequest();
            getservice.UserId = "DEMO";
            getservice.AgentId = "31";
            return getservice;
        }

        private static GetProvidersRequest SetGetProvider()
        {
            GetProvidersRequest getprovider = new GetProvidersRequest();
            getprovider.UserId = "VIETNGA";
            getprovider.AgentId = "24";
            return getprovider;
        }

        /// <Author>Chu.Pham</Author>
        /// <summary>
        /// This function is used to set querybill value.
        /// </summary>
        /// <returns>
        /// QueryBill object.
        /// </returns>
        private static QueryBillExRequest SetQueryBillEx()
        {
            QueryBillExRequest queryInput = new QueryBillExRequest();
            queryInput.AgentId = "34";
            queryInput.CustomerId = "PE11000003298";
            queryInput.ProviderId = "EVNSG";
            queryInput.ServiceId = "DIEN";
            queryInput.UserId = "01213513716";
            return queryInput;
        }

        /// <author>Chu.Pham</author>
        /// <summary>
        /// This function is used to MD5.
        /// </summary>
        /// <param name="strInput">
        /// Value
        /// </param>
        /// <returns>new string</returns>
        public static string GetMD5Hash(string strInput)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider objMD5Crypt = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytInputData = System.Text.Encoding.UTF8.GetBytes(strInput);
            bytInputData = objMD5Crypt.ComputeHash(bytInputData);
            System.Text.StringBuilder objStrBuilder = new System.Text.StringBuilder();
            foreach (byte bytElement in bytInputData)
            {
                objStrBuilder.Append(bytElement.ToString("x2").ToLower());
            }
            return objStrBuilder.ToString();
        }

        /// <Author>Chu.Pham</Author>
        /// <summary>
        /// This function is used signature.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// data string signed.
        /// </returns>
        public static string SignData(string data, string privateKeyUrl, string passwordForPrivateKey)
        {
            try
            {
                byte[] datas = File.ReadAllBytes(privateKeyUrl);
                CmsCryptography objSign = new CmsCryptography();
                //objSign.CharacterEncoding = Encoding.Default.BodyName;
                objSign.LoadSignerCredential(datas,
                    passwordForPrivateKey);
                return objSign.Sign(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <Author>Chu.Pham</Author>
        /// <summary>
        /// This function is used to verify signature.
        /// </summary>
        /// <param name="signedData"></param>
        /// <param name="signature"></param>
        /// <returns>
        /// True / false
        /// True: Verify signature is correct.
        /// False: Verify signature is not correct.
        /// </returns>
        public static bool Verify(string signedData, string signature, string publicKeyUrl)
        {
            //Log.Write(Encoding.Default.BodyName, "PKCS7");
            bool result = true;

            ContentInfo content = new ContentInfo(Encoding.Default.GetBytes(signedData));
            SignedCms signatureVerifier = new SignedCms(content, true);

            try
            {
                X509Certificate2 cert = new X509Certificate2(publicKeyUrl);
                signatureVerifier.Decode(Convert.FromBase64String(signature));
                X509Certificate2Collection certCollection = new X509Certificate2Collection(cert);
                signatureVerifier.CheckSignature(certCollection, true);
                if (signatureVerifier.Certificates.Count > 0)
                {
                    if (!signatureVerifier.Certificates[0].Equals(cert))
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Write(ex, "PKCS7");
                result = false;
            }

            return result;
        }
    }

    public class CertPolicy : ICertificatePolicy
    {
        /// <author>Dung Do</author>
        ///<cm>H</cm>
        /// <summary>
        /// Handles CheckValidationResult of the ICertificatePolicy.
        /// </summary>
        /// <param name="srvPoint">The ServicePoint of remote service.</param>
        /// <param name="certificate">Certificate to be checked.</param>
        /// <param name="request">The current webservice request.</param>
        /// <param name="certificateProblem">Problem code.</param>
        /// <returns>Always returns true.</returns>
        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            // You can do your own certificate checking.
            // You can obtain the error values from WinError.h.
            // Return true so that any certificate will work with this service.
            return true;
        }

    }
}
