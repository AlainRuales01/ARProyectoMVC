using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ARProyectoWeb.Utilities
{
    public class LoginFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userName = context.HttpContext.Session.GetString("UserName");
            
            if (string.IsNullOrEmpty(userName))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
