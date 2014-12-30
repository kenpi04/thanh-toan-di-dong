using System.Web.Mvc;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;
using Telerik.Web.Mvc;

namespace PlanX.Admin.Models.News
{
    public partial class CategoryNewsListModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.ContentManagement.News.List.SearchNewsTitle")]
        [AllowHtml]
        public string SearchCategoryName { get; set; }

        public GridModel<CategoryNewsModel> Categories { get; set; }
    }
}