using ISPRO.Persistence.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ISPRO.Web.Authorization
{
    public class UserLevelRequirement : IAuthorizationRequirement
    {
        public UserLevelAuth UserLevelAuth { get; set; }

        public UserLevelRequirement(UserLevelAuth UserLevelAuth_)
        {
            UserLevelAuth = UserLevelAuth_;
        }
    }
}
