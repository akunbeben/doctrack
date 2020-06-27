using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocTrack.Web.App_Start
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            // Check for authorization  
            if (HttpContext.Current.Session["dht-username"] == null)
            {
                filterContext.Result = new RedirectResult("~/Authentication/Login");
            }
        }
    }
}