﻿using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using PlanX.Core.Domain.News;
using PlanX.Web.Framework.Mvc;
using PlanX.Web.Models.Media;
using PlanX.Web.Validators.News;

namespace PlanX.Web.Models.News
{
    [Validator(typeof(NewsItemCommentValidator))]
    public partial class NewsItemModel : BaseNopEntityModel
    {
        public NewsItemModel()
        {
            Comments = new List<NewsCommentModel>();
            AddNewComment = new AddNewsCommentModel();
            //CategoryNewsModel = new CategoryNewsModel();
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
        
        //public string CateName { get; set; }
        //public string CategorySename { get; set; }
        public IList<NewsCommentModel> Comments { get; set; }
        public AddNewsCommentModel AddNewComment { get; set; }
        public CategoryNewsModel CategoryNewsModel { get; set; }
        public PictureModel DefaultPictureModel { get; set; }

        public partial class NewsBreadcrumbModel : BaseNopModel
        {
            public NewsBreadcrumbModel()
            {
                CategoryBreadcrumb = new List<CategoryNewsModel>();
            }

            public int NewsId { get; set; }
            public string NewsName { get; set; }
            public string NewsSeName { get; set; }
            public IList<CategoryNewsModel> CategoryBreadcrumb { get; set; }
        }
    }
}