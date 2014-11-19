using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Home
{
    public partial class DashboardModel : BaseNopModel
    {
        public bool IsLoggedInAsVendor { get; set; }
    }
}