using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace Dolly.Controllers
{
    public abstract class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            ViewData["ReturnUri"] = context.HttpContext.Request.Path;
        }

    }
}