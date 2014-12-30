using PlanX.Admin.Models.Common;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Customers
{
    public partial class CustomerAddressModel : BaseNopModel
    {
        public int CustomerId { get; set; }

        public AddressModel Address { get; set; }
    }
}