using System;
using System.Web.UI;

namespace WebApplication2
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
			string SECURE_SECRET = "18D7EC3F36DF842B42E1AA729E4AB010"; 
			// Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
			VPCRequest conn = new VPCRequest(virtualPaymentClientURL.Text);
			conn.SetSecureSecret(SECURE_SECRET);
			// Add the Digital Order Fields for the functionality you wish to use
			// Core Transaction Fields
            conn.AddDigitalOrderField("AgainLink", "http://onepay.vn");
			conn.AddDigitalOrderField("Title", "onepay paygate");
			conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
			conn.AddDigitalOrderField("vpc_Version", vpc_Version.Text);
			conn.AddDigitalOrderField("vpc_Command", vpc_Command.Text);
			conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant.Text);
			conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode.Text);
			conn.AddDigitalOrderField("vpc_MerchTxnRef", /*vpc_MerchTxnRef.Text*/ Guid.NewGuid().ToString());
			conn.AddDigitalOrderField("vpc_OrderInfo", vpc_OrderInfo.Text);
			conn.AddDigitalOrderField("vpc_Amount", vpc_Amount.Text);
			conn.AddDigitalOrderField("vpc_ReturnURL", vpc_ReturnURL.Text);
			// Thong tin them ve khach hang. De trong neu khong co thong tin
			conn.AddDigitalOrderField("vpc_SHIP_Street01", "194 Tran Quang Khai");
			conn.AddDigitalOrderField("vpc_SHIP_Provice", "Hanoi");
			conn.AddDigitalOrderField("vpc_SHIP_City", "Hanoi");
			conn.AddDigitalOrderField("vpc_SHIP_Country", "Vietnam");
			conn.AddDigitalOrderField("vpc_Customer_Phone", "043966668");
			conn.AddDigitalOrderField("vpc_Customer_Email", "support@onepay.vn");
			conn.AddDigitalOrderField("vpc_Customer_Id", "onepay_paygate");
			// Dia chi IP cua khach hang
			conn.AddDigitalOrderField("vpc_TicketNo", vpc_TicketNo.Text);
			// Chuyen huong trinh duyet sang cong thanh toan
			String url = conn.Create3PartyQueryString();
			Page.Response.Redirect(url);

            //int rows = 19;
            //string seperator = "?";
            //string SECURE_SECRET = "18D7EC3F36DF842B42E1AA729E4AB010"; 
            //string[,] MyArray =
			//{
			//{"AgainLink","http://onepay.vn"},
         	//{"Title","ASP VPC 3-Party" }  ,
			//{"vpc_AccessCode",vpc_AccessCode.Text},
			//{"vpc_Amount",vpc_Amount.Text},
			//{"vpc_Command",vpc_Command.Text	}, 
            //{"vpc_Customer_Email",vpc_Customer_Email.Text},
            //{"vpc_Customer_Id",vpc_Customer_Id.Text},
            //{"vpc_Customer_Phone",vpc_Customer_Phone.Text},
			//{"vpc_Locale",vpc_Locale.Text},  
            //{"vpc_MerchTxnRef",vpc_MerchTxnRef.Text},			
            //{"vpc_Merchant",vpc_Merchant.Text},			
			//{"vpc_OrderInfo",vpc_OrderInfo.Text},
			//{"vpc_ReturnURL",vpc_ReturnURL.Text}, 
            //{"vpc_SHIP_City",vpc_SHIP_City.Text},
            //{"vpc_SHIP_Country",vpc_SHIP_Country.Text},
            //{"vpc_SHIP_Provice",vpc_SHIP_Provice.Text},
            //{"vpc_SHIP_Street01",vpc_SHIP_Street01.Text},                    				
			//{"vpc_TicketNo",vpc_TicketNo.Text},
			//{"vpc_Version",vpc_Version.Text}
            //};
            
            //string redirectURL = virtualPaymentClientURL.Text;
            //for (int i = 0; i < rows; i++)
            //{                
            //    redirectURL = redirectURL + seperator + Server.UrlEncode(MyArray[i, 0]) + "=" + Server.UrlEncode(MyArray[i, 1]);               
            //    seperator = "&";
            //}
            
            //string md5HashData = SECURE_SECRET;
            //for (int k = 0; k < rows; k++)
            //{
            //    string tmp = MyArray[k, 1].ToString();
            //    if (tmp.Length > 0)
            //    {
            //        md5HashData = md5HashData + tmp;
            //    }
            //}           
            //string doSecureHash = DoMD5(md5HashData);
            // redirectURL = redirectURL.Replace("_", "%5F");
            //redirectURL = redirectURL + seperator + "vpc_SecureHash=" + doSecureHash;
            //Response.Redirect(redirectURL);
        }
        //public static string ToHexa(byte[] data)
        //{
        //    System.Text.StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < data.Length; i++)
        //        sb.AppendFormat("{0:X2}", data[i]);
        //    return sb.ToString();
        //}
        //public static string DoMD5(string SData)
        //{   
        //    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //    System.Text.UTF8Encoding encode = new System.Text.UTF8Encoding();
        //    byte[] result1 = md5.ComputeHash(encode.GetBytes(SData));
        //    string sResult2 = ToHexa(result1);
        //    return sResult2;
        //}       

        
    }
}
