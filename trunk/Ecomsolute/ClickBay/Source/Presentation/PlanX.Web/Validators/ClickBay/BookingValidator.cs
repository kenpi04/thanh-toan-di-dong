using FluentValidation;
using PlanX.Services.Localization;
using PlanX.Web.Models.ClickBay;

namespace PlanX.Web.Validators.ClickBay
{
    public class BookingValidator:AbstractValidator<BookingModel>
    {
        public BookingValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ContactName).NotEmpty().WithMessage(localizationService.GetResource("Booking.Field.ContactName.Required"));
            RuleFor(x => x.ContactPhone).NotEmpty().WithMessage(localizationService.GetResource("Booking.Field.ContactPhone.Required"));
            RuleFor(x => x.ContactEmail).NotEmpty().EmailAddress().WithMessage(localizationService.GetResource("Booking.Field.ContactEmail.Required"));
            RuleFor(x => x.ContactCityName).NotEmpty().WithMessage(localizationService.GetResource("Booking.Field.ContactCityName.Required"));
            RuleFor(x => x.ContactAddress).NotEmpty().WithMessage(localizationService.GetResource("Booking.Field.ContactAddress.Required"));
        }
    }
}