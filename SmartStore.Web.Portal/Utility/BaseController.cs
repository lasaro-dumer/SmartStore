using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartStore.Web.Portal.Utility
{
    public abstract class BaseController : Controller
    {
        public const string URLHELPER = "URLHELPER";
        public const string CookieUserId = "_UnauthenticatedUserId";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            context.HttpContext.Items[URLHELPER] = this.Url;
        }
    }
}
