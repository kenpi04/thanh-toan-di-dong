using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Web.Models.Common
{
    public partial class FooterModel : BaseNopModel
    {
        public FooterModel()
        {
            Topics = new List<SelectListItem>();
            Districts = new List<SelectListItem>();
        }
        public string StoreName { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string YoutubeLink { get; set; }
        public string GooglePlusLink { get; set; }
        public bool WishlistEnabled { get; set; }
        public bool ShoppingCartEnabled { get; set; }
        public bool HideAddresses { get; set; }
        public bool HideOrders { get; set; }
        public bool SitemapEnabled { get; set; }
        public bool NewsEnabled { get; set; }
        public bool BlogEnabled { get; set; }
        public bool CompareProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }
        public bool AllowPrivateMessages { get; set; }
        public bool RecentlyViewedProductsEnabled { get; set; }
        public bool RecentlyAddedProductsEnabled { get; set; }
        public int WorkingLanguageId { get; set; }

        public List<SelectListItem> Districts { get; set; }
        public List<SelectListItem> Topics { get; set; }
    }
}