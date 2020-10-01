using Abp.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using IIASA.FloodCitiSense.Authorization.Roles;
using IIASA.FloodCitiSense.Authorization.Users;
using IIASA.FloodCitiSense.MultiTenancy;

namespace IIASA.FloodCitiSense.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options, 
            SignInManager signInManager,
            ISystemClock systemClock) 
            : base(options, signInManager, systemClock)
        {
        }
    }
}