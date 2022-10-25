using ISPRO.Persistence.Enums;

namespace ISPRO.Web.Authorization
{
    public class Policies
    {
        public static string AdminUserPolicy { get; set; } = UserLevelAuth.ADMIN.ToString();
        public static string SuperUserPolicy { get; set; } = UserLevelAuth.SUPERUSER.ToString();
        public static string AuthenticatedUserPolicy { get; set; } = UserLevelAuth.AUTHENTICATED.ToString();
    }
}
