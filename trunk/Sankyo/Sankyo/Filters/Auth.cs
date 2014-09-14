using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sankyo.Filters
{
    public class Auth : AuthorizeAttribute
    {
        private bool isLogin = false;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //Invoke base authorization


            //If base authorization pass, do custom authorization
            isLogin = httpContext.Session["SessionUser"] != null;

            return isLogin;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!isLogin)
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Write("nologin");
                    return;
                }
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                string url=filterContext.HttpContext.Request.Url.PathAndQuery;
                filterContext.HttpContext.Response.Redirect(urlHelper.Action("Login", "Home", new {returnurl=url }));
                return;
            }


        }
    }
}