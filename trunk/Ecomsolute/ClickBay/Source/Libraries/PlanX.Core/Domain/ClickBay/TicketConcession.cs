using System;
using System.Collections.Generic;

namespace PlanX.Core.Domain.ClickBay
{
    public class TicketConcession : BaseEntity
    {

        public string TicketType { get; set; }

        public bool RoundTrip { get; set; }

        public string PassengerName { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public DateTime DepartDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal TicketPrice { get; set; }

        public string CurrencyCode { get; set; }

        public bool IsHelper { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public string ContactName { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public bool Deleted { get; set; }





        public bool IsRename { get; set; }
    }
}
