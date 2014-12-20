using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanX.Web.Models.ClickBay
{
    public class SearchModel
    {
        public SearchModel ()
	{
            ListCitys=new List<SelectListItem>();
            Tickets = new List<TicketModel>();
	}
        public IList<SelectListItem> ListCitys { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }

        public string FromName { get; set; }

        public string ToName { get; set; }
       
        public string DepartDate { get; set; }

        
        public string ReturnDate { get; set; }

        public string DateFormat { get; set; }
        
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Flant { get; set; }
        public bool Return { get; set; }
        public IList<TicketModel> Tickets { get; set; }

        public string Source { get; set; }

        public string SessionId { get; set; }
        public int Sort { get; set; }
        public DateTime SearchDate { get; set; }

        public bool Compare(SearchModel model)
        {
            return this.Adult == model.Adult && this.Child == model.Child
                && this.Flant == model.Flant && this.FromId == model.FromId
                && this.DepartDate == model.DepartDate&&this.ReturnDate==model.ReturnDate;
        }
    }
    public enum Sort
    {
        Price=1,
        Date=2,
        Brand=3
    }
}