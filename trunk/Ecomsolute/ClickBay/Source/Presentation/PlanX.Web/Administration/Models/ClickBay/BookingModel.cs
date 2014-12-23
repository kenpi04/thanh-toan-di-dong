using PlanX.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using PlanX.Core.Domain.ClickBay;

namespace PlanX.Admin.Models.ClickBay
{
    public class BookingModel:BaseNopEntityModel
    {
        public BookingModel()
        {
            this.BookTicketNotesModel = new List<BookTicketNoteModel>();
            this.BookingInfoFlightModel = new BookingInfoFlightModel();
            this.BookingInfoFlightReturnModel = new BookingInfoFlightModel();
            this.BookingPassengerModel = new List<BookingPassengerModel>();
        }
        public string ContactName { get; set; }
        public string ContactGender { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public int ContactCountryId { get; set; }
        public string ContactCountryName { get; set; }
        public string ContactCityName { get; set; }
        public Nullable<System.DateTime> ContactBirthDate { get; set; }
        public string ContactRequestMore { get; set; }
        public string VoucherCode { get; set; }
        public bool RoundTrip { get; set; }
        public Nullable<int> BookingInfoFlightToId { get; set; }
        public Nullable<int> BookingInfoFlightReturnId { get; set; }
        public short Adult { get; set; }
        public short Child { get; set; }
        public short Infant { get; set; }
        public string FromPlace { get; set; }
        public string ToPlace { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
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
        public string PaymentMethod { get; set; }
        public string ReasonCancel { get; set; }
        public short ContactStatusId { get; set; }
        public int CustomerId { get; set; }
        public Nullable<System.DateTime> CustomerReceivedDate { get; set; }
        public string CustomerNote { get; set; }
        public string CustomerName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public short ContactPassengerType { get; set; }

        public string PasserType { get; set; }

        public string BookingStatus { get; set; }
        
        public string PaymentStatus{get;set;}
        public string ContactStatus{get;set;}

        public string TicketId { get; set; }
        public BookingInfoFlightModel BookingInfoFlightModel { get; set; }
        public BookingInfoFlightModel BookingInfoFlightReturnModel { get; set; }
        public List<BookTicketNoteModel> BookTicketNotesModel { get; set; }
        public List<BookingPassengerModel> BookingPassengerModel { get; set; }
    }
}