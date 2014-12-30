using PlanX.Web.Framework.Mvc;

namespace PlanX.Web.Models.Customer
{
    public partial class CustomerAvatarModel : BaseNopModel
    {
        public string AvatarUrl { get; set; }
        public CustomerNavigationModel NavigationModel { get; set; }
    }
}