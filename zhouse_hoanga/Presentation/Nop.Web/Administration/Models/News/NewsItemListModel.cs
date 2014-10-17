using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.News
{
    public partial class NewsItemListModel : BaseNopModel
    {
        public NewsItemListModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsItems.List.SearchStore")]
        public int SearchStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.List.SearchNewsTitle")]
        public string SearchTileName { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.List.SearchTile")]
        public int SearchCategoryName { get; set; }

    }
}