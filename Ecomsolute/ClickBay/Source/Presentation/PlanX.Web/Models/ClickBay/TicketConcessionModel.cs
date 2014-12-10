using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanX.Web.Models.ClickBay
{
    public class TicketConcessionModel
    {
        public TicketConcessionModel()
        {
            DepartDate = new DateTime();
            CreatedOnUtc = new DateTime();
            ReturnDate = new DateTime();
        }

        public bool IsRename { get; set; }
        public string TicketType { get; set; }

        public bool RoundTrip { get; set; }

        public string PassengerName { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public DateTime DepartDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string TicketPrice { get; set; }

        public bool IsHelper { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public string ContactName { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public bool Deleted { get; set; }

        public int Id { get; set; }
    }
    public class TicketConcessionListModel
     {     

         public TicketConcessionListModel()
        {

            listItem = new List<TicketConcessionModel>();
            listPlace = new List<string>();
            listType = new List<string>();
          
      
        }
        public List<TicketConcessionModel> listItem { get; set; }

        public List<string> listPlace { get; set; }

        public List<string> listType { get; set; }

        public string PassengerNameSearch { get; set; }

        public string FromPlaceSearch { get; set; }

        public string ToPlaceSearch { get; set; }

        public string TicketTypeSearch { get; set; }

        public string DepartDateSearch { get; set; }

        public int Total { get; set; }

        public int PageSize { get; set; }

        public int Page { get; set; }
     }
}