using ISPRO.Persistence.Context;
using ISPRO.Persistence.Entities;
using ISPRO.Persistence.Enums;
using ISPRO.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ISPRO.Web.Authorization
{
    public class UserLevelRequirementHandler : AuthorizationHandler<UserLevelRequirement>
    {
        private readonly DataContext dataContext;

        public UserLevelRequirementHandler(DataContext context) : base()
        {
            this.dataContext = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserLevelRequirement requirement)
        {
            if (context.User != null)
            {
                var role = context.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role);
                if (role != null)
                {
                    PathString requestPath = null;
                    string[]? pathParts = null;
                    string? entityClass = null;
                    string? targetAPI = null;
                    string? entityId = null;

                    if (context.Resource != null)
                    {
                        requestPath = ((DefaultHttpContext)context.Resource).Request.Path;
                        pathParts = requestPath.Value?.Split('/');

                        if (pathParts != null && pathParts?.Count() > 3)
                        {
                            entityClass = pathParts[1].Substring(0, pathParts[1].Length - 1); ;
                            targetAPI = pathParts[2];
                            entityId = pathParts[3]?.Trim();

                            if ((role?.Value == UserType.ADMIN.ToString() || role?.Value == UserType.MANAGER.ToString() || role?.Value == UserType.USER_ACCOUNT.ToString()) &&
                                (entityClass.Equals(typeof(ManagerAccount).Name, StringComparison.CurrentCultureIgnoreCase) || entityClass.Equals(typeof(UserAccount).Name, StringComparison.CurrentCultureIgnoreCase)) &&
                                targetAPI.Equals("Profile", StringComparison.InvariantCultureIgnoreCase) && entityId.Equals(context.User.Identity.Name, StringComparison.CurrentCultureIgnoreCase))
                            {
                                foreach (var req in context.PendingRequirements)
                                {
                                    context.Succeed(req);
                                }
                                return Task.CompletedTask;
                            }
                        }
                    }

                    if ((requirement.UserLevelAuth == UserLevelAuth.ADMIN && (role?.Value == UserType.ADMIN.ToString()))
                        || (requirement.UserLevelAuth == UserLevelAuth.SUPERUSER && (role?.Value == UserType.ADMIN.ToString() || role?.Value == UserType.MANAGER.ToString()))
                        || ((requirement.UserLevelAuth == UserLevelAuth.AUTHENTICATED) && (role?.Value == UserType.ADMIN.ToString() || role?.Value == UserType.MANAGER.ToString() || role?.Value == UserType.USER_ACCOUNT.ToString())))
                    {
                        if (context.Resource != null)
                        {
                            if(targetAPI != null)
                            {
                                if (role?.Value != UserType.ADMIN.ToString() && targetAPI != null && 
                                    (targetAPI.Equals("Details", StringComparison.InvariantCultureIgnoreCase) ||
                                    targetAPI.Equals("Edit", StringComparison.InvariantCultureIgnoreCase) ||
                                    targetAPI.Equals("Delete", StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    if (entityId != null && entityClass!=null)
                                    {
                                        bool? isMatching = null;

                                        if (entityClass.Equals(typeof(Project).Name, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            isMatching = dataContext.Projects.Where(x => x.Name == entityId && x.ProjectManager.Username == context.User.Identity.Name).Any();
                                        }
                                        else if (entityClass.Equals(typeof(Subscription).Name, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            isMatching = dataContext.Subscriptions.Where(x => x.Id == int.Parse(entityId) && x.Project.ProjectManager.Username == context.User.Identity.Name).Any();
                                        }
                                        else if (entityClass.Equals(typeof(UserAccount).Name, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            isMatching = dataContext.UserAccounts.Where(x => x.Username == entityId && x.Project.ProjectManager.Username == context.User.Identity.Name).Any();
                                        }
                                        else if (entityClass.Equals(typeof(PrePaidCard).Name, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            isMatching = dataContext.PrePaidCards.Where(x => x.Id == entityId && x.Subscription.Project.ProjectManager.Username == context.User.Identity.Name).Any();
                                        }
                                        else if (entityClass.Equals(typeof(CashPayment).Name, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            isMatching = dataContext.CashPayments.Where(x => x.Id == int.Parse(entityId) && x.UserAccount.Project.ProjectManager.Username == context.User.Identity.Name).Any();
                                        }

                                        if (isMatching.HasValue && isMatching.Value == false)
                                        {
                                            context.Fail();
                                            return Task.CompletedTask;
                                        }
                                    }
                                }
                            
                            }
                        }
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
