using PlanX.Web.Framework;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PlanX.Admin.Models.ClickBay
{
    public class SearchConditionsModel
    {
        public SearchConditionsModel()
        {
            Airlines = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.ClickBay.SearchConditions.Airlines")]
        public List<SelectListItem> Airlines { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.SearchConditions.AirlineId")]
        public int AirlineId { get; set; }
    }
}