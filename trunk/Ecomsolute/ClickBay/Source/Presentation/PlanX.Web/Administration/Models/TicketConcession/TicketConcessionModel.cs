using PlanX.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PlanX.Admin.Validators.TicketConcession;
using PlanX.Web.Framework;
using FluentValidation.Attributes;

namespace PlanX.Admin.Models.TicketConcession
{
   [Validator(typeof(TicketConcessionValidator))]
    public class TicketConcessionModel : BaseNopEntityModel
    {
       public TicketConcessionModel()
        {
            DepartDate = new DateTime();
            CreatedOnUtc = new DateTime();
            ReturnDate = new DateTime();
        }
        
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
    public class TicketConcessionListModel
    {

        public TicketConcessionListModel()
        {

            listItem = new List<TicketConcessionModel>();
            listPlace = new List<string>();
            listType = new List<string>();
            DepartDateSearch = new DateTime(); 

        }
        public List<TicketConcessionModel> listItem { get; set; }

        public List<string> listPlace { get; set; }

        public List<string> listType { get; set; }

        public string PassengerNameSearch { get; set; }

        public string FromPlaceSearch { get; set; }

        public string ToPlaceSearch { get; set; }

        public string TicketTypeSearch { get; set; }
          
        [UIHint("DateNullable")]
        public DateTime DepartDateSearch { get; set; }
    }
}