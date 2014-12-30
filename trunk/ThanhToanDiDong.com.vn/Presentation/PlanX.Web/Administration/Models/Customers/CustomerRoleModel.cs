using System.Web.Mvc;
using FluentValidation.Attributes;
using PlanX.Admin.Validators.Customers;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Customers
{
    [Validator(typeof(CustomerRoleValidator))]
    public partial class CustomerRoleModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Customers.CustomerRoles.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        //[NopResourceDisplayName("Admin.Customers.CustomerRoles.Fields.FreeShipping")]
        //[AllowHtml]
        //public bool FreeShipping { get; set; }

        //[NopResourceDisplayName("Admin.Customers.CustomerRoles.Fields.TaxExempt")]
        //public bool TaxExempt { get; set; }

        [NopResourceDisplayName("Admin.Customers.CustomerRoles.Fields.Active")]
        public bool Active { get; set; }

        [NopResourceDisplayName("Admin.Customers.CustomerRoles.Fields.IsSystemRole")]
        public bool IsSystemRole { get; set; }

        [NopResourceDisplayName("Admin.Customers.CustomerRoles.Fields.SystemName")]
        public string SystemName { get; set; }
    }
}