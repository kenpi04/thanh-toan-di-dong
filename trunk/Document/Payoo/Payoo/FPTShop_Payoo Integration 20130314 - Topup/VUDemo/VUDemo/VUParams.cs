using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VUDemo
{
    public class GetServicesRequest
    {
        public string UserId;
        public string AgentId;
    }
    public class GetProvidersRequest
    {
        public string UserId;
        public string AgentId;
    }

    public class QueryBillExRequest
    {
        public string UserId;
        public string AgentId;
        public string CustomerId;
        public string ServiceId;
        public string ProviderId;
        public string Capcha;
        public string Area;
        public string HouseNumber;
    }
    public class Issuer
    {
        public string IssuerId;
        public string IssuerName;
        public bool IsOnline;
    }
    public class Service
    {
        public string ServiceId;
        public string ServiceName;
        public Issuer[] Issuers;
        public int MatchProviderCount;
    }
    public class BillInfo
    {
        public string BillId;
        public string ServiceId;
        public string ProviderId;
        public string Month;
        public decimal MoneyAmount;
        public decimal PaymentFee;
        public string CustomerName;
        public string Address;
        public string ExpiredDate;
        public string IsPrepaid;
        public string MonthAmount;
        public string PaymentRange;
        public string RenewalDate;
    }
    public class QueryBillExResult
    {
        public int ReturnCode;
        public byte[] Capcha;
        public BillInfo[] Bills;
        public Service[] Services;
        public string VietUnionId;
        public CustomerInfo[] CustomerInfos;
        public int MatchServiceCount;
        public int PaymentRule;
    }

    public class CustomerInfo
    {
        public string Area;
        public string CustomerId;
        public string CustomerName;
        public string HouseNumber;
    }
    
    public class BillInfoEx : BillInfo
    {
        public string ServiceName;
        public string ProviderName;
    }

    public class PayOnlineBillExRequest
    {
        public int AgentId;
        public string ApprovalCode;
        public string BankCode;
        public BillInfoEx[] Bills;
        public string CardNo;
        public string ContactAddress;
        public string ContactName;
        public string ContactPhone;
        public string CustomerId;
        public string InvoiceNo;
        public bool IsAutoPayment;
        public bool IsConfirm;
        public string OTP;
        public string ReferenceId;
        public string SystemTrace;
        public string SystemTraceEx;
        public string TransactionTime;
        public string UserId;
    }
    public class GetEwalletInfoBERequest
    {
        public string AgentId;
        public string EwalletId;
        public decimal MoneyAmount;
        public string UserId;

    }
    public class EwalletDepositBERequest : EwalletDepositRequest
    {
        public string AgentId;
        public string TransactionTime;
        public string UserId;
    }

    public class EwalletDepositRequest
    {
        public string ApprovalCode;
        public string BankCode;
        public string CardNo;
        public string Contact;
        public string DeviceId;
        public string InvoiceNo;
        public decimal MoneyAmount;
        public decimal PaymentFee;
        public int? PaymentMethod;
        public string ProcessingCode;
        public string ReferenceId;
        public string RequestTime;
        public string StaffId;
        public string SystemTrace;
        public string SystemTraceEx;
        public string Token;
        public string UserName;
        public string Version;
    }
}
