using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IIASA.FloodCitiSense.Authorization.Roles;
using IIASA.FloodCitiSense.Configuration;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Debugging;
using IIASA.FloodCitiSense.MultiTenancy;
using IIASA.FloodCitiSense.Notifications;

namespace IIASA.FloodCitiSense.Authorization.Users
{
    public class UserRegistrationManager : FloodCitiSenseDomainServiceBase
    {
        public IAbpSession AbpSession { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserPolicy _userPolicy;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRegistrationManager(
            TenantManager tenantManager, 
            UserManager userManager,
            RoleManager roleManager, 
            IUserEmailer userEmailer, 
            INotificationSubscriptionManager notificationSubscriptionManager, 
            IAppNotifier appNotifier, 
            IUserPolicy userPolicy,
            IPasswordHasher<User> passwordHasher)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _userPolicy = userPolicy;
            _passwordHasher = passwordHasher;

            AbpSession = NullAbpSession.Instance;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed, string emailActivationLink, ExperienceLevel experienceLevel)
        {
            try
            {
                CheckForTenant();
                CheckSelfRegistrationIsEnabled();

                var tenant = await GetActiveTenantAsync();
                var isNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault);

                await _userPolicy.CheckMaxUserCountAsync(tenant.Id);

                var user = new User
                {
                    TenantId = tenant.Id,
                    Name = name,
                    Surname = surname,
                    EmailAddress = emailAddress,
                    IsActive = isNewRegisteredUserActiveByDefault,
                    UserName = userName,
                    IsEmailConfirmed = isEmailConfirmed,
                    Roles = new List<UserRole>(),
                    ExperienceLevel = experienceLevel
                };

                user.SetNormalizedNames();

                user.Password = _passwordHasher.HashPassword(user, plainPassword);

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
                }

                CheckErrors(await _userManager.CreateAsync(user));
                await CurrentUnitOfWork.SaveChangesAsync();


                if (!user.IsEmailConfirmed)
                {
                    user.SetNewEmailConfirmationCode();
                    await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
                }

                //Notifications
                await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
                await _appNotifier.WelcomeToTheApplicationAsync(user);
                await _appNotifier.NewUserRegisteredAsync(user);

                return user;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration))
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
