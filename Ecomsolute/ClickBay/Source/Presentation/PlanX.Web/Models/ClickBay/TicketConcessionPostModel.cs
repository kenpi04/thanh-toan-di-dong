using FluentValidation.Attributes;
using PlanX.Web.Validators.ClickBay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanX.Web.Models.ClickBay
{
   // [Validator(typeof(TicketConcessionPostValidator))]
    public class TicketConcessionPostModel
    {
        public TicketConcessionPostModel()
        {

            CreatedOnUtc = new DateTime();
            listPlace = new List<string>();
            listType = new List<string>();
      
        }

        public List<string> listPlace { get; set; }

        public List<string> listType { get; set; }
        public string TicketType { get; set; }

        public bool RoundTrip { get; set; }

        public string PassengerName { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public string DepartDate { get; set; }

        public string TimeDepartDate { get; set; }

        public string ReturnDate { get; set; }

        public string TimeReturnDate { get; set; }

        public decimal TicketPrice { get; set; }

        public string CurrencyCode { get; set; }

        public bool IsHelper { get; set; }

        public bool IsChangeName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public string ContactName { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public bool Deleted { get; set; }

        public bool IsRename { get; set; }
    }
}