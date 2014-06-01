using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Telerik.Web.Mvc;

namespace Nop.Admin.Models.News
{
    public partial class CategoryNewsListModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.ContentManagement.News.List.SearchNewsTitle")]
        [AllowHtml]
        public string SearchCategoryName { get; set; }

        public GridModel<CategoryNewsModel> Categories { get; set; }
    }
}