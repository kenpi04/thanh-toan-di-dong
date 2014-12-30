using FluentValidation;
using PlanX.Services.Localization;
using PlanX.Web.Models.PrivateMessages;

namespace PlanX.Web.Validators.PrivateMessages
{
    public class SendPrivateMessageValidator : AbstractValidator<SendPrivateMessageModel>
    {
        public SendPrivateMessageValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("PrivateMessages.SubjectCannotBeEmpty"));
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationService.GetResource("PrivateMessages.MessageCannotBeEmpty"));
        }
    }
}