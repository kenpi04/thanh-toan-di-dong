using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanX.Web.Models.ClickBay
{
    public class BookingModel
    {
      
        public string ContactName { get; set; }
        public string ContactGender { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public int ContactCountryId { get; set; }
        public string ContactCityName { get; set; }
        public DateTime? ContactBirthDate { get; set; }
        public string ContactRequestMore { get; set; }
        public string VoucherCode { get; set; }
        public bool RoundTrip { get; set; }
        public int BookingInfoFlightToId { get; set; }
        public int BookingInfoFlightReturnId { get; set; }
        public short Adult { get; set; }
        public short Child { get; set; }
        public short Infant { get; set; }
        public string CurrencyType { get; set; }
        public decimal CurrentcyRate { get; set; }
        public string CallBackUrl { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalFeeAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalFeeOtherAmount { get; set; }
        public decimal TotalBaggageFeeAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public short BookingStatusId { get; set; }
        public short PaymentStatusId { get; set; }
        public short PaymentMethodId { get; set; }
        public short ContactStatusId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CustomerReceivedDate { get; set; }
        public string CustomerNote { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }

        public TicketModel TicketInfo { get; set; }
       
        public class BookingPasserModel
        {
            public int BookingId { get; set; }
            public short PassengerType { get; set; }
            public string Gender { get; set; }
            public string Title { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string MobileNumber { get; set; }
            public int Baggage { get; set; }
            public int ReturnBaggage { get; set; }
            public DateTime? BirthDay { get; set; }
            public string Email { get; set; }
            public string PassportNumber { get; set; }
            public DateTime? PassportExpired { get; set; }
            public decimal BaggageFee { get; set; }
            public decimal ReturnBaggageFee { get; set; }
        }
        
       
       
    }
}