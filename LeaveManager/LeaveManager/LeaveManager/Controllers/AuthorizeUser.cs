using LeaveManager.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace LeaveManager.Controllers
{
    public class AuthorizeUser : ActionFilterAttribute
    {

        public string[] RoleName { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Employee emp = (Employee)filterContext.HttpContext.Session["user"];
            List<String> roles = (List<string>)filterContext.HttpContext.Session["roles"];
            bool canDo = false;
            if (emp != null && roles != null)
            {
                foreach (string r in RoleName)
                {
                    if (roles.Contains(r))
                    {
                        canDo = true;
                        base.OnActionExecuted(filterContext);

                    }
                }

                if (!canDo)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Login", action = "NotAuthorized" }));

                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                }
            }

            else
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }


        }

    }
}