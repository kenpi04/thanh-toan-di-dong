using System.Collections.Generic;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Web.Models.News
{
    public partial class NewsItemListModel : BaseNopModel
    {
        public NewsItemListModel()
        {
            PagingFilteringContext = new NewsPagingFilteringModel();
            NewsItems = new List<NewsItemModel>();
        }

        public int WorkingLanguageId { get; set; }
        public NewsPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }
        public int CurrentCategoryId { get; set; }
        public string CurrentCategoryName { get; set; }
        public string CurrentCategorySeName { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyWords { get; set; }
        public string MetaDescription { get; set; }
    }
}