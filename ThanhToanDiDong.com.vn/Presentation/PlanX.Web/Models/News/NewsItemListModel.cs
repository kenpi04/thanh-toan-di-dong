using System.Collections.Generic;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Web.Models.News
{
    public partial class NewsItemListModel
    {
        public NewsItemListModel()
        {
            PagingFilteringContext = new NewsPagingFilteringModel();
            NewsItems = new List<NewsItemModel>();
        }

        public int WorkingLanguageId { get; set; }
        public NewsPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string MetaTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }
    }

}