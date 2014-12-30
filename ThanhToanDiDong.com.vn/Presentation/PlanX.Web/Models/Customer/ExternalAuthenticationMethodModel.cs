using System.Web.Routing;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Web.Models.Customer
{
    public partial class ExternalAuthenticationMethodModel : BaseNopModel
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}