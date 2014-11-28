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
	}
        public IList<SelectListItem> ListCitys { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }

        public string DepartDate  { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Flant { get; set; }
        public bool Return { get; set; }

    }
}