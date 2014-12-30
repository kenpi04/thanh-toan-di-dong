using System.Collections.Generic;
using System.Web.Mvc;
//using PlanX.Services.Payments;

namespace PlanX.Web.Framework.Controllers
{
    public abstract class BaseNopPaymentController : Controller
    {
        public abstract IList<string> ValidatePaymentForm(FormCollection form);
        //public abstract ProcessPaymentRequest GetPaymentInfo(FormCollection form);
    }
}
