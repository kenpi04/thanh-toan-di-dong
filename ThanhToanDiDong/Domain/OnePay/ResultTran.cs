
/// <summary>
/// Module chứa các thông tin trả về sau giao dịch.
/// 
/// Revision History
/// Date			Author		                Reason for Change
/// -----------------------------------------------------------
/// 20/09/2012		Thuyhk                      Create.
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.OnePay
{
    public class ResultTran
    {
        public string vpc_MerchantRef { get; set; }
        public string vpc_MerchantID { get; set; }
        public string vpc_ResponseCode { get; set; }
        public string vpc_Result { get; set; }
        public string vpc_Amount { get; set; }
        public string vpc_Locale { get; set; }
        public string vpc_Command { get; set; }
        public string vpc_Version { get; set; }
        public string vpc_Card { get; set; }
        public string vpc_OrderInfor { get; set; }
        public string vpc_Merchant { get; set; }
        public string vpc_AuthorizeId { get; set; }
        public string vpc_MerchTxnRef { get; set; }
        public string vpc_TracsactionNo { get; set; }
        public string vpc_Message { get; set; }

        public string hashvalidate { get; set; }
    }
}