using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.News;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;
using Nop.Web.Validators.News;

namespace Nop.Web.Models.News
{
    [Validator(typeof(NewsItemValidator))]
    public partial class NewsItemModel : BaseNopEntityModel
    {
        public NewsItemModel()
        {
            Comments = new List<NewsCommentModel>();
            AddNewComment = new AddNewsCommentModel();
            parentCategoryNews = new List<CategoryNews>();
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
        public string CateName { get; set; }
        public string CategorySename { get; set; }
        public IList<NewsCommentModel> Comments { get; set; }
        public AddNewsCommentModel AddNewComment { get; set; }
        public List<CategoryNews> parentCategoryNews { get; set; }
        public PictureModel DefaultPictureModel { get; set; }
    }
}