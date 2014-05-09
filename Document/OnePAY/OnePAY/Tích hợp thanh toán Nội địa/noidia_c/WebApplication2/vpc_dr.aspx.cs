using System;
using System.Web.UI;

namespace WebApplication2
{
    public partial class vpc_dr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SECURE_SECRET = "A3EFDFABA8653DF2342E8DAC29B51AF0";
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
            string cardBin = conn.GetResultField("vpc_Card", "Unknown");
            string orderInfo = conn.GetResultField("vpc_OrderInfo", "Unknown");
            string merchantID = conn.GetResultField("vpc_Merchant", "Unknown");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId", "Unknown");
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef", "Unknown");
            string transactionNo = conn.GetResultField("vpc_TransactionNo", "Unknown");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message", "Unknown");
            //int loop1;

            // Bo cac ham ma hoa du lieu cu
            //NameValueCollection coll = Request.QueryString;
            //// Get names of all keys into a string array.
            //String[] arr1 = coll.AllKeys;
            //for (int j = 0; j < arr1.Length;j++ )
            //{
            //    arr1[j] = Server.HtmlEncode(arr1[j]);
            //}
            //Array.Sort(arr1, arr1);
            //string sdataHash = "";
            //for (loop1 = 0; loop1 < arr1.Length; loop1++)
            //{    
            //    String[] arr2 = coll.GetValues(arr1[loop1]);      
            //    if ((arr2[0] != null) && (arr2[0].Length > 0) && (arr1[loop1]!="vpc_SecureHash"))
            //    {
            //        sdataHash += Server.HtmlEncode(arr2[0]);
            //    }
            //}

            //    sdataHash = SECURE_SECRET + sdataHash;
            //    string doSecureHash = DoMD5(sdataHash).Trim();

            // Sua lai ham check chuoi ma hoa du lieu
            if (hashvalidateResult == "INVALIDATED")
            {
                vpc_Result.Text = "Transaction is pending";
                hashvalidateResult = "INVALIDATED";
            }
            else
            {
                if (txnResponseCode.Trim() == "0")
                {
                    vpc_Result.Text = "Transaction was paid successful";
                }
                else
                {
                    vpc_Result.Text = "Transaction was not paid successful";
                }
                hashvalidateResult = "CORRECTED";

            }
            vpc_Version.Text = version;
            vpc_Amount.Text = amount;
            this.vpc_Command.Text = command;
            vpc_MerchantID.Text = merchantID;
            vpc_MerchantRef.Text = merchTxnRef;
            vpc_OderInfor.Text = orderInfo;
            vpc_ResponseCode.Text = txnResponseCode;
            vpc_Command.Text = command;
            vpc_TransactionNo.Text = transactionNo;
            hashvalidate.Text = hashvalidateResult;
            vpc_Message.Text = message;
        }
    }
}