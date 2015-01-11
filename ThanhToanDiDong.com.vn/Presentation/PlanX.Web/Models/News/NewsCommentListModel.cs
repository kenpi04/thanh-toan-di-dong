using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanX.Web.Models.News
{
    public class NewsCommentListModel
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        public int NewsId { get; set; }
        public IList<NewsCommentModel> Comments { get; set; }
    }
}