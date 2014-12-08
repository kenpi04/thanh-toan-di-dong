using System.Collections.Generic;

namespace PlanX.Core.Domain.ClickBay
{
    public class BookingBaggage:BaseEntity
    {
        public int BookingInfoFlightId { get; set; }
        public short PassengerType { get; set; }
        public int Baggage { get; set; }
        public decimal BaggageFee { get; set; }
        public string Description { get; set; }
        public virtual PassengerType PassengerTypeEnum 
        {
            get { return (PassengerType)this.PassengerType; }
            set { this.PassengerType = (short)value; }
        }

        public virtual BookingInfoFlight BookingInfoFlight;
    }
}
