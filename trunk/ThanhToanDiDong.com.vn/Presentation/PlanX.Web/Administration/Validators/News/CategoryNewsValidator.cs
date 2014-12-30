using FluentValidation;
using PlanX.Admin.Models.News;
using PlanX.Services.Localization;

namespace PlanX.Admin.Validators.News
{
    public class CategoryNewsValidator : AbstractValidator<CategoryNewsModel>
    {
        public CategoryNewsValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(localizationService.GetResource("Admin.Catalog.Categories.Fields.Name.Required"));
        }
    }
}