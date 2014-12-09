using FluentValidation;
using PlanX.Core.Domain.ClickBay;
using PlanX.Services.Localization;
using PlanX.Web.Models.ClickBay;

namespace PlanX.Web.Validators.ClickBay
{
    public class TicketConcessionPostValidator : AbstractValidator<TicketConcessionPostModel>
    {
        public TicketConcessionPostValidator(ILocalizationService localizationService)
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