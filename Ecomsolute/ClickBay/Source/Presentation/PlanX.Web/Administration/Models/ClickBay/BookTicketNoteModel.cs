using System;

namespace PlanX.Admin.Models.ClickBay
{
    public class BookTicketNoteModel : PlanX.Web.Framework.Mvc.BaseNopEntityModel
    {
        public int BookTicketId { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }

        //public virtual BookingModel BookingModel { get; set; }
    }
}