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
        }

        public IList<CategorySimpleModel> Categories { get; set; }
        public IList<SelectListItem> Districts { get; set; }

        public bool BlogEnabled { get; set; }
        public bool RecentlyAddedProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }
    }
}