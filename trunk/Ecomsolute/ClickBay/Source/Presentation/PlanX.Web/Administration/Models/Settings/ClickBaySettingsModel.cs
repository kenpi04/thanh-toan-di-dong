using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Settings
{
    public class ClickBaySettingsModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }



        [NopResourceDisplayName("Admin.Configuration.Settings.ClickBay.PricePlusPerPassenger")]
        public decimal PricePlusPerPassenger { get; set; }
        public bool PricePlusPerPassenger_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ClickBay.DiscountPerPassenger")]
        public decimal DiscountPerPassenger { get; set; }
        public bool DiscountPerPassenger_OverrideForStore { get; set; }
    }
}