using PlanX.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlanX.Core.Domain.ClickBay;

namespace PlanX.Admin.Models.ClickBay
{
    public class BookingBaggageModel : BaseNopEntityModel
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

        public BookingInfoFlightModel BookingInfoFlightModel { get; set; }
    }
}