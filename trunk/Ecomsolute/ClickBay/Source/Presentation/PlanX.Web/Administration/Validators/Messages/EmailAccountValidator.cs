using FluentValidation;
using PlanX.Admin.Models.Messages;
using PlanX.Services.Localization;

namespace PlanX.Admin.Validators.Messages
{
    public class EmailAccountValidator : AbstractValidator<EmailAccountModel>
    {
        public EmailAccountValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
            
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}