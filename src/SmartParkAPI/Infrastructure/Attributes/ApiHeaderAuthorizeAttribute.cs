using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace SmartParkAPI.Infrastructure.Attributes
{
    public class ApiHeaderAuthorizeAttribute : ActionFilterAttribute
    {
        private const string HeaderAuthorizeName = "HashHeader";
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers[HeaderAuthorizeName];
            if (string.IsNullOrEmpty(token))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { authenticated = false, message = "Wrong password." }));
            }
            await next();
        }
    }
}
