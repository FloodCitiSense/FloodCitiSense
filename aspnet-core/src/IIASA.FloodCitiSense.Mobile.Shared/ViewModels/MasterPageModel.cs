//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MasterPageViewModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   MasterPageViewModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Abp.IO.Extensions;
using Acr.UserDialogs;
using FFImageLoading;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Authorization.Users.Profile;
using IIASA.FloodCitiSense.Authorization.Users.Profile.Dto;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Controls;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using IIASA.FloodCitiSense.Mobile.Core.Models.NavigationMenu;
using IIASA.FloodCitiSense.Mobile.Core.Services.Permission;
using IIASA.FloodCitiSense.Mobile.Core.UI;
using IIASA.FloodCitiSense.Mobile.Core.UI.Assets;
using IIASA.FloodCitiSense.Sessions.Dto;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Account;
using IIASA.FloodCitiSense.Views.Common;
using IIASA.FloodCitiSense.Views.Init;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace IIASA.FloodCitiSense.ViewModels
{
    /// <summary>
    /// The master page view model.
    /// </summary>
    public class MasterPageModel : XamarinViewModel
    {

        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearing);
        public ICommand ChangeProfilePhotoCommand => new Command(ChangeProfilePhoto);
        public ICommand ShowProfilePhotoCommand => AsyncCommand.Create(ShowProfilePhoto);

        private readonly IProfileAppService _profileAppService;
        private readonly ProxyProfileControllerService _profileControllerService;
        private readonly IApplicationContext _applicationContext;
        private readonly IDataStorageManager _dataStorageManager;
        private readonly IAccessTokenManager _accessTokenManager;

        private ImageSource _photo;
        private const string ApplicationName = "FloodCitiSense";
        private bool _isInitialized;
        private byte[] _profilePictureBytes;
        private string _userNameAndSurname;
        private bool _showMasterPage;
        private string _applicationInfo;
        private string _profilePhotoPath;
        public MasterPageModel(IProfileAppService profileAppService,
                               ProxyProfileControllerService profileControllerService,
                               IApplicationContext applicationContext,
                                IDataStorageManager dataStorageManager,
                                IAccessTokenManager accessTokenManager)
        {
            _profileAppService = profileAppService;
            _profileControllerService = profileControllerService;
            _applicationContext = applicationContext;
            _dataStorageManager = dataStorageManager;
            _accessTokenManager = accessTokenManager;
            _isInitialized = false;
        }

        private async Task PageAppearing()
        {
            if (_isInitialized)
            {
                return;
            }

            if (_dataStorageManager.HasKey(DataStorageKey.LoginInfo))
            {
                _applicationContext.LoginInfo = _dataStorageManager.Retrieve<GetCurrentLoginInformationsOutput>(DataStorageKey.LoginInfo);
            }



            var tenant = _dataStorageManager.Retrieve<string>(DataStorageKey.TenancyName);
            var tenantId = _dataStorageManager.Retrieve<int>(DataStorageKey.TenantId);
            if (!string.IsNullOrEmpty(tenant) && tenantId > 0)
            {
                _applicationContext?.SetAsTenant(tenant, tenantId);
            }

            var applicationContext = _applicationContext;
            if (applicationContext != null)
            {
                applicationContext.AppInformation = new AppInformation();
                if (_dataStorageManager.HasKey(DataStorageKey.LastTokenRequestedTime))
                {
                    applicationContext.AppInformation.LastTokenRequestedTime =
                        _dataStorageManager.Retrieve<DateTime>(DataStorageKey.LastTokenRequestedTime);
                }

                if (_dataStorageManager.HasKey(DataStorageKey.AccessToken))
                {
                    applicationContext.AppInformation.AccessToken =
                        _dataStorageManager.Retrieve<string>(DataStorageKey.AccessToken);
                }

                if (_dataStorageManager.HasKey(DataStorageKey.RefreshToken))
                {
                    applicationContext.AppInformation.RefreshToken =
                        _dataStorageManager.Retrieve<string>(DataStorageKey.RefreshToken, shouldDecrpyt: true);
                }

                applicationContext.AppInformation.PropertyChanged += async (sender, args) =>
                {
                    Debug.WriteLine(sender);
                    switch (args.PropertyName)
                    {
                        case "AccessToken":
                            if (applicationContext.AppInformation.AccessToken != null)
                            {
                                await _dataStorageManager.StoreAsync(DataStorageKey.AccessToken, applicationContext.AppInformation.AccessToken);
                            }
                            break;
                        case "RefreshToken":
                            if (applicationContext.AppInformation.RefreshToken != null)
                            {
                                await _dataStorageManager.StoreAsync(DataStorageKey.RefreshToken, applicationContext.AppInformation.RefreshToken, true);
                            }
                            break;
                        case "LastTokenRequestedTime":
                            await _dataStorageManager.StoreAsync(DataStorageKey.LastTokenRequestedTime, applicationContext.AppInformation.LastTokenRequestedTime);
                            break;
                    }
                };
                try
                {
                    await applicationContext.AppInformation.GetAccessTokenAsync();
                }
                catch (Exception e)
                {
                    applicationContext.AppInformation = null;
                    _dataStorageManager.RemoveIfExists(DataStorageKey.RefreshToken);
                    _dataStorageManager.RemoveIfExists(DataStorageKey.LastTokenRequestedTime);
                    _dataStorageManager.RemoveIfExists(DataStorageKey.AccessToken);
                    _dataStorageManager.RemoveIfExists(DataStorageKey.LoginInfo);
                }
            }
            if (_accessTokenManager.IsUserLoggedIn && _applicationContext?.LoginInfo?.User != null)
            {
                UserNameAndSurname = _applicationContext.LoginInfo.User.Name; //+ " " + _applicationContext.LoginInfo.User.Surname;
                                                                              //await GetUserPhoto(_applicationContext.LoginInfo.User.ProfilePictureId);
                string photoId = _applicationContext.LoginInfo.User.ProfilePictureId;

                if (_dataStorageManager.HasKey(DataStorageKey.UserProfilePhoto + photoId))
                {
                    string photoPath = _dataStorageManager.Retrieve<string>(DataStorageKey.UserProfilePhoto + photoId);
                    if (!string.IsNullOrEmpty(photoPath))
                    {
                        Photo = ImageSource.FromFile(photoPath);

                    }
                }
                else
                {
                   await GetUserPhoto(photoId);
                }
            }
            else
            {
                UserNameAndSurname = string.Empty;
                Photo = ImageSource.FromResource(AssetsHelper.ProfileImagePlaceholderNamespace);
            }

            BuildMenuItems();
            SetApplicationInfo();
            _isInitialized = true;
        }

        private void SetApplicationInfo()
        {
            ApplicationInfo = $"{ApplicationName} " +
                               $"v{AppInfo.VersionString}.{AppInfo.BuildString} ";
        }

        public string UserNameAndSurname
        {
            get => _userNameAndSurname;
            set
            {
                _userNameAndSurname = value;
                RaisePropertyChanged(() => UserNameAndSurname);
            }
        }

        public string ApplicationInfo
        {
            get => _applicationInfo;
            set
            {
                _applicationInfo = value;
                RaisePropertyChanged(() => ApplicationInfo);
            }
        }

        public int MenuItemsCount
        {
            get
            {
                if (MenuItems == null)
                {
                    return -1;
                }

                return MenuItems.Count;
            }
        }

        public ImageSource Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                RaisePropertyChanged(() => Photo);
            }
        }


        private async Task GetUserPhoto(string profilePictureId)
        {
            if (!Guid.TryParse(profilePictureId, out var guid))
            {
                return;
            }
            await WebRequestExecuter.Execute(async () =>
            {
                var result = await _profileAppService.GetProfilePictureById(guid);
                _profilePictureBytes = Convert.FromBase64String(result.ProfilePicture);
                Photo = ImageSource.FromStream(() => new MemoryStream(_profilePictureBytes));
            }, () => Task.CompletedTask);

        }

        private void ChangeProfilePhoto()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig
            {
                Title = L.Localize("ChangeProfilePicture"),
                Options = new List<ActionSheetOption>  {
                                                                                                    new ActionSheetOption(L.Localize("PickFromGallery"),  async () => await PickProfilePictureAsync(CropImageIfNeedsAsync)),
                                                                                                    new ActionSheetOption(L.Localize("TakePhoto"),  async () => await TakeProfilePhotoAsync(CropImageIfNeedsAsync))
                                                                                                }
            });
        }

        private async Task CropImageIfNeedsAsync(MediaFile photo)
        {
            if (photo == null)
            {
                return;
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                var cropModalView = new CropView(photo.Path, OnCropCompleted, L.Localize("Ok"), L.Localize("Rotate"), L.Localize("Cancel"));
                _profilePhotoPath = photo.Path;
                await ModalService.ShowModalAsync(cropModalView);
            }
            else
            {
                await OnCropCompleted(File.ReadAllBytes(photo.Path), Path.GetFileName(photo.Path));
            }
        }

        private async Task OnCropCompleted(byte[] croppedImageBytes, string fileName)
        {
            if (croppedImageBytes == null)
            {
                return;
            }

            var jpgStream = await ResizeImageAsync(croppedImageBytes);
            await SaveProfilePhoto(jpgStream.GetAllBytes(), fileName);
        }

        private async Task<Stream> ResizeImageAsync(byte[] imageBytes, int width = 256, int height = 256)
        {
            var result = ImageService.Instance.LoadStream(token =>
            {
                var tcs = new TaskCompletionSource<Stream>();
                tcs.TrySetResult(new MemoryStream(imageBytes));
                return tcs.Task;
            }).DownSample(width, height);

            return await result.AsJPGStreamAsync();
        }

        private static async Task PickProfilePictureAsync(Func<MediaFile, Task> picturePicked)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return;
            }

            using (var photo = await CrossMedia.Current.PickPhotoAsync())
            {
                await picturePicked(photo);
            }
        }

        private async Task TakeProfilePhotoAsync(Func<MediaFile, Task> photoTaken)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable ||
                !CrossMedia.Current.IsTakePhotoSupported)
            {
                UserDialogHelper.Warn("NoCamera");
                return;
            }

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera, Permission.Storage);
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                await SetBusyAsync(async () =>
                {
                    using (var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        DefaultCamera = CameraDevice.Front,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        AllowCropping = true,
                        CompressionQuality = 80,
                        MaxWidthHeight = 700
                    }))
                    {
                        await photoTaken(photo);
                    }
                });
            }
        }

        private async Task SaveProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(async () => await UpdateProfilePhoto(photoAsBytes, fileName), () =>
                {
                    Photo = ImageSource.FromStream(() => new MemoryStream(photoAsBytes));
                    CloneProfilePicture(photoAsBytes);

                    return Task.CompletedTask;
                });
            });
        }

        private void CloneProfilePicture(byte[] photoAsBytes)
        {
            _profilePictureBytes = new byte[photoAsBytes.Length];
            photoAsBytes.CopyTo(_profilePictureBytes, 0);
        }

        private async Task UpdateProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            using (Stream photoStream = new MemoryStream(photoAsBytes))
            {
                var uploadResult = await _profileControllerService.UploadProfilePicture(photoStream, fileName);
                if (uploadResult?.FileName != null)
                {
                    await _profileAppService.UpdateProfilePicture(new UpdateProfilePictureInput
                    {
                        FileName = uploadResult.FileName
                    });
                    if (_accessTokenManager.IsUserLoggedIn && _applicationContext?.LoginInfo?.User != null)
                    {
                        string photoId = _applicationContext.LoginInfo.User.ProfilePictureId;
                        await _dataStorageManager.StoreAsync(DataStorageKey.UserProfilePhoto + photoId,
                            _profilePhotoPath);
                    }
                }
            }
        }

        private async Task ShowProfilePhoto()
        {
            if (_profilePictureBytes == null)
            {
                return;
            }

            await ModalService.ShowModalAsync<ProfilePictureView>(_profilePictureBytes);
        }

        private void WatchLanguageChanges()
        {
            MessagingCenter.Instance.SubscribeSafe<MySettingsViewModel>(this, MessagingCenterKeys.LanguagesChanged, sender =>
            {
                BuildMenuItems();
            });
        }

        #region Navigation Menu
        public void BuildMenuItems()
        {
            var menuItems = new Collection<NavigationMenuItem>
            {
                new NavigationMenuItem
                {
                    Title = L.Localize("Home"),
                    Icon = "home.png",
                    ViewType = typeof(MainPage)
                },
                new NavigationMenuItem
                {
                    Title = L.Localize("Welcome"),
                    Icon = "doormat.png",
                    ViewType = typeof(WelcomePage)
                },
                new NavigationMenuItem
                {
                    Title = L.Localize("About"),
                    Icon = "info.png",
                    ViewType = typeof(AboutPage)
                },
                new NavigationMenuItem
                {
                    Title = L.Localize("Contact"),
                    Icon = "mail.png",
                    ViewType = typeof(ContactPage)
                },
                new NavigationMenuItem
                {
                    Title = L.Localize("Users"),
                    Icon = "user-list.png",
                    ViewType = typeof(UsersView),
                    RequiredPermissionName = PermissionKey.Users
                }
            };

            if (!_accessTokenManager.IsUserLoggedIn)
            {
                menuItems.Add(new NavigationMenuItem
                {
                    Title = L.Localize("LogIn"),
                    Icon = "login.png",
                    ViewType = typeof(LoginView)
                });
            }
            else
            {
                menuItems.Add(new NavigationMenuItem
                {
                    Title = L.Localize("MySettings"),
                    Icon = "settings-gears.png",
                    ViewType = typeof(MySettingsView)
                });
            }

            var authorizedMenuItems = new ObservableRangeCollection<NavigationMenuItem>();
            foreach (var menuItem in menuItems)
            {
                if (menuItem.RequiredPermissionName == null)
                {
                    authorizedMenuItems.Add(menuItem);
                    continue;
                }

                if (_applicationContext?.Configuration?.Auth?.GrantedPermissions != null &&
                    _applicationContext.Configuration.Auth.GrantedPermissions.ContainsKey(menuItem.RequiredPermissionName))
                {
                    authorizedMenuItems.Add(menuItem);
                }
            }

            //if (_applicationContext.Configuration != null)
            //{
            //    var grantedMenutems = _applicationContext.Configuration.Auth.GrantedPermissions;
            //    MenuItems = Resolver.Resolve<IMenuProvider>().GetAuthorizedMenuItems(grantedMenutems);
            //}
            //else
            //{
            //    MenuItems = Resolver.Resolve<IMenuProvider>().GetAuthorizedMenuItems(null);
            //}
            MenuItems = authorizedMenuItems;
            RaisePropertyChanged(() => MenuItemsCount);
        }

        private ObservableRangeCollection<NavigationMenuItem> _menuItems;
        public ObservableRangeCollection<NavigationMenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }


        private NavigationMenuItem _selectedMenuItem;
        public NavigationMenuItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                _selectedMenuItem = value;
                ClearSelectedMenu();
                if (_selectedMenuItem != null)
                {
                    AsyncRunner.Run(NavigateToMenuAsync(_selectedMenuItem));
                }

                RaisePropertyChanged(() => SelectedMenuItem);
            }
        }

        public bool ShowMasterPage
        {
            get => _showMasterPage;
            set
            {
                _showMasterPage = value;
                RaisePropertyChanged(() => ShowMasterPage);
            }
        }

        private void ClearSelectedMenu()
        {
            MenuItems.ForEach(m => m.IsSelected = false);
        }

        private async Task NavigateToMenuAsync(NavigationMenuItem newNavigationMenu)
        {
            ShowMasterPage = false;
            SelectedMenuItem.IsSelected = true;
            await NavigationService.SetDetailPageAsync(newNavigationMenu.ViewType, _selectedMenuItem.NavigationParameter);
        }

        #endregion
    }
}