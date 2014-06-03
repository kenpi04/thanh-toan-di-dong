using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework.Mvc;


namespace Nop.Web.Models.Catalog
{
    public partial class TopMenuModel : BaseNopModel
    {
        public TopMenuModel()
        {
            Categories = new List<CategorySimpleModel>();
            Districts = new List<SelectListItem>();
            PriceRange = new List<SelectListItem>();
            PriceRange.Add(new SelectListItem { Value = "0-0", Text = "Chọn mức giá" });
            PriceRange.Add(new SelectListItem { Value = "1000-1500", Text = "1 tỷ ~ 1.5 tỷ" });
            PriceRange.Add(new SelectListItem { Value = "1500-3000", Text = "1.5 tỷ ~ 3 tỷ" });
            PriceRange.Add(new SelectListItem { Value = "3000-0", Text = "Trên 3 tỷ" });
        }

        public IList<CategorySimpleModel> Categories { get; set; }
        public IList<SelectListItem> Districts { get; set; }
     

        public bool BlogEnabled { get; set; }
        public bool RecentlyAddedProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }
        public List<SelectListItem> PriceRange { get; set; }
    }
}