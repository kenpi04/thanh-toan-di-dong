using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using _mvcOnePay.Models;

namespace _mvcOnePay.Controllers
{
    public class VisaCardController : Controller
    {
        //
        // GET: /VisaCard/

        public ActionResult Index()
        {
            Transaction inputTran = new Transaction()
            {
                vpc_Amount = "1000",
                vpc_OrderInfo = "thong tin hoa don"
            };
            return View(inputTran);
        }

        [HttpPost]
        public ActionResult Index(Transaction tran)
        {
            if (ModelState.IsValid)
            {
                string SECURE_SECRET = "18D7EC3F36DF842B42E1AA729E4AB010"; //HashCode (Tên khác là SECURE_SECRET)
                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest("http://mtf.onepay.vn/vpcpay/vpcpay.op"); // Link checkout test của OnePay
                conn.SetSecureSecret(SECURE_SECRET);

                // Thông tin về các trường trong giao dịch
                conn.AddDigitalOrderField("AgainLink", "http://onepay.vn");
                conn.AddDigitalOrderField("Title", "onepay paygate");
                conn.AddDigitalOrderField("vpc_Locale", "vn"); //Chọn ngôn ngữ hiển thị khi thanh toán(vn/en)
                conn.AddDigitalOrderField("vpc_Version", "2"); // Gán giá trị Version mặc định = 2
                conn.AddDigitalOrderField("vpc_Command", "pay"); // Gán giá trị Type_Comment mặc định là pay
                conn.AddDigitalOrderField("vpc_Merchant", "TESTONEPAYUSD"); // Mở file info.txt để lấy thông tin MerchantID. ONEPAY chỉ là ví dụ
                conn.AddDigitalOrderField("vpc_AccessCode", "614240F4"); // Mở file info.txt để lấy thông tin AccessCode. D67342C2 chỉ là ví dụ
                conn.AddDigitalOrderField("vpc_MerchTxnRef", Guid.NewGuid().ToString()); // Mã giao dịch, lập trình viên lập trình ra mã giao dịch (viết hàm tự tăng hoặc ghi theo thời gian)
                conn.AddDigitalOrderField("vpc_OrderInfo", tran.vpc_OrderInfo); // Thông tin giao dịch
                conn.AddDigitalOrderField("vpc_Amount", tran.vpc_Amount); // Tổng giá tiền giao dịch                
                conn.AddDigitalOrderField("vpc_ReturnURL", "http://localhost:28467/VisaCard/Result"); // Link trả về sau khi thanh toán

                String sIP;
                sIP = Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString();

                // Thông tin về khách hàng
                conn.AddDigitalOrderField("vpc_SHIP_Street01", "124 Ton That Dam");
                conn.AddDigitalOrderField("vpc_SHIP_Provice", "HoChiMinh");
                conn.AddDigitalOrderField("vpc_SHIP_City", "HoChiMinh");
                conn.AddDigitalOrderField("vpc_SHIP_Country", "Vietnam");
                conn.AddDigitalOrderField("vpc_Customer_Phone", "0933389128");
                conn.AddDigitalOrderField("vpc_Customer_Email", "thuyhk2@fpt.com.vn");
                conn.AddDigitalOrderField("vpc_Customer_Id", "onepay_paygate"); // Mã khách hàng
                conn.AddDigitalOrderField("vpc_TicketNo", sIP); // LTV viết hàm lấy IP Client

                // Chuyển hướng dang cổng thanh toán
                String url = conn.Create3PartyQueryString();
                return Redirect(url);
            }
            else return View();
        }

        public ActionResult Result()
        {
            string SECURE_SECRET = "18D7EC3F36DF842B42E1AA729E4AB010"; //HashCode (Tên khác là SECURE_SECRET)
            string hashvalidateResult = "";
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest("http://onepay.vn"); // Link checkout test của OnePay
            conn.SetSecureSecret(SECURE_SECRET);
            // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
            hashvalidateResult = conn.Process3PartyResponse(Request.QueryString); //(Page.Request.QueryString);

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

            ResultTran resultTran = new ResultTran();
            if (hashvalidateResult == "INVALIDATED")
            {
                resultTran.vpc_Result = "Giao dịch chưa thành công.";
                hashvalidateResult = "INVALIDATED";
            }
            else
            {
                if (txnResponseCode.Trim() == "0")
                {
                    resultTran.vpc_Result = "Giao dịch thành công";
                }
                else
                {
                    resultTran.vpc_Result = "Giao dịch không thành công";
                }
                hashvalidateResult = "CORRECTED";

            }
            resultTran.vpc_Version = version;
            resultTran.vpc_Amount = amount;
            resultTran.vpc_MerchantID = merchantID;
            resultTran.vpc_MerchantRef = merchTxnRef;
            resultTran.vpc_OrderInfor = orderInfo;
            resultTran.vpc_ResponseCode = txnResponseCode;
            resultTran.vpc_Command = command;
            resultTran.vpc_TracsactionNo = transactionNo;
            resultTran.hashvalidate = hashvalidateResult;
            resultTran.vpc_Message = message;
            return View(resultTran);
        }

    }
}
