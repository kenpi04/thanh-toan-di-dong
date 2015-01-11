using System;
using PlanX.Web.Framework.Mvc;
using System.Collections.Generic;

namespace PlanX.Web.Models.News
{
    public partial class NewsCommentModel : BaseNopEntityModel
    {
        public NewsCommentModel()
        {
            SubComments = new List<NewsCommentModel>();
        }
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAvatarUrl { get; set; }

        public string CommentTitle { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool AllowViewingProfiles { get; set; }

        public bool IsApproved { get; set; }

        public string AppName { get; set; }

        public string Place { get; set; }

        public int TotalLike { get; set; }

        public IList<NewsCommentModel> SubComments { get; set; }
    }
}