using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThanhToanDiDong.Admin.Filter
{
    public class Auth : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["User"] == null)
                HttpContext.Current.Response.Redirect("/login");
            base.OnActionExecuting(filterContext);
        }
    }
}