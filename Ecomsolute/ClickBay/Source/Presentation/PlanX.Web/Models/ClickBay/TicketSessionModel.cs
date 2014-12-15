using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanX.Web.Models.ClickBay
{
    public class TicketSessionModel
    {
    
        public TicketModel Ticket { get; set; }
        public TicketModel TicketReturn { get; set; }
    }
}