
namespace PlanX.Core.Domain.ClickBay
{
    public partial class BookingInfoFlightDetail:BaseEntity
    {
        public int BookingInfoFlightId { get; set; }
        public string Airline { get; set; }
        public string AirlineCode { get; set; }
        public string FlightDuration { get; set; }
        public string FlightNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string DepartTime { get; set; }
        public string LandingTime { get; set; }
        public string FromPlace { get; set; }
        public string ToPlace { get; set; }
        public virtual BookingInfoFlight BookingInfoFlight { get; set; }
    }
}
