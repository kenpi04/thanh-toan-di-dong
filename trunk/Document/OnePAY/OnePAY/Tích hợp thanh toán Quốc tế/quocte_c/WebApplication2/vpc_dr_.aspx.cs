using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;  
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace WebApplication1
{
	/// <summary>
	/// Summary description for VPC_ASP_3P_DR.
	/// Version 2.0
	/// Copyright 2007 OnePAY JSC.  All rights reserved.
	/// </summary>
   
  

	public partial class vpc_dr : System.Web.UI.Page
	{
       

	    
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string SECURE_SECRET = "18D7EC3F36DF842B42E1AA729E4AB010";
			string hashvalidateResult = "";
			// Khoi tao lop thu vien
			VPCRequest conn = new VPCRequest("http://onepay.vn");
			conn.SetSecureSecret(SECURE_SECRET);
			// Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
			hashvalidateResult = conn.Process3PartyResponse(Page.Request.QueryString);
			// Lay gia tri tham so tra ve tu cong thanh toan
			String vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode", "Unknown");
			string amount = conn.GetResultField("vpc_Amount", "Unknown");
			string localed = conn.GetResultField("vpc_Locale", "Unknown"); 
			string command = conn.GetResultField("vpc_Command", "Unknown");
			string version = conn.GetResultField("vpc_Version", "Unknown");
			//string cardBin = conn.GetResultField("vpc_Card", "Unknown");
			string cardType        = null2unknown(Request.QueryString["vpc_Card"])		  ;
			
			string orderInfo = conn.GetResultField("vpc_OrderInfo", "Unknown");
			string merchantID = conn.GetResultField("vpc_Merchant", "Unknown");
			string authorizeID = conn.GetResultField("vpc_AuthorizeId", "Unknown");
			string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef", "Unknown");
			string transactionNo = conn.GetResultField("vpc_TransactionNo", "Unknown");
			string acqResponseCode = null2unknown(Request.QueryString["vpc_AcqResponseCode"]);
			
			string txnResponseCode = vpc_TxnResponseCode;
			string message = conn.GetResultField("vpc_Message", "Unknown");
			
			/*
            string SECURE_SECRET = "18D7EC3F36DF842B42E1AA729E4AB010";
			string Version = Request.QueryString["vpc_Version"];
			string amount          = null2unknown(Request.QueryString["vpc_Amount"]) ;
			string localed          = null2unknown(Request.QueryString["vpc_Locale"])  ;	
			string command         = null2unknown(Request.QueryString["vpc_Command"])	 ;
			string version         = null2unknown(Request.QueryString["vpc_Version"])	 ;
			string cardType        = null2unknown(Request.QueryString["vpc_Card"])		  ;
			string orderInfo       = null2unknown(Request.QueryString["vpc_OrderInfo"])	  ;		
			string merchantID      = null2unknown(Request.QueryString["vpc_Merchant"])	  ;
			string authorizeID     = null2unknown(Request.QueryString["vpc_AuthorizeId"])  ;
			string merchTxnRef     = null2unknown(Request.QueryString["vpc_MerchTxnRef"])   ;
			string transactionNo   = null2unknown(Request.QueryString["vpc_TransactionNo"])	;
			string acqResponseCode = null2unknown(Request.QueryString["vpc_AcqResponseCode"]);
			string txnResponseCode = null2unknown(Request.QueryString["vpc_TxnResponseCode"]);
			string recive_SecureHash = null2unknown(Request.QueryString["vpc_SecureHash"]).ToString().Trim();		
			string 	message     = null2unknown(Request.QueryString["vpc_Message"])   ;
			string hashvalidateResult = "";
			//string vpc_AVS_Street01 = null2unknown(Request.QueryString["vpc_AVS_Street01"]);
			int loop1;
			
			// Load NameValueCollection object.
			NameValueCollection coll = Request.QueryString;
			// Get names of all keys into a string array.
			String[] arr1 = coll.AllKeys;
			for (int j = 0; j < arr1.Length;j++ )
			{
				arr1[j] = Server.HtmlEncode(arr1[j]);//WARN
			}
		    //MyCompareable mc = new MyCompareable();  
		    // Array.Sort(arr1,mc);
			Array.Sort(arr1, StringComparer.Ordinal);      
			string sdataHash = "";
			for (loop1 = 0; loop1 < arr1.Length; loop1++)
			{
				//Response.Write("Key: " + Server.HtmlEncode(arr1[loop1]) + "<br>");


				String[] arr2 = coll.GetValues(arr1[loop1]);
				for (int loop2 = 0; loop2 < arr2.Length; loop2++)
				{
				 //   Response.Write(arr1[loop1] + ":" + Server.HtmlEncode(arr2[loop2]) + "<br>");
					
					
				}
				if ((arr2[0] != null) && (arr2[0].Length > 0) && (arr1[loop1]!="vpc_SecureHash"))
				{
				   
				 //  sdataHash += Server.HtmlEncode(arr2[0]);
					sdataHash += arr2[0];
				  
				}
			}
            
            sdataHash = SECURE_SECRET + sdataHash;

            //vpc_AVS_Street01 = Server.HtmlEncode(coll.GetValues("vpc_AVS_Street01")[0]);
            // vpc_AVS_Street01 = coll.GetValues("vpc_AVS_Street01")[0];
            //vpc_AVS_Street01 = Request["vpc_AVS_Street01"];
            // Response.Write("vpc_AVS_Street=" + vpc_AVS_Street01+"<br>");
            //  System.Text.Encoding encode = System.Text.Encoding.GetEncoding("ISO-8859-1");
            //char c = vpc_AVS_Street01.ToCharArray()[2];
            
            //System.Text.Encoding encode = new System.Text.UnicodeEncoding();
           //   Response.Write("vpc_AVS_StreetHex=" + ToHexa(encode.GetBytes(vpc_AVS_Street01))+"#");
          //    Response.Write("sdataHash=" + ToHexa(encode.GetBytes(sdataHash)) + "#");
            //byte[] result1 = md5.ComputeHash(encode.GetBytes(SData));

            //string doSecureHash = DoMD51(sdataHash).Trim();
            string doSecureHash = DoMD5(sdataHash).Trim();
          //  Response.Write("sdataHash :" + doSecureHash  + "<br>");
          //  Response.Write("sdataHash :" + sdataHash + "<br>");
         //   Response.Write("doSecureHash      " + doSecureHash + "<br>");
         //   Response.Write("recive_SecureHash " + recive_SecureHash + "<br>");
		 */
		 
		 // Sua lai ham check chuoi ma hoa du lieu
            //if (recive_SecureHash !=doSecureHash)
			if (hashvalidateResult == "INVALIDATED")
            {
                vpc_Result.Text = "Transaction is pending";
                hashvalidateResult = "INVALIDATED";
               // Response.Write("Hash value is equal=> Transaction is successful");
            }else
            {
                if (txnResponseCode.Trim() == "0") {
                    vpc_Result.Text = "Transaction was paid successful";
                } else {
                    vpc_Result.Text = "Transaction was not paid successful";
                }
                hashvalidateResult = "CORRECTED";
               // Response.Write("Hash value is not equal=> Transaction is unsuccessful"); 
            }
            vpc_Version.Text = version;
			vpc_Amount.Text=amount;
			this.vpc_Command.Text=command;
			vpc_MerchantID.Text=merchantID;
			vpc_MerchantRef.Text=merchTxnRef;
			vpc_OderInfor.Text=orderInfo;
			vpc_ResponseCode.Text=txnResponseCode;			     
            vpc_Command.Text = command;         
            vpc_TransactionNo.Text = transactionNo;
            hashvalidate.Text = hashvalidateResult;
            vpc_Message.Text = message;
		}

		/*
		private static string null2unknown(string sInput)
		{
			return sInput+" ";

		}
        public static string ToHexa(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sb.AppendFormat("{0:X2}", data[i]);
            return sb.ToString();
        }
        public static string DoMD5(string SData)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
           System.Text.Encoding encode = System.Text.Encoding.GetEncoding("ISO-8859-1");
           byte[] result1 = md5.ComputeHash(encode.GetBytes(SData));
            string sResult2 = ToHexa(result1);
         //   string sResult2 = HexEncoding.ToString(byteArray)
           return sResult2;

           // System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //System.Text.UTF8Encoding encode = new System.Text.UTF8Encoding();
          //  byte[] result1 = md5.ComputeHash(encode.GetBytes(SData));
          //  string sResult2 = ToHexa(result1);
          //  return sResult2;
        }
        */
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
