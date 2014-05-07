
/// <summary>
/// Model chứa các thông tin yêu cầu trong giao dịch.
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
    public class Transaction
    {
        public string Title { get; set; }
        public string vpc_Locale { get; set; }
        public string vpc_Version { get; set; }
        public string vpc_Command { get; set; }
        public string vpc_Merchant { get; set; }
        public string vpc_AccessCode { get; set; }
        public string vpc_MerchTxnRef { get; set; }
        public string vpc_OrderInfo { get; set; }
        public string vpc_Amount { get; set; }
        public string vpc_Currency { get; set; }
        public string vpc_ReturnURL { get; set; }
    }
}