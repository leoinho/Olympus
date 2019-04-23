using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fullbar.Olympus.MVC.Filters
{
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UsuarioAdmin"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "login", Action = "Sair" }));
            }
        }
    }
}