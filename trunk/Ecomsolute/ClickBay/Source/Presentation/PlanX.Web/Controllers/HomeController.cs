using System.Web.Mvc;
using PlanX.Web.Framework.Security;

namespace PlanX.Web.Controllers
{
    public partial class HomeController : BaseNopController
    {
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
