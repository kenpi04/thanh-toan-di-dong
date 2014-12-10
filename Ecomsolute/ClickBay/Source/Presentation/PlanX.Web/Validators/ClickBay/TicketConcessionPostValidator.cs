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
            RuleFor(x => x.ContactEmail).NotEmpty().WithMessage(localizationService.GetResource("ticketConcession.contactemail.isrequired"));
            RuleFor(x => x.ContactName).NotEmpty().WithMessage(localizationService.GetResource("ticketConcession.contactname.isrequired"));
            RuleFor(x => x.ContactPhone).NotEmpty().WithMessage(localizationService.GetResource("ticketConcession.contactphone.isrequired"));
            RuleFor(x => x.PassengerName).NotEmpty().WithMessage(localizationService.GetResource("ticketConcession.passengername.isrequired"));
            RuleFor(x => x.TicketType).NotEmpty().WithMessage(localizationService.GetResource("ticketConcession.tickettype.isrequired"));
            RuleFor(x => x.FromPlace).NotEmpty().WithMessage(localizationService.GetResource("ticketConcession.fromplace.isrequired"));
            RuleFor(x => x.ToPlace).NotEmpty().WithMessage(localizationService.GetResource("ticketConcession.toplace.isrequired"));
            RuleFor(x => x.ContactEmail).EmailAddress().WithMessage(localizationService.GetResource("ticketConcession.contactemail.error"));
        }
    }
}