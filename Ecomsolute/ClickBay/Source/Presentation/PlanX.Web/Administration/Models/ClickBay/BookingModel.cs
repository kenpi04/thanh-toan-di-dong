﻿using PlanX.Web.Framework.Mvc;
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
        }
        public string ContactName { get; set; }
        public string ContactGender { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public int ContactCountryId { get; set; }
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
        public string ReasonCancel { get; set; }
        public short ContactStatusId { get; set; }
        public int CustomerId { get; set; }
        public Nullable<System.DateTime> CustomerReceivedDate { get; set; }
        public string CustomerNote { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public short ContactPassengerType { get; set; }

        public virtual PasserType PasserType {
            get { return (PasserType)ContactPassengerType; }
            set { this.ContactPassengerType = (short)value; }
        }

        public virtual BookingStatus BookingStatus
        {
            get { return (BookingStatus)BookingStatusId; }
            set { this.BookingStatusId = (short)value; }
        }
        public virtual PaymentStatus PaymentStatus
        {
            get { return (PaymentStatus)PaymentStatusId; }
            set { this.PaymentStatusId = (short)value; }
        }
        public virtual ContactStatus ContactStatus
        {
            get { return (ContactStatus)ContactStatusId; }
            set { this.ContactStatusId = (short)value; }
        }
    
        public virtual BookingInfoFlightModel BookingInfoFlightModel { get; set; }
        public virtual BookingInfoFlightModel BookingInfoFlightReturnModel { get; set; }
        public virtual List<BookTicketNoteModel> BookTicketNotesModel { get; set; }
    }
}