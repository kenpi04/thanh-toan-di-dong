using FluentValidation;
using PlanX.Admin.Models.Messages;
using PlanX.Services.Localization;

namespace PlanX.Admin.Validators.Messages
{
    public class QueuedEmailValidator : AbstractValidator<QueuedEmailModel>
    {
        public QueuedEmailValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Priority).NotNull().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.Priority.Required"))
                                    .InclusiveBetween(0, 99999).WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.Priority.Range"));

            RuleFor(x => x.From).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.From.Required"));
            RuleFor(x => x.To).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.To.Required"));

            RuleFor(x => x.SentTries).NotNull().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.SentTries.Required"))
                                    .InclusiveBetween(0, 99999).WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.SentTries.Range"));

        }
    }
}