using System.Web.Mvc;
using FluentValidation.Attributes;
using PlanX.Admin.Validators.Settings;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Settings
{
    [Validator(typeof(SettingValidator))]
    public partial class SettingModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Value")]
        [AllowHtml]
        public string Value { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.StoreName")]
        public string Store { get; set; }
        public int StoreId { get; set; }
    }
}