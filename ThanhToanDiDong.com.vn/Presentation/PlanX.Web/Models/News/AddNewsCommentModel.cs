using System.Web.Mvc;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using PlanX.Web.Validators.News;
using FluentValidation.Attributes;

namespace PlanX.Web.Models.News
{
    [Validator(typeof(NewsItemCommentValidator))]
    public partial class AddNewsCommentModel : BaseNopModel
    {
       
        [NopResourceDisplayName("Web.News.Comments.CommentTitle")]
        [AllowHtml]
        public string CommentTitle { get; set; }

        [NopResourceDisplayName("Web.News.Comments.CommentContent")]
        [AllowHtml]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
        [NopResourceDisplayName("Web.News.Comments.CommentName")]
        [AllowHtml]
        public string AppName { get; set; }
        public string   Place { get; set; }

        public string UserName { get; set; }


        public int ParentId { get; set; }
    }
}