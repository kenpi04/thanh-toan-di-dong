using FluentValidation;
using PlanX.Services.Localization;
using PlanX.Web.Models.ClickBay;


namespace PlanX.Web.Validators.ClickBay
{
    public class BookingPasserValidator:AbstractValidator<BookingPasserModel>
    {
        public BookingPasserValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("Booking.Field.Passer.Name.Required"));
        }
    }
}