using PlanX.Web.Framework.Mvc;
using System.Collections.Generic;

namespace PlanX.Web.Models.News
{
    public class NewsByTagModel : BaseNopEntityModel
    {
        public NewsByTagModel()
        {
            PagingFilteringContext = new NewsPagingFilteringModel();
            NewsItems = new List<NewsItemModel>();
        }

        public string TagName { get; set; }
        public string SeName { get; set; }
        public NewsPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }

    }
}