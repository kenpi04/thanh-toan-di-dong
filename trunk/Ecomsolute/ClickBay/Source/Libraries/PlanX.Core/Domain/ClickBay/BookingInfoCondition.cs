using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanX.Core.Domain.ClickBay
{
  public partial  class BookingInfoCondition:BaseEntity
    {
        public int BookingInfoFlightId { get; set; }
        public string ConditionType { get; set; }
        public string ConditionDescription { get; set; }
        public virtual  BookingInfoFlight BookingInfoFlight { get; set; }
    }
}
