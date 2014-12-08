using PlanX.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace PlanX.Admin.Models.ClickBay
{
    public class BookingInfoFlightModel:BaseNopEntityModel
    {
         public BookingInfoFlightModel()
        {
            this.BookingsModel = new List<BookingModel>();
            this.ReturnBookingsModel = new List<BookingModel>();
            this.BookingPriceDetailsModel = new List<BookingPriceDetailModel>();
            this.BookingInfoConditionsModel = new List<BookingInfoConditionModel>();
            this.BookingBaggagesModel = new List<BookingBaggageModel>();
        }
    
       
        public string Brand { get; set; }
        public string BrandName { get; set; }
        public int AirlinesId { get; set; }
        public short Adult { get; set; }
        public short Child { get; set; }
        public short Infant { get; set; }
        public string FlightNumber { get; set; }
        public string PRNCode { get; set; }
        public Nullable<System.DateTime> DepartDateTime { get; set; }
        public Nullable<System.DateTime> ArrivalDateTime { get; set; }
        public int FlightDuration { get; set; }
        public string TicketType { get; set; }
        public int FromPlaceId { get; set; }
        public int ToPlaceId { get; set; }
        public string FromPlaceCode { get; set; }
        public string ToPlaceCode { get; set; }
        public string FromPlaceName { get; set; }
        public string ToPlaceName { get; set; }
        public short Stops { get; set; }
        public string FareBasis { get; set; }
        public int IdBooking { get; set; }
        public decimal TotalPriceNet { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalBaggageFee { get; set; }
        public decimal TotalFeeOther { get; set; }
        public decimal DiscountAmount { get; set; }
    
        public List<BookingModel> BookingsModel { get; set; }
        public List<BookingModel> ReturnBookingsModel { get; set; }
        public List<BookingPriceDetailModel> BookingPriceDetailsModel { get; set; }
        public List<BookingInfoConditionModel> BookingInfoConditionsModel { get; set; }
        public List<BookingBaggageModel> BookingBaggagesModel { get; set; }
    }
}