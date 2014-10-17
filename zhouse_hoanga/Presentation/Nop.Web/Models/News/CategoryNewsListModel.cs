using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.News;

namespace Nop.Web.Models.News
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
    
}