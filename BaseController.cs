    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = HttpContext.User.Identity;

            if (filterContext.ActionDescriptor.ActionName != "ResetPassword" && user != null && user.IsAuthenticated && ((CustomIdentity)user).Status == EnumDef.AccountStatus.ResetPassword.ToByte())
            {
                if (Request.IsAjaxRequest())
                {
                    filterContext.Result = Json(new { result = "Redirect", url = Url.Action("ResetPassword", "User") });
                    return;
                }
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "User" },
                        { "action", "ResetPassword" }
                    });

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }