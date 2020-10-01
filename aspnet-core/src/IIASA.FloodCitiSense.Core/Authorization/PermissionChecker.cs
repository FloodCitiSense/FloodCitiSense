using Abp.Authorization;
using IIASA.FloodCitiSense.Authorization.Roles;
using IIASA.FloodCitiSense.Authorization.Users;

namespace IIASA.FloodCitiSense.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
