using ISPRO.Persistence.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ISPRO.Web.Authorization
{
    public class AuthorizeUserLevel : AuthorizeAttribute
    {
        public AuthorizeUserLevel(UserLevelAuth userType):base(userType.ToString())
        {

        }
    }
}
