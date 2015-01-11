using FluentValidation;
using PlanX.Services.Localization;
using PlanX.Web.Models.News;

namespace PlanX.Web.Validators.News
{
    public class NewsItemCommentValidator : AbstractValidator<AddNewsCommentModel>
    {
        public NewsItemCommentValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Place).NotEmpty().WithMessage(localizationService.GetResource("News.Comments.Place.Required"));
            RuleFor(x => x.UserName).NotEmpty().WithMessage(string.Format(localizationService.GetResource("News.Comments.UserName.Required")));
            RuleFor(x => x.CommentText).NotEmpty().WithMessage(localizationService.GetResource("News.Comments.CommentText.Required"));
        }
    }
}