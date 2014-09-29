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
            Districts2 = new List<SelectListItem>();
            PriceRange = Nop.Web.Framework.Extensions.GetPrice(false);
            PriceRangeRent = Nop.Web.Framework.Extensions.GetPrice(true);
            CategoriesNews = new List<SelectListItem>();
            Topics = new List<SelectListItem>();
        }

        public IList<CategorySimpleModel> Categories { get; set; }
        public IList<SelectListItem> Districts { get; set; }
        public IList<SelectListItem> Districts2 { get; set; }     

        public bool BlogEnabled { get; set; }
        public bool RecentlyAddedProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }
        public List<SelectListItem> PriceRange { get; set; }
        public List<SelectListItem> PriceRangeRent { get; set; }
        public List<SelectListItem> CategoriesNews { get; set; }
        public List<SelectListItem> Topics { get; set; }
    }    
}