using FluentValidation;
using PlanX.Admin.Models.Plugins;
using PlanX.Services.Localization;

namespace PlanX.Admin.Validators.Plugins
{
    public class PluginValidator : AbstractValidator<PluginModel>
    {
        public PluginValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.FriendlyName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Plugins.Fields.FriendlyName.Required"));
        }
    }
}