using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.News
{
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
    }
}