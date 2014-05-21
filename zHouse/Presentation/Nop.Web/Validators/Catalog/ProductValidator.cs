using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Validators.Catalog
{
    public class ProductValidator:AbstractValidator<InsertProductModel>
    {
        public ProductValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ContactName).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.ContactName.Required"));
            RuleFor(x => x.ContactPhone).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.ContactPhone.Required"));
            
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.ContactEmail.Required"))
                .EmailAddress().WithMessage(localizationService.GetResource("Common.EmailNotValid"));

            RuleFor(x => x.Price).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.Price.Required"));
            RuleFor(x => x.CateId).NotEqual(0).WithMessage(localizationService.GetResource("Products.Fields.CateId.Required"));
            RuleFor(x => x.Area).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.Area.Required"));
            RuleFor(x => x.Street).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.Street.Required"));
            RuleFor(x => x.WardId).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.WardId.Required"));
            RuleFor(x => x.DistrictId).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.DistrictId.Required"));


          
        }
    }
}