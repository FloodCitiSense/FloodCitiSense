using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Configuration;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using IIASA.FloodCitiSense.Mobile.Core.Properties;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Base
{
    public static class UserConfigurationManager
    {
        private static readonly Lazy<IApplicationContext> AppContext = new Lazy<IApplicationContext>(
            Resolver.Resolve<IApplicationContext>
        );



        private static IAccessTokenManager AccessTokenManager => Resolver.IocManager.Resolve<IAccessTokenManager>();

        public static async Task GetIfNeedsAsync()
        {
            if (AppContext.Value.Configuration != null)
            {
                return;
            }

            await GetAsync();
        }

        public static async Task GetAsync(Func<Task> successCallback = null)
        {

            //var dataStoreManager = Resolver.IocManager.Resolve<DataStorageManager>();

            //if (dataStoreManager.HasKey(DataStorageKey.UserConfiguration))
            //{
            //    AppContext.Value.Configuration =
            //        dataStoreManager.Retrieve<AbpUserConfigurationDto>(DataStorageKey.UserConfiguration);
            //    SetCurrentCulture();
            //    AppContext.Value.CurrentLanguage = AppContext.Value.Configuration.Localization.CurrentLanguage;
            //    WarnIfUserHasNoPermission();
            //    if (successCallback != null)
            //    {
            //        await successCallback();
            //    }
            //}
            //else
            {
                var userConfigurationService = Resolver.IocManager.Resolve<UserConfigurationService>();

                await WebRequestExecuter.Execute(
                    async () => await userConfigurationService.GetAsync(AccessTokenManager.IsUserLoggedIn),
                    async result =>
                        {
                            AppContext.Value.Configuration = result;
                            //await dataStoreManager.StoreAsync(DataStorageKey.UserConfiguration, result);
                            SetCurrentCulture();
                            AppContext.Value.CurrentLanguage = result.Localization.CurrentLanguage;
                            //WarnIfUserHasNoPermission();
                            if (successCallback != null)
                            {
                                await successCallback();
                            }
                        },
                    _ =>
                        {
                            //App.ExitApplication();
                            return Task.CompletedTask;
                        },
                    () => Resolver
                        .IocManager
                        .Release(userConfigurationService)
                );
            }
        }

        private static void WarnIfUserHasNoPermission()
        {
            if (!AccessTokenManager.IsUserLoggedIn)
            {
                return;
            }

            var hasAnyPermission = AppContext.Value.Configuration.Auth.GrantedPermissions != null &&
                                   AppContext.Value.Configuration.Auth.GrantedPermissions.Any();

            if (!hasAnyPermission)
            {
                // UserDialogHelper.Warn("NoPermission");
            }
        }

        private static void SetCurrentCulture()
        {
            var locale = Resolver.Resolve<ILocale>();
            var userCulture = GetUserCulture(locale);

            locale.SetLocale(userCulture);
            Resources.Culture = userCulture;
        }

        private static CultureInfo GetUserCulture(ILocale locale)
        {
            if (AppContext.Value.Configuration.Localization.CurrentCulture.Name == null)
            {
                return locale.GetCurrentCultureInfo();
            }

            try
            {
                return new CultureInfo(AppContext.Value.Configuration.Localization.CurrentCulture.Name);
            }
            catch (CultureNotFoundException)
            {
                return locale.GetCurrentCultureInfo();
            }
        }

    }
}