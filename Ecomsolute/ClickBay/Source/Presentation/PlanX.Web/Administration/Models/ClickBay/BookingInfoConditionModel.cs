using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.ClickBay
{
    public class BookingInfoConditionModel : BaseNopEntityModel
    {
        public int BookingInfoFlightId { get; set; }
        public string ConditionType { get; set; }
        public string ConditionDescription { get; set; }
        //public virtual BookingInfoFlightModel BookingInfoFlightModel { get; set; }
    }
}