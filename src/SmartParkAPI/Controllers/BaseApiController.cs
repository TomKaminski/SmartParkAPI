using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartParkAPI.Models;

namespace SmartParkAPI.Controllers
{
    [Authorize]
    public abstract class BaseApiController : Controller
    {
        protected AppUserState CurrentUser;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (token != null)
            {
                token = token.Replace("Bearer ", "");
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var decodedJwt = jwtSecurityTokenHandler.ReadJwtToken(token);
                CurrentUser = new AppUserState(decodedJwt);
            }
            else
            {
                CurrentUser = new AppUserState();
            }
        }

        protected IEnumerable<string> GetModelStateErrors(ModelStateDictionary modelState)
        {
            var modelStateErrors = new List<string>();
            foreach (var mdl in modelState)
            {
                modelStateErrors.AddRange(mdl.Value.Errors.Select(error => error.ErrorMessage));
            }
            return modelStateErrors;
        }

        protected IActionResult ReturnBadRequestWithModelErrors()
        {
            return new BadRequestObjectResult(GetModelStateErrors(ModelState));
        }

        protected string GetAppBaseUrl()
        {
            return Url.Action("Index", "Home", new {area = "Portal"}, "http");
        }

    }
}
