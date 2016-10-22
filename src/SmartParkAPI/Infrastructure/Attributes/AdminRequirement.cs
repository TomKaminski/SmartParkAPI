using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SmartParkAPI.Models;

namespace SmartParkAPI.Infrastructure.Attributes
{
    public class AdminRequirement : AuthorizationHandler<AdminRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            var user = context.User == null ? new AppUserState() : new AppUserState(context.User);
            if (user.IsAdmin)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.FromResult(0);
        }
    }
}
