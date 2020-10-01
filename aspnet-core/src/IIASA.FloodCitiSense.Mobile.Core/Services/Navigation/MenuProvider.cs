using Abp.Dependency;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Mobile.Core.Models.NavigationMenu;
using MvvmHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Navigation
{
    public class MenuProvider : ISingletonDependency, IMenuProvider
    {
        private readonly IAccessTokenManager _accessTokenManager;

        public MenuProvider(IAccessTokenManager accessTokenManager)
        {
            _accessTokenManager = accessTokenManager;
        }

        /* For more icons:
            https://material.io/icons/
        */
        private static IEnumerable<NavigationMenuItem> MenuItems => new Collection<NavigationMenuItem>
        {
            //new NavigationMenuItem
            //{
            //    Title = L.Localize("Home"),
            //    Icon = "home.png",
            //    ViewType = typeof(MainPage),
            //},
            //new NavigationMenuItem
            //{
            //    Title = L.Localize("Welcome"),
            //    Icon = "doormat.png",
            //    ViewType = typeof(WelcomePage),
            //},
            //new NavigationMenuItem
            //{
            //    Title = L.Localize("TermsAndConditions"),
            //    Icon = "statement.png",
            //    ViewType = typeof(TermsPage),
            //},
            //new NavigationMenuItem
            //{
            //    Title = L.Localize("SelectCity"),
            //    Icon = "statement.png",
            //    ViewType = typeof(SelectCityPage),
            //},
            //new NavigationMenuItem
            //{
            //    Title = L.Localize("About"),
            //    Icon = "info.png",
            //    ViewType = typeof(AboutPage),
            //},
            //new NavigationMenuItem
            //{
            //    Title = L.Localize("Contact"),
            //    Icon = "mail.png",
            //    ViewType = typeof(ContactPage),
            //},
            //new NavigationMenuItem
            //{
            //    Title = L.Localize("Users"),
            //    Icon = "user-list.png",
            //    ViewType = typeof(UsersView),
            //    RequiredPermissionName = PermissionKey.Users,
            //},


            /*This is a sample menu item to guide how to add a new item.
                        ,new NavigationMenuItem
                        {
                            Title = "Sample View",
                            Icon = "MyIcon.png",
                            TargetType = typeof(_SampleView),
                            Order = 10
                        }
                    */
        };

        public ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions = null)
        {
            var authorizedMenuItems = new ObservableRangeCollection<NavigationMenuItem>();
            foreach (var menuItem in MenuItems)
            {
                if (menuItem.RequiredPermissionName == null)
                {
                    authorizedMenuItems.Add(menuItem);
                    continue;
                }

                if (grantedPermissions != null &&
                    grantedPermissions.ContainsKey(menuItem.RequiredPermissionName))
                {
                    authorizedMenuItems.Add(menuItem);
                }
            }

            //if (!_accessTokenManager.IsUserLoggedIn)
            //{
            //    authorizedMenuItems.Add(new NavigationMenuItem
            //    {
            //        Title = L.Localize("Login"),
            //        Icon = "login.png",
            //        ViewType = typeof(LoginView),
            //    });
            //    authorizedMenuItems.Add(new NavigationMenuItem
            //    {
            //        Title = L.Localize("Register"),
            //        Icon = "register.png",
            //        ViewType = typeof(RegisterPage),
            //    });
            //}
            //else
            //{
            //    authorizedMenuItems.Add(new NavigationMenuItem
            //    {
            //        Title = L.Localize("MySettings"),
            //        Icon = "settings-gears.png",
            //        ViewType = typeof(MySettingsView)
            //    });
            //}

            return authorizedMenuItems;
        }
    }
}