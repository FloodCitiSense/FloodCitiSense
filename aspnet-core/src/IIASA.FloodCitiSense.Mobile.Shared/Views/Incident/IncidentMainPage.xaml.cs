using FFImageLoading.Cache;
using FFImageLoading.Forms;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.ViewModels.Incident;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace IIASA.FloodCitiSense.Views.Incident
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentMainPage : ContentPage, IXamarinView
    {
        public IncidentMainPage()
        {
            InitializeComponent();
            SelectionCount = 0;
        }

        /// <summary>
        ///     The take picture button_ clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private async void TakePictureButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await this.DisplayAlert("No Camera", "No camera available", "ok");
                return;
            }

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var fileName = $"{Guid.NewGuid():N}.png";

                //Create actions
                var actions = new string[] {"TakePhoto".Translate(), "PickFromGallery".Translate() };

                //Show simple dialog
                var result = await MaterialDialog.Instance.SelectActionAsync(actions: actions);

                MediaFile file = null;

                if(result == 0)
                {
                    file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions { SaveToAlbum = true, Name = fileName, AllowCropping = false, CompressionQuality = 20 });
                }

                if (result == 1)
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }


                if (file == null)
                {
                    return;
                }

                var cachedImage = new CachedImage
                {
                    HeightRequest = 150,
                    WidthRequest = 150,
                    IsVisible = true,
                    Source = ImageSource.FromStream(file.GetStreamWithImageRotatedForExternalStorage),
                    CacheType = CacheType.Disk,
                    DownsampleToViewSize = true
                };
                //Photos.Children.Add(cachedImage);
                //Photos.Children.Add(new Image { Source = ImageSource.FromStream(() => file.GetStream()), HeightRequest = 150, WidthRequest = 150, IsVisible = true });
                var vm = this.BindingContext as IncidentMainPageModel;
                if (vm != null && vm?.Images == null)
                {
                    vm.Images = new List<string>();
                }
                vm?.Images.Add(file.Path);
            }
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = this.BindingContext as IncidentMainPageModel;
            if (vm?.Report?.Id != null)
            {
                //Photos.Children.Clear();
                if (vm.Images != null && vm.Images.Any())
                {
                    foreach (var img in vm.Images)
                    {
                        var cachedImage = new CachedImage
                        {
                            HeightRequest = 150,
                            WidthRequest = 150,
                            IsVisible = true,
                            Source = ImageSource.FromFile(img),
                            CacheType = CacheType.All,
                            DownsampleToViewSize = true
                        };
                        //Photos.Children.Add(cachedImage);
                    }
                }

                foreach (var child in FloodTypes.Children)
                {
                    if (vm.Report?.TypeOfFloodings.Any(x => child.ClassId.Equals(x.ToString())) == true)
                    {
                        child.BackgroundColor = Color.Gray;
                        SelectionCount++;
                    }
                }

                foreach (var child in TypeOfRain.Children)
                {
                    if (child.ClassId.Equals(vm.Report?.TypeOfRain.ToString()))
                    {
                        child.BackgroundColor = Color.Gray;
                    }
                }

                SetValues();
                //this.CurrentDateAndTime.Text =
                //$"Date and Time : {vm.Report.Date.DateTime.ToShortDateString()} {vm.Report.Date.DateTime.ToShortTimeString()}";
            }
            else
            {
                if (vm != null)
                {
                    vm.Date = DateTime.UtcNow;
                }
                //this.CurrentDateAndTime.Text =
                //$"Date and Time : {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            }
        }

        public int SelectionCount { get; set; }


        private void SelectType(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;

            if (view.BackgroundColor == Color.Gray)
            {
                view.BackgroundColor = Color.White;
                SelectionCount--;
            }
            else
            {
                if (SelectionCount >= 3)
                {
                    DisplayAlert("Error".Translate(), "NotAllowedToSelectAnyMore".Translate(), "ok".Translate());
                }
                else
                {
                    if (view.ClassId == "0")
                    {
                        ResetFloodTypes();
                        AdvanceInfo.IsVisible = false;
                    }
                    else
                    {
                        ResetNoFlood();
                        AdvanceInfo.IsVisible = true;
                    }

                    view.BackgroundColor = Color.Gray;
                    SelectionCount++;
                }
            }

            SetValues();
        }

        private void SetValues()
        {
            if (this.BindingContext is IncidentMainPageModel vm)
            {
                vm.TypeOfFloodings.Clear();
                var views = FloodTypes.Children.Where(x => x.BackgroundColor == Color.Gray).ToList();
                if (views.Count > 0)
                {
                    foreach (var view in views)
                    {
                        var currentEnum = int.Parse(view.ClassId);
                        vm.TypeOfFloodings.Add(currentEnum);
                    }
                }
            }
        }

        private void ResetFloodTypes()
        {
            foreach (var child in FloodTypes.Children)
            {
                child.BackgroundColor = Color.White;
            }

            SelectionCount = 0;
        }

        private void ResetNoFlood()
        {
            var view = FloodTypes.Children.FirstOrDefault(x => x.ClassId == "0");
            if (view != null && view.BackgroundColor == Color.Gray)
            {
                view.BackgroundColor = Color.White;
                SelectionCount--;
            }
        }

        private void AdvanceClicked(object sender, EventArgs e)
        {
            AdvanceSection.IsVisible = true;
            var button = (Button)sender;
            button.IsVisible = false;

            if (this.BindingContext is IncidentMainPageModel vm)
            {
                vm.IsAdvance = true;
            }
        }

        private void TypeOfRain_OnTapped(object sender, EventArgs e)
        {
            foreach (var child in TypeOfRain.Children)
            {
                child.BackgroundColor = Color.White;
            }

            var view = (StackLayout)sender;
            view.BackgroundColor = Color.Gray;

            if (this.BindingContext is IncidentMainPageModel vm)
            {
                vm.TypeOfRain = int.Parse(view.ClassId);
            }
        }
    }
}