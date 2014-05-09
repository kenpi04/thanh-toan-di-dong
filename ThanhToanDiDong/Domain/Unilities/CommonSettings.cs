using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Unilities
{
  public  class CommonSettings
  {
      #region OnePay Info
      public static string vpc_Merchant = ConfigurationSettings.AppSettings["vpc_Merchant"];
      public static string vpc_AccessCode = ConfigurationSettings.AppSettings["vpc_AccessCode"];
      #endregion


      public static string ClientId = ConfigurationSettings.AppSettings["PayooClientId"];
      public static string ClientPassword = ConfigurationSettings.AppSettings["PayooClientPassword"];
      public static string PrivateKeyUrl = ConfigurationSettings.AppSettings["PayooPrivateKeyUrl"];
      public static string PasswordForPrivateKey = ConfigurationSettings.AppSettings["PayooPasswordForPrivateKey"];
      public static string PublicKeyUrl = ConfigurationSettings.AppSettings["PayooPublicKeyUrl"];
      public static String AgentId = ConfigurationSettings.AppSettings["PayooAgentId"];
      public static String UserId = ConfigurationSettings.AppSettings["PayooUserId"];
        public static string MMS_GetTopupValueList = "MMS_GetTopupValueList";
        public static string MMS_TopupPaymentBE = "MMS_TopupPaymentBE";
        public static string MMS_CodePaymentBE = "MMS_CodePaymentBE";
        public static string MMS_PaycodeInquiryBE = "MMS_PaycodeInquiryBE";
        public static string MMS_GetCardProviderList = "MMS_GetCardProviderList";
        public static string MMS_GetServices = "MMS_GetServices";
        public static string MMS_GetProviders = "MMS_GetProviders";
        public static string MMS_QueryBillEx = "MMS_QueryBillEx";
        public static string MMS_PayOnlineBillEx = "MMS_PayOnlineBillEx";
        public static string MMS_GetTransactionStatusBE = "MMS_GetTransactionStatusBE";
        public static string MMS_CodeGetCardListBE = "MMS_CodeGetCardListBE";
        #region Method

        #region TransactionMessage
        public static string GetTransactionStatusMessage(int ReturnCode)
        {
            switch (ReturnCode)
            {
                case -1:
                    return "Nghi vấn. Hệ thống chưa nhận được kết quả từ Payoo. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ.";
                case -2:
                    return "Giao dịch không tồn tại, vui lòng liên hệ với nhân viên để được hổ trợ";
                case 0:
                    return "Nghi vấn. Hệ thống chưa nhận được kết quả từ Payoo. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ.";
                case 1:
                    return "Giao dịch thành công";
                case 2:
                    return "Giao dịch đã hủy. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case 3:
                    return "Nghi vấn. Hệ thống chưa nhận được kết quả từ Payoo. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ.";
                case 4:
                    return "Nghi vấn. Hệ thống chưa nhận được kết quả từ Payoo. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ.";
                case 5:
                    return "Giao dịch Thất bại. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case -111:
                    return "Có lỗi trong quá trình xử lý";
                case -112:
                    return "Verify signature is not valid";
                default:
                    return null;
            }
        }
        #endregion

        #region GetTopupValueListMessage
        public static string GetTopupValueListMessage(int ReturnCode)
        {
            switch (ReturnCode)
            {
                case -1:
                    return "Lấy danh sách mệnh giá thất bại";
                case -3:
                    return "Lấy danh sách mệnh giá bị time out";
                case -5:
                    return "Số điện thoại hoặc mệnh giá không hổ trợ.";
                case 0:
                    return "Lấy danh sách thành công";
                case -111:
                    return "Có lỗi trong quá trình xử lý, vui lòng thử lại sau";
                case -112:
                    return "Có lỗi trong quá trình xử lý, vui lòng thử lại sau";
                default:
                    return null;
            }
        }
        #endregion



        #region TopupPaymentMessage
        public static string GetTopupPaymentMessage(int ReturnCode)
        {
            switch (ReturnCode)
            {
                case -1:
                    return "Nạp tiền thất bại. Vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case -3:
                    return "Giao dịch bị time out";
                case -5:
                    return "Mệnh giá không hổ trợ.";
                case -7:
                    return "Nạp tiền thất bại. Vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case 0:
                    return "Nạp Topup thành công.";
                case -111:
                    return "Có lỗi trong quá trình xử lý";
                case -112:
                    return "Verify signature is not valid";
                default:
                    return null;
            }
        }
        #endregion

        #region paycode
        public static string GetPaycodeInquiryMessage(int ReturnCode)
        {
            switch (ReturnCode)
            {
                case -1:
                    return "Lấy thông tin thất bại";
                case -3:
                    return "Lấy thông tin bị time out";
                case -4:
                    return "Vượt quá số lượng cho phép của hệ thống Payoo";
                case -5:
                    return "Mệnh giá không hổ trợ";
                case 0:
                    return "Lấy thông tin giá bán thành công.";
                case -111:
                    return "Có lỗi trong quá trình xử lý";
                case -112:
                    return "Verify signature is not valid";
                default:
                    return null;
            }
        }

        public static string GetCodePaymentMessage(int ReturnCode)
        {
            switch (ReturnCode)
            {
                case -1:
                    return "Giao dịch thất bại. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case -4:
                    return "Giao dịch thất bại. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case -5:
                    return "Giao dịch thất bại. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case -6:
                    return "Giao dịch thất bại. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case -7:
                    return "Giao dịch thất bại. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case -3:
                    return "Thanh toán mã thẻ bị time out";
                case 0:
                    return "Thanh toán mã thẻ thành công. Quý khách vui lòng kiểm tra mail để nhận mã thẻ. Cảm ơn quý khách";
                case -111:
                    return "Có lỗi trong quá trình xử lý";
                case -112:
                    return "Verify signature is not valid";
                default:
                    return null;
            }
        }

        public static string GetCardListMessage(int ReturnCode)
        {
            switch (ReturnCode)
            {
                case -1:
                    return "Chưa nhận được kết quả cuối cùng từ Payoo. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ";
                case -2:
                    return "Giao dịch thất bại. Tạm giữ tiền khách hàng, vui lòng liên hệ nhân viên để được hổ trợ hoặc nhận lại tiền";
                case 0:
                    return "Lấy mã thẻ thành công.";
                case -111:
                    return "Có lỗi trong quá trình xử lý";
                case -112:
                    return "Verify signature is not valid";
                default:
                    return null;
            }
        }
        #endregion

        #region Payment
        public static string GetQueryBillExMessage(int ReturnCode, string DescriptionCode = null)
        {
            switch (ReturnCode)
            {
                case 0:
                    return "Query bill thành công.";
                case 2:
                    return "Query bill không thành công do mã khách hàng nhập vào không tồn tại hoặc không nợ cước.";
                case 3:
                    return "Query không được bill do khách hàng không nợ cước.";
                case 4:
                    return "Query không được bill do số điện thoại không tồn tại.";
                case 5:
                    return "Tìm được nhiều nhà cung cấp-dịch vụ.";
                case 7:
                    return "khách hàng hoặc đại lý đã truy vấn > n lần trong khoảng thời gian x(s) mà hệ thống quy định.";
                case -1:
                    return "Query bill thất bại.";
                case -3:
                    return "Query bill bị timeout.";
                case -9:
                    return DescriptionCode;
                case -10:
                    return "Với dịch vụ của nhà cung cấp mà khách hàng chọn bị hạn chế cho đại lý này.";
                case -14:
                    return "Hệ thống đang offline, vui lòng thử lại sau.";
                case -16:
                    return "Có lổi trong quá trình tìm kiếm Bill, vui lòng thử lại sau.";
                case -18:
                    return "Sai Capcha.";
                case -23:
                    return "Cần thêm capcha.";
                case -24:
                    return "Không tìm thấy có thông tin khách hàng.";
                case -25:
                    return null;
                case -26:
                    return "Nhà cung cấp và dịch vụ không hợp lệ";
                case -111:
                    return "Có lỗi trong quá trình xử lý";
                case -112:
                    return "Verify signature is not valid";
                default:
                    return null;
            }
        }

        public static string GetPayOnlineBillExMessage(int ReturnCode)
        {
            switch (ReturnCode)
            {
                case 0:
                    return "Thanh toán thành công và đã gạch nợ cho khách hàng.";
                case 1:
                    return "Thanh toán thành công.";
                case -1:
                case -7:
                case -8:
                case -17:
                case -19:
                    return "Thanh toán hóa đơn bị thất bại. Vui lòng liên hệ nhân viên để được hổ trợ";
                case -111:
                    return "Có lỗi trong quá trình xử lý";
                case -112:
                    return "Verify signature is not valid";
                default:
                    return null;
            }
        }


        #endregion

        #endregion

    }
}
