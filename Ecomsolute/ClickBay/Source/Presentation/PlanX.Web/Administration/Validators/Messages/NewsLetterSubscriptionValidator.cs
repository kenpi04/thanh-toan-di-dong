using FluentValidation;
using PlanX.Admin.Models.Messages;
using PlanX.Services.Localization;

namespace PlanX.Admin.Validators.Messages
{
    public class NewsLetterSubscriptionValidator : AbstractValidator<NewsLetterSubscriptionModel>
    {
        public NewsLetterSubscriptionValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.NewsLetterSubscriptions.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
        }
    }
}