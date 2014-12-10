using FluentValidation;
using PlanX.Admin.Models.TicketConcession;
using PlanX.Services.Localization;

namespace PlanX.Admin.Validators.TicketConcession
{
    public class TicketConcessionValidator : AbstractValidator<TicketConcessionModel>
    {
        public TicketConcessionValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ContactEmail).NotEmpty().WithMessage(localizationService.GetResource("Thông tin này là bắt buộc"));
            RuleFor(x => x.ContactName).NotEmpty().WithMessage(localizationService.GetResource("Thông tin này là bắt buộc"));
            RuleFor(x => x.ContactPhone).NotEmpty().WithMessage(localizationService.GetResource("Thông tin này là bắt buộc"));
            RuleFor(x => x.PassengerName).NotEmpty().WithMessage(localizationService.GetResource("Thông tin này là bắt buộc"));
            RuleFor(x => x.TicketType).NotEmpty().WithMessage(localizationService.GetResource("Thông tin này là bắt buộc"));
            RuleFor(x => x.FromPlace).NotEmpty().WithMessage(localizationService.GetResource("Thông tin này là bắt buộc"));
            RuleFor(x => x.ToPlace).NotEmpty().WithMessage(localizationService.GetResource("Thông tin này là bắt buộc"));
            RuleFor(x => x.ContactEmail).EmailAddress().WithMessage(localizationService.GetResource("Sai email"));
        }
    }
}