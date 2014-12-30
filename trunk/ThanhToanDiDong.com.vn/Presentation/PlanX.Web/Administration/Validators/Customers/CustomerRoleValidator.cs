using FluentValidation;
using PlanX.Admin.Models.Customers;
using PlanX.Services.Localization;

namespace PlanX.Admin.Validators.Customers
{
    public class CustomerRoleValidator : AbstractValidator<CustomerRoleModel>
    {
        public CustomerRoleValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.Name.Required"));
        }
    }
}