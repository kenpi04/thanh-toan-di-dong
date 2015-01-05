using System.Collections.Generic;
using PlanX.Core.Domain.News;

namespace PlanX.Web.Models.News
{
    public partial class CategoryNewsListModel
    {
        public CategoryNewsListModel()
        {
            CheckedCategoryNewsId = 0;
            CategoryNews = new List<CategoryNews>();
        }

        public int WorkingLanguageId { get; set; }
        public IList<CategoryNews> CategoryNews { get; set; }
        public int CheckedCategoryNewsId { get; set; }
    }

    public class NavigativeCategory 
    {
        public NavigativeCategory()
        {
            CategoryNewsChild = new List<CategoryNewsModel>();
        }
        public int CurrentCategoryId { get; set; }
        public IList<CategoryNewsModel> CategoryNewsChild { get; set; }
    }

    public class CategoryNewsModel : PlanX.Web.Framework.Mvc.BaseNopEntityModel
    {               
        public string Name { get; set; }
        public IList<CategoryNewsModel> CategoryNewsChild { get; set; }

        public string SeName { get; set; }

        public int NewsCount { get; set; }

        public CategoryNewsModel ParentCategory { get; set; }
    }
}