using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Directory
{
    public partial class DistrictListModel : BaseNopEntityModel
    {
        public DistrictListModel()
        {
            //Countries = new List<SelectListItem>();
            StateProvinces = new List<SelectListItem>();
        }

        //[NopResourceDisplayName("Admin.ContentManagement.Topics.List.Country")]
        //public int CountryId { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.Topics.List.StateProvince")]
        public int StateProvinceId { get; set; }
        //public IList<SelectListItem> Countries { get; set; }

        public IList<SelectListItem> StateProvinces { get; set; }
    }
}