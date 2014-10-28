﻿using FluentValidation;
using Nop.Core;
using Nop.Services.Localization;
using Nop.Web.Models.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Catalog;

namespace Nop.Web.Validators.Catalog
{
    public class ProductValidator : AbstractValidator<InsertProductModel>
    {
        public ProductValidator(ILocalizationService localizationService, IWorkContext context)
        {


            RuleFor(x => x.ContactName).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.ContactName.Required"));
            RuleFor(x => x.ContactPhone).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.ContactPhone.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.EmailNotValid"));
            RuleFor(x => x.StreetId).NotEqual(0).When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Products.Fields.Street.Required"));
            RuleFor(x => x.WardId).NotEqual(0).When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Products.Fields.WardId.Required"));
            RuleFor(x => x.DistrictId).NotEqual(0).When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Products.Fields.DistrictId.Required"));

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Product.Fields.Name.Required"))
                .Length(10, 150).WithMessage(localizationService.GetResource("Product.Field.Name.LengthRequired"));
            RuleFor(x => x.FullDescription).NotEmpty().When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Product.Field.FullDescription.Required"));
            RuleFor(x => x.Price).NotEmpty().When(x => x.ProductType == (int)ProductType.RentProduct).WithMessage(localizationService.GetResource("Products.Fields.Price.Required"))
                .LessThan(99999).WithMessage(localizationService.GetResource("Product.Fields.Price.Overload"));
            RuleFor(x => x.CateId).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.CateId.Required"));
            RuleFor(x => x.Area).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.Area.Required"));
                //.InclusiveBetween(1, 99999).When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Products.Fields.Area.InclusiveBetween"));

            RuleFor(x => x.Width).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.Width.Required"))
                .InclusiveBetween(1, 99999).When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Products.Fields.Width.GreaterRange"));
                //.LessThan(99999).When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Products.Fields.Width.LessRange"));
            
            RuleFor(x => x.Dept).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.Dept.Required"))
                .InclusiveBetween(1, 99999).When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Products.Fields.Dept.GreaterRange"));
                //.LessThan(99999).When(x => x.ProductType != (int)ProductType.ProjectProduct).WithMessage(localizationService.GetResource("Products.Fields.Dept.LessRange"));

            RuleFor(x => x.AvailableStartDateTime).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.AvailableStartDateTime.Required"));
            RuleFor(x => x.AvailableEndDateTime).NotEmpty().WithMessage(localizationService.GetResource("Products.Fields.AvailableEndDateTime.Required"));
        }
    }
}