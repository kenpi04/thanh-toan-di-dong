
using PlanX.Core;
using PlanX.Core.Domain.ClickBay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PlanX.Services.ClickBay
{
   public class OnePayServices:IOnePayServices
    {
       private readonly IClickBayService _clickBayService;
       private readonly OnePAYPaymentSettings _onePAYPaymentSettings;
       private readonly IWebHelper _webHelper;
       private readonly HttpContextBase _httpContext;
       public OnePayServices(IClickBayService clickBayService,
           OnePAYPaymentSettings onePAYPaymentSettings,
           IWebHelper webHelper, HttpContextBase httpContext
           
           )
       {
           _httpContext = httpContext;
           _onePAYPaymentSettings = onePAYPaymentSettings;
           _clickBayService = clickBayService;
           _webHelper = webHelper;

       }
       private string GetOnePAYUrl()
       {
           return _onePAYPaymentSettings.UseSandbox ? "http://mtf.onepay.vn/vpcpay/vpcpay.op" :
               "https://onepay.vn/vpcpay/vpcpay.op";
       }
        public void PostProcessPayment(BookTicket ticket)
        {
            var model = _onePAYPaymentSettings;
            string merchantID = model.MerchantID;
            string accessCode = model.AccessCode;
            string merchTxnRef = ticket.TicketGuid.ToString();

            VPCRequest conn = new VPCRequest(GetOnePAYUrl());
            string total = ticket.TicketPrice.ToString();
            string IdOrder = ticket.Id.ToString();
            string IdCustumer = ticket.CustomerId.ToString();
            string IpCustumer = ticket.Ip;
            string returnUrl = _webHelper.GetStoreLocation(false) + "/onepayreturn";

            conn.SetSecureSecret("18D7EC3F36DF842B42E1AA729E4AB010");

            //Thông tin về các trường trong giao dịch
            conn.AddDigitalOrderField("AgainLink", "http://onepay.vn");
            conn.AddDigitalOrderField("Title", "onepay paygate");
            conn.AddDigitalOrderField("vpc_Locale", "vn"); //Chọn ngôn ngữ hiển thị khi thanh toán(vn/en)
            conn.AddDigitalOrderField("vpc_Version", "2");//GetOnePayVersion()); //version Onepay
            conn.AddDigitalOrderField("vpc_Command", "pay"); // Gán giá trị Type_Comment mặc định là pay
            conn.AddDigitalOrderField("vpc_Merchant", merchantID);
            conn.AddDigitalOrderField("vpc_AccessCode", accessCode);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", merchTxnRef); // Mã giao dịch, lập trình viên lập trình ra mã giao dịch (viết hàm tự tăng hoặc ghi theo thời gian)
            conn.AddDigitalOrderField("vpc_OrderInfo", IdOrder);
            conn.AddDigitalOrderField("vpc_Amount", total); // Tổng giá tiền giao dịch            
            conn.AddDigitalOrderField("vpc_ReturnURL", returnUrl); // Link trả về sau khi thanh toán

            //chuyển hướng sang cổng thanh toán
            String url = conn.Create3PartyQueryString();
            _httpContext.Response.Redirect(url);    
        }

        public bool GetReturnPaymentSuccess(FormCollection form)
        {
            var model = _onePAYPaymentSettings;
            string hashValidateResult = "";
            VPCRequest conn = new VPCRequest("http://onepay.vn");
            conn.SetSecureSecret(model.HashCode);
            hashValidateResult = conn.Process3PartyResponse(_httpContext.Request.QueryString);

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
            string additionalFee = model.AdditionalFee.ToString();
            
            var ticketOrder = _clickBayService.GetBookTicketById(Convert.ToInt32(orderInfo)); // GetOrderById(IdOrder);
            if (ticketOrder != null)
            {
                var sb = new StringBuilder();
                sb.AppendLine("OnePay Returl:");
                sb.AppendLine("vpc_TxnResponseCode: " + txnResponseCode);
                sb.AppendLine("vpc_Amount: " + amount);
                sb.AppendLine("vpc_Locale: " + localed);
                sb.AppendLine("vpc_Command: " + command);
                sb.AppendLine("vpc_Version: " + version);
                sb.AppendLine("vpc_Card: " + cardBin);
                sb.AppendLine("vpc_OrderInfo: " + order);
                sb.AppendLine("vpc_Merchant: " + merchantID);
                sb.AppendLine("vpc_AuthorizeId: " + authorizeID);
                sb.AppendLine("vpc_MerchTxnRef: " + merchTxnRef);
                sb.AppendLine("vpc_Message: " + message);
                sb.AppendLine("payment_fee: " + additionalFee);

                //order note
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = sb.ToString(),
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(order);

                //validate order total
                if (!Math.Round(Convert.ToDecimal(amount), 2).Equals(Math.Round((order.OrderTotal * 100), 2)))
                {
                    string errorStr = string.Format("OnePay Result. Returned order total {0} doesn't equal order total {1}", amount, order.OrderTotal);
                    _logger.Error(errorStr);
                    //Response.Write(@"<script language='javascript'>alert('Quá trình giao dịch xảy ra lỗi./n Xin vui lòng liên hệ bộ phận hỗ trợ khách hàng để được biết thêm chi tiết.')</script>");

                    return RedirectToAction("CheckoutError", "Checkout", new { area = "" });
                }

                //mark order as paid
                if (_orderProcessingService.CanMarkOrderAsPaid(order))
                {
                    order.AuthorizationTransactionId = authorizeID;
                    _orderService.UpdateOrder(order);

                    _orderProcessingService.MarkOrderAsPaid(order);
                }
            }
        }
    }
}
