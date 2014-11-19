using FluentValidation;
using PlanX.Admin.Models.Topics;
using PlanX.Services.Localization;

namespace PlanX.Admin.Validators.Topics
{
    public class TopicValidator : AbstractValidator<TopicModel>
    {
        public TopicValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SystemName).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Topics.Fields.SystemName.Required"));
        }
    }
}