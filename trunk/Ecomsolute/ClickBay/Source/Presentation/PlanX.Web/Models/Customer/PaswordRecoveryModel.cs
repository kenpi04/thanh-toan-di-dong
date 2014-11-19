using System.Web.Mvc;
using FluentValidation.Attributes;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;
using PlanX.Web.Validators.Customer;

namespace PlanX.Web.Models.Customer
{
    [Validator(typeof(PasswordRecoveryValidator))]
    public partial class PasswordRecoveryModel : BaseNopModel
    {
        [AllowHtml]
        [NopResourceDisplayName("Account.PasswordRecovery.Email")]
        public string Email { get; set; }

        public string Result { get; set; }
    }
}