using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using PlanX.Web.Framework.Mvc;
using PlanX.Web.Validators.News;
using PlanX.Core.Domain.Media;
using PlanX.Web.Models.Media;
using PlanX.Core.Domain.News;

namespace PlanX.Web.Models.News
{
    [Validator(typeof(NewsItemValidator))]
    public partial class NewsItemModel : BaseNopEntityModel
    {
        public NewsItemModel()
        {
            Comments = new List<NewsCommentModel>();
            AddNewComment = new AddNewsCommentModel();
            Picture = new PictureModel();
        }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }

        public string Title { get; set; }
        public string Short { get; set; }
        public string Full { get; set; }
        public bool AllowComments { get; set; }
        public int NumberOfComments { get; set; }
        public DateTime CreatedOn { get; set; }

        public PictureModel Picture { get; set; }

        public IList<NewsCommentModel> Comments { get; set; }
        public AddNewsCommentModel AddNewComment { get; set; }

        public string Category { get; set; }

        public string CategorySeName { get; set; }

    }
}