//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlanX.Core.Domain.ClickBay
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookingInfoFlight:BaseEntity
    {
        public BookingInfoFlight()
        {
            this.Bookings = new HashSet<Booking>();
            this.ReturnBookings = new HashSet<Booking>();
            this.BookingPriceDetails = new HashSet<BookingPriceDetail>();
            this.BookingInfoConditions = new HashSet<BookingInfoCondition>();
        }
    
       
        public string Brand { get; set; }
        public int AirlinesId { get; set; }
        public short Adult { get; set; }
        public short Child { get; set; }
        public short Infant { get; set; }
        public string FlightNumber { get; set; }
        public string PRNCode { get; set; }
        public Nullable<System.DateTime> DepartDateTime { get; set; }
        public Nullable<System.DateTime> ArrivalDateTime { get; set; }
        public string TicketType { get; set; }
        public int FromPlaceId { get; set; }
        public int ToPlaceId { get; set; }
        public string FromPlaceCode { get; set; }
        public string ToPlaceCode { get; set; }
        public string FromPlaceName { get; set; }
        public string ToPlaceName { get; set; }
        public string FareBasis { get; set; }
        public int IdBooking { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalBaggageFee { get; set; }
        public decimal TotalFeeOther { get; set; }
        public decimal DiscountAmount { get; set; }
    
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Booking> ReturnBookings { get; set; }
        public virtual ICollection<BookingPriceDetail> BookingPriceDetails { get; set; }

        public virtual ICollection<BookingInfoCondition> BookingInfoConditions { get; set; }
    }
}
