using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ESRGC.Broadband.ETL.CensusBlock.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SessionExpiredFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            var context = filterContext.HttpContext;

            // check if session is supported
            if (context.Session != null) {

                // check if a new session id was generated
                if (context.Session.IsNewSession) {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = context.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0)) {
                        var routeResult = new RedirectToRouteResult(new RouteValueDictionary() { 
                            { "action", "login" },
                            {"controller", "Account"},
                            {"timeout", true }
                        });
                        filterContext.Result = routeResult;                      
                    }
                }
            }

        }
    }
}