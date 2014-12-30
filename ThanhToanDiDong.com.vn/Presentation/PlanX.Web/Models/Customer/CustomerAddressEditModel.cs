using PlanX.Web.Framework.Mvc;
using PlanX.Web.Models.Common;

namespace PlanX.Web.Models.Customer
{
    public partial class CustomerAddressEditModel : BaseNopModel
    {
        public CustomerAddressEditModel()
        {
            this.Address = new AddressModel();
        }
        public AddressModel Address { get; set; }
        public CustomerNavigationModel NavigationModel { get; set; }
    }
}