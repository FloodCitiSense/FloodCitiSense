//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IncidentMainPageModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   IncidentMainPageModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Acr.UserDialogs;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Models.Common;
using IIASA.FloodCitiSense.Mobile.Core.Services.Location;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Mobile.Core.Services.Report;
using IIASA.FloodCitiSense.Views;
using IIASA.FloodCitiSense.Views.Incident;
using Microsoft.AppCenter.Analytics;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IIASA.FloodCitiSense.Mobile.Core.Services.Camera;
using IIASA.FloodCitiSense.Views.Map;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace IIASA.FloodCitiSense.ViewModels.Incident
{
    public class IncidentMainPageModel : XamarinViewModel
    {
        private readonly ILocationService _locationService;
        private readonly INavigationService _navigationService;
        private readonly ICameraService _cameraService;
        private readonly IIncidentService _incidentService;

        private bool _anySignOfDamage;

        private bool _anySignOfObstruction;

        private bool _areYouImpacted;

        private DateTimeOffset _date;
        private int _floodDepth;
        private int _floodExtent;
        private int _frequencyOfFlood;

        private List<string> _images;

        private Mobile.Core.Core.Entity.LocalIncident _report;
        private int _typeOfFlooding;
        private int _typeOfRain;
        private int _typesOfSpaceFlooded;
        private int _waterClarity;

        public IncidentMainPageModel(ILocationService locationService, IIncidentService incidentService,
            INavigationService navigationService, ICameraService cameraService)
        {
            Report = null;
            _locationService = locationService;
            _incidentService = incidentService;
            _navigationService = navigationService;
            _cameraService = cameraService;
            TypeOfRainItems = EnumHelper<TypeOfRain>.GetDisplayItems();
            FloodDepthItems = EnumHelper<FloodDepth>.GetDisplayItems();
            FloodExtentItems = EnumHelper<FloodExtent>.GetDisplayItems();
            FrequencyOfFloodItems = EnumHelper<FrequencyOfFlood>.GetDisplayItems();
            TypeOfFloodingItems = EnumHelper<TypeOfFlood>.GetDisplayItems();
            TypesOfSpaceFloodedItems = EnumHelper<TypesOfSpaceFlooded>.GetDisplayItems();
            WaterClarityItems = EnumHelper<WaterClarity>.GetDisplayItems();
            TypeOfFloodings = new List<int>();
            FloodDepth = -1;
            FloodExtent = -1;
            FrequencyOfFlood = -1;
            TypesOfSpaceFlooded = -1;
            WaterClarity = -1;
            TypeOfRain = -1;
            TypeOfFlooding = -1;
            LocalImages = new ObservableCollection<LocalImage>();
        }

        public LocalIncident Report
        {
            get => _report;
            set
            {
                _report = value;
                RaisePropertyChanged(() => Report);
            }
        }

        public List<DisplayItem> TypeOfRainItems { get; set; }
        public List<DisplayItem> FloodDepthItems { get; set; }
        public List<DisplayItem> FloodExtentItems { get; set; }
        public List<DisplayItem> FrequencyOfFloodItems { get; set; }
        public List<DisplayItem> TypeOfFloodingItems { get; set; }
        public List<DisplayItem> TypesOfSpaceFloodedItems { get; set; }
        public List<DisplayItem> WaterClarityItems { get; set; }


        private List<int> _typeOfFloodings;

        public List<int> TypeOfFloodings
        {
            get => _typeOfFloodings;
            set
            {
                _typeOfFloodings = value;
                RaisePropertyChanged(() => TypeOfFloodings);
            }
        }


        public int TypeOfRain
        {
            get => _typeOfRain;
            set
            {
                _typeOfRain = value;
                RaisePropertyChanged(() => TypeOfRain);
            }
        }

        public int FloodDepth
        {
            get => _floodDepth;
            set
            {
                _floodDepth = value;
                RaisePropertyChanged(() => FloodDepth);
            }
        }

        public int FloodExtent
        {
            get => _floodExtent;
            set
            {
                _floodExtent = value;
                RaisePropertyChanged(() => FloodExtent);
            }
        }

        public int FrequencyOfFlood
        {
            get => _frequencyOfFlood;
            set
            {
                _frequencyOfFlood = value;
                RaisePropertyChanged(() => FrequencyOfFlood);
            }
        }

        public int TypeOfFlooding
        {
            get => _typeOfFlooding;
            set
            {
                _typeOfFlooding = value;
                RaisePropertyChanged(() => TypeOfFlooding);
            }
        }

        public int TypesOfSpaceFlooded
        {
            get => _typesOfSpaceFlooded;
            set
            {
                _typesOfSpaceFlooded = value;
                RaisePropertyChanged(() => TypesOfSpaceFlooded);
            }
        }

        public int WaterClarity
        {
            get => _waterClarity;
            set
            {
                _waterClarity = value;
                RaisePropertyChanged(() => WaterClarity);
            }
        }

        public DateTimeOffset Date
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        public bool AreYouImpacted
        {
            get => _areYouImpacted;
            set
            {
                _areYouImpacted = value;
                RaisePropertyChanged(() => AreYouImpacted);
            }
        }

        public bool AnySignOfDamage
        {
            get => _anySignOfDamage;
            set
            {
                _anySignOfDamage = value;
                RaisePropertyChanged(() => AnySignOfDamage);
            }
        }

        public bool AnySignOfObstruction
        {
            get => _anySignOfObstruction;
            set
            {
                _anySignOfObstruction = value;
                RaisePropertyChanged(() => AnySignOfObstruction);
            }
        }

        public List<string> Images
        {
            get => _images;
            set
            {
                _images = value;
                RaisePropertyChanged(() => Images);
            }
        }

        private ObservableCollection<LocalImage> _localImages;

        public ObservableCollection<LocalImage> LocalImages
        {
            get => _localImages;
            set
            {
                _localImages = value;
                RaisePropertyChanged(() => LocalImages);
            }
        }


        private bool _isAdvance;

        public bool IsAdvance
        {
            get => _isAdvance;
            set
            {
                _isAdvance = value;
                RaisePropertyChanged(() => IsAdvance);
            }
        }


        public GeoCoordinate IncidentLocation { get; set; }
        public GeoCoordinate UserLocation { get; set; }

        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearing);

        public ICommand SubmitCommand => new Command(async () => await SubmitAsync());
        public ICommand TakePictureCommand => new Command(async () => await PictureSelection());
        public ICommand CloseCommand => new Command(async (e) => await CloseAsync(e));

        private async Task CloseAsync(object o)
        {
            if (o is LocalImage localImage && LocalImages.Any())
            {
                LocalImages.Remove(localImage);
            }
            await Task.CompletedTask;
        }

        private async Task PictureSelection()
        {
            var actions = new string[] { "TakePhoto".Translate(), "PickFromGallery".Translate(), "TakeVideo".Translate() };

            var result = await MaterialDialog.Instance.SelectActionAsync(actions: actions);

            switch (result)
            {
                case 0:
                    await TakePicture();
                    break;
                case 1:
                    await PickPicture();
                    break;
                case 2:
                    await TakeVideo();
                    break;
            }
        }

        public ICommand SaveAndUploadCommand => new Command(async () => await SaveAndUploadAsync());

        private async Task SaveAndUploadAsync()
        {
            await SubmitAsync(true);
        }

        public ICommand SelectedCommand => new Command<string>(SelectAsync);
        public ICommand TypeOfRainSelectedCommand => new Command<string>(TypeOfRainSelect);

        private void TypeOfRainSelect(string obj)
        {
            TypeOfRain = (int)EnumHelper<TypeOfRain>.Parse(obj);
        }

        private void MapCurrentLocation(Xamarin.Essentials.Location currentLocation)
        {
            if (currentLocation != null)
            {
                IncidentLocation = new GeoCoordinate();
                IncidentLocation.InjectFrom(currentLocation);
                UserLocation = new GeoCoordinate();
                UserLocation.InjectFrom(currentLocation);
            }
        }
        private async Task PageAppearing()
        {
            if (Report?.Id == null)
            {
                if (IncidentLocation == null || UserLocation == null)
                {
                    var pos = await _locationService.GetCurrentLocation();
                    if (pos != null)
                    {
                        MapCurrentLocation(pos);
                    }
                }

                if (Report != null)
                    foreach (var image in Report.Images)
                        Images.Add(image);
            }
            else
            {
                if (Report?.Id != null)
                {
                    Images = Report.Images.ToList();
                    //TypeOfRain = new DisplayItem
                    //{
                    //    Text = EnumHelper<TypeOfRain>.GetEnumDescription(Report?.TypeOfRain),
                    //    Value = Report.TypeOfRain
                    //};
                    TypeOfRain = Report.TypeOfRain;
                    FloodDepth = Report.FloodDepth;
                    FloodExtent = Report.FloodExtent;
                    FrequencyOfFlood = Report.FrequencyOfFlood;
                    TypesOfSpaceFlooded = Report.TypesOfSpaceFlooded;
                    WaterClarity = Report.WaterClarity;
                    TypeOfFloodings = Report.TypeOfFloodings;
                }
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                if (navigationData is LocalIncident report1)
                    Report = report1;
                else
                    try
                    {
                        var id = navigationData.ToString();
                        var report = _incidentService.GetReportByMobileId(id);
                        if (report != null) Report = report;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                if (Report != null)
                {
                    TypeOfRain = Report.TypeOfRain;
                    FloodDepth = Report.FloodDepth;
                    TypeOfFlooding = Report.TypeOfFlooding;
                    FloodExtent = Report.FloodExtent;
                    FrequencyOfFlood = Report.FrequencyOfFlood;
                    TypesOfSpaceFlooded = Report.TypesOfSpaceFlooded;
                    WaterClarity = Report.WaterClarity;
                    AreYouImpacted = Report.AreYouImpacted;
                    AnySignOfDamage = Report.AnySignOfDamage;
                    AnySignOfObstruction = Report.AnySignOfObstruction;
                    Date = Report.Date;
                    Images = Report.Images.ToList();
                    if (Report.LocalImages != null)
                        foreach (var reportLocalImage in Report.LocalImages)
                        {
                            LocalImages.Add(reportLocalImage);
                        }
                }

                Analytics.TrackEvent("IncidentViewed");
            }

            return base.InitializeAsync(navigationData);
        }

        private void SelectAsync(string str)
        {
            //TypeOfFlooding = (int) EnumHelper<TypeOfFlooding>.Parse(str);
        }

        private async Task TakeVideo()
        {
            var file = await _cameraService.TakeVideo();
            var image = new LocalImage
            {
                Id = Guid.NewGuid(),
                Path = file,
                IsUploaded = false,
                IsVideo = true
            };
            LocalImages.Add(image);
            await Task.CompletedTask;
        }

        private async Task PickPicture()
        {
            var pic = await _cameraService.PickPicture();
            var image = new LocalImage
            {
                Id = Guid.NewGuid(),
                Path = pic,
                IsUploaded = false
            };
            LocalImages.Add(image);
            //_images.Add(image);
            //Images = _images;
            await Task.CompletedTask;
        }

        private async Task TakePicture()
        {
            var pic = await _cameraService.TakePicture();
            var image = new LocalImage
            {
                Id = Guid.NewGuid(),
                Path = pic,
                IsUploaded = false
            };
            //_images.Add(image);
            LocalImages.Add(image);
            await Task.CompletedTask;
        }


        private async Task SubmitAsync(bool isUpload = false)
        {
            if (TypeOfRain == -1 || TypeOfFloodings.Count == 0)
            {
                UserDialogs.Instance.Toast("IncidentSavingWarningMessage".Translate());
                return;
            }
            await SetBusyAsync(async () =>
                {
                    var report = new LocalIncident();

                    if (Report != null) report.InjectFrom((object)Report);

                    report.FloodDepth = FloodDepth;
                    report.FloodExtent = FloodExtent;
                    report.FrequencyOfFlood = FrequencyOfFlood;
                    report.TypeOfRain = TypeOfRain;
                    report.TypesOfSpaceFlooded = TypesOfSpaceFlooded;
                    report.WaterClarity = WaterClarity;
                    report.AnySignOfDamage = AnySignOfDamage;
                    report.AnySignOfObstruction = AnySignOfObstruction;
                    report.AreYouImpacted = AreYouImpacted;
                    report.Date = Date;
                    report.TypeOfFloodings = TypeOfFloodings;
                    report.LocalImages = LocalImages.ToList();

                    if (Images != null)
                    {
                        report.Images.Clear();
                        foreach (var image in Images)
                            report.Images.Add(image);
                    }


                    bool result = false;
                    if (report.Id == null)
                    {
                        if (IncidentLocation == null)
                        {
                            var pos = await _locationService.GetLastKnownLocationAsync();
                            MapCurrentLocation(pos);
                        }

                        if (IncidentLocation != null)
                        {
                            report.IncidentLocation = IncidentLocation;
                            report.UserLocation = UserLocation;
                            result = await _incidentService.Create(report,
                                IsAdvance ? "IncidentCreatedAdvance" : "IncidentCreatedSimple");
                        }
                    }
                    else
                    {
                        result = await _incidentService.Update(report);
                    }

                    if (result)
                    {
                        if (IsConnectedToInternet)
                        {
                            if (isUpload)
                            {
                                var output = await _incidentService.Upload(report);
                                if (output.Success)
                                {
                                    await _navigationService.SetMainPage<MasterPage>();
                                    await _navigationService.SetDetailPageAsync(typeof(MapPage));
                                    return;
                                }
                            }
                            await _navigationService.SetMainPage<MasterPage>();
                            await _navigationService.SetDetailPageAsync(typeof(IncidentCurrentPage));
                            return;
                        }

                        await _navigationService.SetMainPage<MasterPage>();
                        await _navigationService.SetDetailPageAsync(typeof(MainPage));
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("NotAbleToSave".Translate());
                    }
                }
                , "SavingIncident".Translate());
        }
    }
}