using ISPRO.Persistence.Entities;
using ISPRO.Persistence.Enums;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ISPRO.Web.Authorization
{
    public class UserLevelRequirementHandler : AuthorizationHandler<UserLevelRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserLevelRequirement requirement)
        {
            if (context.User != null)
            {
                var role = context.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role);
                if (role != null)
                {
                    if (
                       (requirement.UserLevelAuth == UserLevelAuth.ADMIN && (role?.Value == UserType.ADMIN.ToString()))
                    || (requirement.UserLevelAuth == UserLevelAuth.SUPERUSER && (role?.Value == UserType.ADMIN.ToString() || role?.Value == UserType.MANAGER.ToString()))
                    || ((requirement.UserLevelAuth == UserLevelAuth.AUTHENTICATED) && (role?.Value == UserType.ADMIN.ToString() || role?.Value == UserType.MANAGER.ToString() || role?.Value == UserType.USER_ACCOUNT.ToString()))
                    )
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
