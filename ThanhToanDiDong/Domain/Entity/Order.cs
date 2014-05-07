//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order:BaseEntity
    {
        public Order()
        {
            this.OrderNotes = new HashSet<OrderNote>();
        }
    
        public int PartnerId { get; set; }
        public Nullable<System.Guid> OrderGuid { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderTypeId { get; set; }
        public decimal TotalAmount { get; set; }
        public int ProviderId { get; set; }
        public string NumberPhone { get; set; }
        public int CardMobileId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitSellingPrice { get; set; }
        public decimal Price { get; set; }
        public string OrderNote { get; set; }
        public bool Deleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CustomerIp { get; set; }
        public int PaymentStatusId { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public string AuthorizationTransactionId { get; set; }
        public string AuthorizationTransactionCode { get; set; }
        public string AuthorizationTransactionResult { get; set; }
        public int ResultCode { get; set; }
        public Nullable<int> ChannelCode { get; set; }
        public string ResultName { get; set; }
        public string Sessionid { get; set; }
        public string DataSign { get; set; }
        public string FunctionNameFinalCall { get; set; }
        public Nullable<int> FunctionFinalReturnCode { get; set; }
        public bool IsReceiptBill { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string TaxCode { get; set; }
        public string RecipientBillName { get; set; }
        public string RecipientBillPhone { get; set; }
        public string RecipientBillAddress { get; set; }
    
        public virtual ICollection<OrderNote> OrderNotes { get; set; }
    }
}