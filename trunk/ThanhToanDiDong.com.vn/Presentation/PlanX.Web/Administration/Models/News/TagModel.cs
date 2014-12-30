using FluentValidation.Attributes;
using PlanX.Admin.Validators.News;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;
using System.Web.Mvc;

namespace PlanX.Admin.Models.News
{
    [Validator(typeof(TagValidator))]
    public class TagModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.News.Tags.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.News.Tags.Fields.EnglishName")]
        [AllowHtml]
        public string EnglishName { get; set; }
        [NopResourceDisplayName("Admin.News.Tags.Fields.IsShowHomePage")]
        public bool IsShowHomePage { get; set; }
        [NopResourceDisplayName("Admin.News.Tags.Fields.NewsItemCount")]
        public int NewsItemCount { get; set; }
    }
}