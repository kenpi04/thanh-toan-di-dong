using System;

namespace PlanX.Admin.Models.ClickBay
{
    public class BookTicketNoteModel
    {
        public Nullable<int> BookTicketId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }

        //public virtual BookingModel BookingModel { get; set; }
    }
}