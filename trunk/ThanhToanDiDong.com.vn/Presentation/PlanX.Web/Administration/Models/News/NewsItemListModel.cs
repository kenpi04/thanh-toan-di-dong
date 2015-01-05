using System.Collections.Generic;
using System.Web.Mvc;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.News
{
    public partial class NewsItemListModel : BaseNopModel
    {
        public NewsItemListModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsItems.List.SearchStore")]
        public int SearchStoreId { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.News.NewsItems.List.IsHotView")]
        public bool IsHotView { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.News.NewsItems.List.SearchCategory")]
        public int SearchCategoryId { get; set; }
        public IList<SelectListItem> AvailableCategory { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.News.NewsItems.List.IsShowSlider")]
        public bool IsShowSlider { get; set; }
        
    }
}