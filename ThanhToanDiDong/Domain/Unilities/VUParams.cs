using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Unilities
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
        public string ProviderName;
        public string ServiceName;
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
        public string AgentId;
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
    public class PayOnlineBillExResult
    {
        public int ReturnCode { get; set; }
        public string OrderNo { get; set; }
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
    #region Get Card Provider List

    public class GetCardProviderListResult
    {
        public int ReturnCode;
        public CardProviderInfo[] ProviderList;
        public int MatchServiceCount;
        public int PaymentRule;
    }

    public class CardProviderInfo
    {
        public CardValuesList[] CardValues;
        public string ProviderCode;
        public string ProviderName;
    }

    public class CardValuesList
    {
        public Decimal CardValue;
    }

    #endregion

    #region Query paycode price

    public class PaycodeInquiryBERequest
    {
        public PaycodeInquiryBERequest()
        {
            AgentId = CommonSettings.AgentId;
            UserId = CommonSettings.UserId;
        }

        public string AgentId { get; set; }
        public string ProviderId { get; set; }
        public decimal CardValue { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }

    }

    public class PaycodeInquiryBEResult
    {
        public int ReturnCode;
        public decimal PurchasingPrice;
        public decimal ReferPrice;
        public int? MaxQuantity;

    }

    #endregion

    #region codePayment

    public class CodeGetCardListBERequest
    {
        public CodeGetCardListBERequest()
        {
            AgentId = CommonSettings.AgentId;

        }
        public string AgentId { get; set; }
        public string SystemTrace { get; set; }

    }

    public class CodeGetCardListBEResult
    {
        public int ReturnCode { get; set; }
        public PayCodeInfo[] PayCodes { get; set; }
    }

    public class CodePaymentBERequest
    {
        public CodePaymentBERequest()
        {
            AgentId = CommonSettings.AgentId;
            UserId = CommonSettings.UserId;
        }
        public string AgentId { get; set; }
        public string ProviderId { get; set; }
        public decimal CardValue { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemTrace { get; set; }
        public decimal TotalPurchasingAmount { get; set; }
        public decimal TotalReferAmount { get; set; }
        public string ApprovalCode { get; set; }
        public string ReferenceId { get; set; }
        public string CardNo { get; set; }
        public string SystemTraceEx { get; set; }
        public string TransactionTime { get; set; }
    }
    public class CodePaymentBEResult
    {
        public int ReturnCode { get; set; }
        public string Barcode { get; set; }
        public string DescriptionCode { get; set; }
        public int? InventoryQuantity { get; set; }
        public string SystemTrace { get; set; }
        public PayCodeInfo[] PayCodes { get; set; }
    }

    public class PayCodeInfo
    {
        public string CardId { get; set; }
        public string Expired { get; set; }
        public string SeriNumber { get; set; }
        public string TypeCard { get; set; }
    }




    #endregion

    #region Get Topup Value List

    public class GetTopupValueListRequest
    {
        public GetTopupValueListRequest()
        {
            AgentId = CommonSettings.AgentId;
            UserId = CommonSettings.UserId;
        }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public string PhoneNo { get; set; }
    }

    public class GetTopupValueListResult
    {
        public int ReturnCode { get; set; }
        public TopupValueInfo[] TopupValueList;
    }

    public class TopupValueInfo
    {
        public Decimal CardValue { get; set; }
        public string CardValueCode { get; set; }
        public string CardValueName { get; set; }
        public Decimal PurchasingPrice { get; set; }
        public Decimal ReferPrice { get; set; }
    }

    #endregion

    #region topupPayment

    public class TopupPaymentBERequest
    {
        public TopupPaymentBERequest()
        {
            AgentId = CommonSettings.AgentId;
            UserId = CommonSettings.UserId;
        }

        public string AgentId { get; set; }
        public string UserId { get; set; }
        public decimal CardValue { get; set; }
        public string InvoiceNo { get; set; }
        public string SystemTrace { get; set; }
        public decimal TotalPurchasingAmount { get; set; }
        public decimal TotalReferAmount { get; set; }
        public string ApprovalCode { get; set; }
        public string ReferenceId { get; set; }
        public string SystemTraceEx { get; set; }
        public string TransactionTime { get; set; }
        public string PrimaryAccount { get; set; }
    }

    public class TopupPaymentBEResult
    {
        public int ReturnCode { get; set; }
    }

    #endregion

    #region Transaction

    public class GetTransactionStatusBERequest
    {
        public GetTransactionStatusBERequest()
        {
            AgentId = CommonSettings.AgentId;
        }
        public string AgentId { get; set; }
        public string InvoiceNo { get; set; }
        public string RequestTime { get; set; }
        public string VietUnionId { get; set; }
    }

    public class GetTransactionStatusBEResult
    {
        public int ReturnCode { get; set; }
        public int Status { get; set; }
    }

    public class GetProvidersResult
    {
        public int ReturnCode;
        public Provider[] Providers;
    }
    public class Provider
    {
        public string ProviderId;
        public string ProviderName;
        public string ServiceId;
    }

    #endregion
  
}
