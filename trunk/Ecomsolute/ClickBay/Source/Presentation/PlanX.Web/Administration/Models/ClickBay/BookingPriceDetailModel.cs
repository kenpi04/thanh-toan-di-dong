using PlanX.Web.Framework.Mvc;


namespace PlanX.Admin.Models.ClickBay
{
    public class BookingPriceDetailModel : BaseNopEntityModel
    {
        public int BookingInfoFlightId { get; set; }
        public short PassengerType { get; set; }
        public string PassengerTypeName { get; set; }
        public short Quantity { get; set; }
        public string TicketType { get; set; }
        public string CodeFee { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }

        //public virtual BookingInfoFlightModel BookingInfoFlightModel { get; set; }
    }
}