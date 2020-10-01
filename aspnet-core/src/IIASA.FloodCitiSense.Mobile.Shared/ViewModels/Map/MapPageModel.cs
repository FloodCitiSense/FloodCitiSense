//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapPageModel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   MapPageModel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Abp.Application.Services.Dto;
using Acr.UserDialogs;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Commands;
using IIASA.FloodCitiSense.Mobile.Core.Controls;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Models.Common;
using IIASA.FloodCitiSense.Mobile.Core.Services.Location;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using IIASA.FloodCitiSense.Mobile.Core.Services.Observation;
using IIASA.FloodCitiSense.Mobile.Core.Services.Observation.Dto;
using IIASA.FloodCitiSense.Mobile.Core.Services.Report;
using IIASA.FloodCitiSense.Views.Incident;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.LiteDB;
using NetTopologySuite.Features;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;
using PinType = IIASA.FloodCitiSense.Mobile.Core.Controls.PinType;

namespace IIASA.FloodCitiSense.ViewModels.Map
{
    public class MapPageModel : XamarinViewModel
    {
        private static bool _isMapShown;
        private readonly IDataStorageManager _dataStorageManager;
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IIncidentsAppService _incidentsAppService;
        private readonly IIncidentService _incidentService;
        private readonly ILocationsAppService _locationsAppService;
        private readonly ILocationService _locationService;
        private readonly INavigationService _navigationService;

        private readonly IObservationService _observationService;
        private string _aggregateInterval;

        private bool _animated = true;
        private CameraUpdate _cameraUpdateBehavior;

        private PlotModel _chartModel;

        private DateTime _createdTime;
        private CameraPosition _currentCameraPosition;
        private string _dateTimeFormat;


        private LocalIncident _detailReport;
        private DateTime? _endDate;

        private List<Entry> _entries;


        private DateTime _fromDateTime;
        private List<string> _graphFilterOptions;
        private string _headingTitle;
        private int _infoWindowClickedCount;
        private int _infoWindowLongClickedCount;

        private bool _isChartViewVisible;


        /// <summary>
        ///     The is clustering enabled
        /// </summary>
        private bool _isClusteringEnabled;

        private bool _isDailyChecked;

        private bool _isDailyOptionEnabled;

        private bool _isDateFilter;

        private bool _isDetailViewVisible;

        private bool _isFilterViewVisible;
        private bool _isHourlyChecked;
        private bool _isHourlyOptionEnabled;

        private bool _isLastWeek;


        private bool _isLastDay;

        private bool _isOtherReport = true;

        private bool _isPageShown;

        private bool _isSensor = true;

        private bool _isShowAll;

        private bool _isYourReport = true;
        private int _labelingCount;
        private int _mainSensorFilterSelectedIndex;

        private int _mapClickedCount;

        private string _moreText;

        private Pin _pin;
        private int _pinClickedCount;
        private string _pinDragStatus;

        private string _rainType;

        private ImageSource _rainTypeImage;
        private int _selectedPinChangedCount;
        private int _sensorFilterSelectedIndex;
        private string _sensorId;
        private DateTime? _startDate;

        private DateTime _toDate;

        private ObservableCollection<ImageItem> _typeOfFlooding;

        private string _userName;
        private MapSpan _visibleRegion;
        private bool isHourlyEnabled;
        private bool _isGraphDateFilter;
        private DateTime _graphFromDate;
        private bool _isGraphDatePickerEnabled;
        private bool _isGraphOneDayFilter;
        private bool _isGraph3DaysFilter;
        private bool _isGrapLasthWeekFilter;
        private bool _showEmptyGraphMessage;
        private bool _isLayerViewVisible;
        private bool _isLegendViewVisible;
        private readonly IDictionary<double, List<double>> _listOfPinnedLocations = new Dictionary<double, List<double>>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapPageModel" /> class.
        /// </summary>
        /// <param name="observationService">
        ///     The observation service.
        /// </param>
        /// <param name="locationService">
        ///     The location service.
        /// </param>
        /// <param name="incidentService"></param>
        /// <param name="incidentsAppService"></param>
        /// <param name="locationsAppService"></param>
        /// <param name="dataStorageManager"></param>
        /// <param name="navigationService"></param>
        public MapPageModel(IObservationService observationService,
            ILocationService locationService,
            IIncidentService incidentService,
            IIncidentsAppService incidentsAppService,
            ILocationsAppService locationsAppService,
            IDataStorageManager dataStorageManager,
            IAccessTokenManager accessTokenManager,
            INavigationService navigationService)
        {
            _observationService = observationService;
            _locationService = locationService;
            _incidentService = incidentService;
            _incidentsAppService = incidentsAppService;
            _locationsAppService = locationsAppService;
            _dataStorageManager = dataStorageManager;
            _accessTokenManager = accessTokenManager;
            _navigationService = navigationService;

            IsShowingUser = true;
            IsClusteringEnabled = false;
            IsShowAll = true;
            _isPageShown = false;
            _isGraphDatePickerEnabled = false;

            GraphFilterOptions = new List<string>
            {
                "ShowYesterday".Translate(),
                "FilterLastWeek".Translate(),
                "Showlastweek".Translate()
            };
            SensorFilterSelectedIndex = -1;
            IsDailyChecked = true;
            FromDate = GetFromDate(DateTime.Today);
            ToDate = GetToDate(DateTime.Today);
            IsFilterViewVisible = false;
            IsLayerViewVisible = false;
            IsLegendViewVisible = false;
        }

        public bool ShowEmptyGraphMessage
        {
            get => _showEmptyGraphMessage;
            set
            {
                _showEmptyGraphMessage = value;
                RaisePropertyChanged(() => ShowEmptyGraphMessage);
            }
        }

        public string HeadingTitle
        {
            get => _headingTitle;
            set
            {
                _headingTitle = value;
                RaisePropertyChanged(() => HeadingTitle);
            }
        }

        public string MoreText
        {
            get => _moreText;
            set
            {
                _moreText = value;
                RaisePropertyChanged(() => MoreText);
            }
        }

        public int MapClickedCount
        {
            get => _mapClickedCount;
            set
            {
                _mapClickedCount = value;
                RaisePropertyChanged(() => MapClickedCount);
            }
        }

        public int PinClickedCount
        {
            get => _pinClickedCount;
            set
            {
                _pinClickedCount = value;
                RaisePropertyChanged(() => PinClickedCount);
            }
        }

        public bool IsHourlyOptionEnabled
        {
            get => _isHourlyOptionEnabled;
            set
            {
                _isHourlyOptionEnabled = value;
                RaisePropertyChanged(() => IsHourlyOptionEnabled);
            }
        }

        public bool IsDailyOptionEnabled
        {
            get => _isDailyOptionEnabled;
            set
            {
                _isDailyOptionEnabled = value;
                RaisePropertyChanged(() => IsDailyOptionEnabled);
            }
        }

        public CameraPosition CurrentCameraPosition
        {
            get => _currentCameraPosition;
            set
            {
                _currentCameraPosition = value;
                RaisePropertyChanged(() => CurrentCameraPosition);
                _dataStorageManager.StoreAsync(DataStorageKey.CurrentCameraPosition, CurrentCameraPosition);
            }
        }

        public CameraUpdate CameraUpdateBehavior
        {
            get => _cameraUpdateBehavior;
            set
            {
                _cameraUpdateBehavior = value;
                RaisePropertyChanged(() => CameraUpdateBehavior);
            }
        }

        public string SensorId
        {
            get => _sensorId;
            set

            {
                _sensorId = value;
                RaisePropertyChanged(() => SensorId);
            }
        }

        public int SelectedPinChangedCount
        {
            get => _selectedPinChangedCount;
            set
            {
                _selectedPinChangedCount = value;
                RaisePropertyChanged(() => SelectedPinChangedCount);
            }
        }

        public int InfoWindowClickedCount
        {
            get => _infoWindowClickedCount;
            set
            {
                _infoWindowClickedCount = value;
                RaisePropertyChanged(() => InfoWindowClickedCount);
            }
        }

        public int InfoWindowLongClickedCount
        {
            get => _infoWindowLongClickedCount;
            set
            {
                _infoWindowLongClickedCount = value;
                RaisePropertyChanged(() => InfoWindowLongClickedCount);
            }
        }

        public string PinDragStatus
        {
            get => _pinDragStatus;
            set
            {
                _pinDragStatus = value;
                RaisePropertyChanged(() => PinDragStatus);
            }
        }

        public Pin Pin
        {
            get => _pin;
            set
            {
                _pin = value;
                RaisePropertyChanged(() => Pin);
            }
        }

        public bool IsDetailViewVisible
        {
            get => _isDetailViewVisible;
            set
            {
                _isDetailViewVisible = value;
                RaisePropertyChanged(() => IsDetailViewVisible);
            }
        }

        public LocalIncident DetailReport
        {
            get => _detailReport;
            set
            {
                _detailReport = value;
                RaisePropertyChanged(() => DetailReport);
            }
        }

        public DateTime CreatedTime
        {
            get => _createdTime;
            set
            {
                _createdTime = value;
                RaisePropertyChanged(() => CreatedTime);
            }
        }

        public string Username
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged(() => Username);
            }
        }


        public string RainType
        {
            get => _rainType;
            set
            {
                _rainType = value;
                RaisePropertyChanged(() => RainType);
            }
        }

        public ImageSource RainTypeImage
        {
            get => _rainTypeImage;
            set
            {
                _rainTypeImage = value;
                RaisePropertyChanged(() => RainTypeImage);
            }
        }

        public ObservableCollection<ImageItem> TypeOfFlooding
        {
            get => _typeOfFlooding;
            set
            {
                _typeOfFlooding = value;
                RaisePropertyChanged(() => TypeOfFlooding);
            }
        }

        public PlotModel ChartModel
        {
            get => _chartModel;
            set
            {
                _chartModel = value;
                RaisePropertyChanged(() => ChartModel);
            }
        }


        public ObservableCollection<Pin> Pins { get; set; }

        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(
            args =>
            {
                MapClickedCount++;
                if (IsFilterViewVisible || IsLayerViewVisible || IsLegendViewVisible)
                {
                    IsFilterViewVisible = false;
                    IsLayerViewVisible = false;
                    IsLegendViewVisible = false;
                }
            });

        public Command<PinClickedEventArgs> PinClickedCommand => new Command<PinClickedEventArgs>(async args =>
        {
            PinClickedCount++;
            Pin = args.Pin;
            if (Pin != null)
            {
                var pinInfo = Pin.GetPinInfo();
                //if (pinInfo != null && pinInfo.IsLocal)
                //    await _navigationService.SetMainPage<IncidentMainPage>(pinInfo.IncidentId);

                if (pinInfo?.IsDevice == true)
                    try
                    {
                        await SetBusyAsync(async () =>
                        {
                            SensorId = pinInfo.DeviceId;
                            SensorFilterSelectedIndex = 2;
                            IsGrapLasthWeekFilter = true;
                            _dateTimeFormat = "{0:dd/MM}";
                            _labelingCount = 1;
                            //await SensorFilterClicked("IsLast7Days");
                            SensorFilterChanged();
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
            }
        });

        public Command NavigateToDetailsPage => new Command(NavigateToDetails);

        public Command<SelectedPinChangedEventArgs> SelectedPinChangedCommand =>
            new Command<SelectedPinChangedEventArgs>(
                args =>
                {
                    SelectedPinChangedCount++;
                    Pin = args.SelectedPin;
                });

        public Command<InfoWindowClickedEventArgs> InfoWindowClickedCommand => new Command<InfoWindowClickedEventArgs>(
            async args => await InfoWindowClick(args));

        private async Task InfoWindowClick(InfoWindowClickedEventArgs args)
        {
            InfoWindowClickedCount++;
            Pin = args.Pin;
            try
            {
                if (Pin != null)
                {
                    var pinInfo = Pin.GetPinInfo();
                    if (pinInfo != null && !pinInfo.IsDevice)
                        await SetBusyAsync(async () =>
                        {
                            MoreText = pinInfo.IsLocal ? "EditOrMore".Translate() : "More".Translate();

                            if (pinInfo.IsLocal)
                            {
                                var report = _incidentService.GetReportByMobileId(pinInfo.MobileId);
                                if (report != null)
                                {
                                    RainType = EnumHelper<TypeOfRain>.GetEnumDescription(
                                        EnumHelper<TypeOfRain>.ParseInt(report.TypeOfRain));
                                    RainTypeImage = EnumHelper<TypeOfRain>.GetImage(report.TypeOfRain);
                                    CreatedTime = report.Date.DateTime;
                                    Username = "You".Translate();
                                    TypeOfFlooding = new ObservableCollection<ImageItem>();
                                    foreach (var incidentFloodType in report.TypeOfFloodings)
                                        TypeOfFlooding.Add(new ImageItem
                                        {
                                            ImageSource =
                                                EnumHelper<TypeOfFlood>.GetImage(incidentFloodType),
                                            Tooltip = EnumHelper<TypeOfFlood>.GetEnumDescription(
                                                EnumHelper<TypeOfFlood>.ParseInt(incidentFloodType))
                                        });

                                    IsDetailViewVisible = true;
                                }
                            }
                            else
                            {
                                var incidentForView =
                                    await _incidentsAppService.GetIncidentById(new EntityDto(pinInfo.IncidentId));
                                if (incidentForView != null)
                                {
                                    RainType = incidentForView.Incident.TypeOfRain.ToString().Translate();
                                    RainTypeImage =
                                        EnumHelper<TypeOfRain>.GetImage(incidentForView.Incident.TypeOfRain);
                                    CreatedTime = incidentForView.Incident.Date.DateTime;
                                    Username = incidentForView.Username;
                                    TypeOfFlooding = new ObservableCollection<ImageItem>();
                                    foreach (var incidentFloodType in incidentForView.Incident.FloodTypes)
                                        TypeOfFlooding.Add(new ImageItem
                                        {
                                            ImageSource =
                                                EnumHelper<TypeOfFlood>.GetImage(incidentFloodType.TypeOfFlood),
                                            Tooltip = EnumHelper<TypeOfFlood>.GetEnumDescription(incidentFloodType
                                                .TypeOfFlood)
                                        });

                                    IsDetailViewVisible = true;
                                }
                            }
                        });

                    if (pinInfo != null && pinInfo.IsDevice) IsChartViewVisible = true;
                    //_navigationService.SetMainPage<IncidentDetailsPage>(pinInfo.IncidentId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Command<InfoWindowLongClickedEventArgs> InfoWindowLongClickedCommand =>
            new Command<InfoWindowLongClickedEventArgs>(
                args =>
                {
                    InfoWindowLongClickedCount++;
                    Pin = args.Pin;
                });

        public Command<PinDragEventArgs> PinDragStartCommand => new Command<PinDragEventArgs>(
            args =>
            {
                PinDragStatus = "Start";
                Pin = args.Pin;
            });

        public Command<PinDragEventArgs> PinDraggingCommand => new Command<PinDragEventArgs>(
            args =>
            {
                PinDragStatus = "Dragging";
                Pin = args.Pin;
            });

        public Command<PinDragEventArgs> PinDragEndCommand => new Command<PinDragEventArgs>(
            args =>
            {
                PinDragStatus = "End";
                Pin = args.Pin;
            });

        public ICommand PinSelectedCommand => new Command(async () => await PinSelected());
        public ICommand FromCommand => new Command(async () => await FromClickedAsync());
        public ICommand GraphFromCommand => new Command(async () => await GraphFromClickedAsync());
        public ICommand FilterSelected => new Command(async e => await FilterClicked(e));
        public ICommand GraphFilterSelected => new Command(async e => GraphFilterClicked(e));

        private void GraphFilterClicked(object filter)
        {
            var filterText = filter.ToString();
        }

        public Command<int> SensorFilterSelected => new Command<int>(async e => SensorFilterChanged());

        public Command<bool> HourlyCommand => new Command<bool>(async e => await HourlyClicked(e));

        private async Task HourlyClicked(bool e)
        {
            if (e)
            {
                IsDailyChecked = false;
                SensorFilterChanged();
            }
        }

        public Command<bool> DailyCommand => new Command<bool>(async e => await DailyClicked(e));

        private async Task DailyClicked(bool e)
        {
            if (e)
            {
                IsHourlyChecked = false;
                SensorFilterChanged();
            }
        }

        public ICommand DataCommand => HttpRequestCommand.Create(FilterSelectedClickedAsync);
        public ICommand YourReportCommand => HttpRequestCommand.Create(FilterSelectedClickedAsync);

        public ICommand OtherReportCommand => new Command(async e => await FilterSelectedClickedAsync());

        public ICommand RainfallCommand => new Command(async e => await FilterSelectedClickedAsync());

        public ICommand ToCommand => new Command(async () => await ToClicked());
        public ICommand CloseDetailsCommand => new Command(CloseDetailsClicked);

        public ICommand CloseChartCommand => new Command(CloseChartClicked);

        public bool IsChartViewVisible
        {
            get => _isChartViewVisible;
            set
            {
                _isChartViewVisible = value;
                RaisePropertyChanged(() => IsChartViewVisible);
            }
        }

        public bool IsFilterViewVisible
        {
            get => _isFilterViewVisible;
            set
            {
                _isFilterViewVisible = value;
                RaisePropertyChanged(() => IsFilterViewVisible);
            }
        }

        public bool IsLayerViewVisible
        {
            get => _isLayerViewVisible;
            set
            {
                _isLayerViewVisible = value;
                RaisePropertyChanged(() => IsLayerViewVisible);
            }
        }

        public bool IsLegendViewVisible
        {
            get => _isLegendViewVisible;
            set
            {
                _isLegendViewVisible = value;
                RaisePropertyChanged(() => IsLegendViewVisible);
            }
        }

        public bool Animated
        {
            get => _animated;
            set
            {
                _animated = value;
                RaisePropertyChanged(() => Animated);
            }
        }

        public bool IsYourReport
        {
            get => _isYourReport;
            set
            {
                _isYourReport = value;
                RaisePropertyChanged(() => IsYourReport);
            }
        }

        public bool IsOtherReport
        {
            get => _isOtherReport;
            set
            {
                _isOtherReport = value;
                RaisePropertyChanged(() => IsOtherReport);
            }
        }

        public bool IsSensor
        {
            get => _isSensor;
            set
            {
                _isSensor = value;
                RaisePropertyChanged(() => IsSensor);
            }
        }

        public bool IsShowAll
        {
            get => _isShowAll;
            set
            {
                _isShowAll = value;
                RaisePropertyChanged(() => IsShowAll);
            }
        }

        public bool IsLastDay
        {
            get => _isLastDay;
            set
            {
                _isLastDay = value;
                RaisePropertyChanged(() => IsLastDay);
            }
        }

        public int SensorFilterSelectedIndex
        {
            get => _sensorFilterSelectedIndex;
            set
            {
                _sensorFilterSelectedIndex = value;
                RaisePropertyChanged(() => SensorFilterSelectedIndex);
            }
        }

        public bool IsLastWeek
        {
            get => _isLastWeek;
            set
            {
                _isLastWeek = value;
                RaisePropertyChanged(() => IsLastWeek);
            }
        }


        public int MainSensorFilterSelectedIndex
        {
            get => _mainSensorFilterSelectedIndex;
            set
            {
                _mainSensorFilterSelectedIndex = value;
                RaisePropertyChanged(() => MainSensorFilterSelectedIndex);
            }
        }

        public bool IsDateFilter
        {
            get => _isDateFilter;
            set
            {
                _isDateFilter = value;
                RaisePropertyChanged(() => IsDateFilter);
            }
        }

        public bool IsGraphOneDayFilter
        {
            get => _isGraphOneDayFilter;
            set
            {
                _isGraphOneDayFilter = value;
                RaisePropertyChanged(() => IsGraphOneDayFilter);
                if (_isGraphOneDayFilter)
                {
                    SensorFilterSelectedIndex = 0;
                    SensorFilterChanged();
                }
            }
        }
        public bool IsGraph3DaysFilter
        {
            get => _isGraph3DaysFilter;
            set
            {
                _isGraph3DaysFilter = value;
                RaisePropertyChanged(() => IsGraph3DaysFilter);
                if (_isGraph3DaysFilter)
                {
                    SensorFilterSelectedIndex = 1;
                    SensorFilterChanged();
                }
            }
        }
        public bool IsGrapLasthWeekFilter
        {
            get => _isGrapLasthWeekFilter;
            set
            {
                _isGrapLasthWeekFilter = value;
                RaisePropertyChanged(() => IsGrapLasthWeekFilter);
                if (_isGrapLasthWeekFilter)
                {
                    SensorFilterSelectedIndex = 2;
                    SensorFilterChanged();
                }
            }
        }
        public bool IsGraphDateFilter
        {
            get => _isGraphDateFilter;
            set
            {
                _isGraphDateFilter = value;
                RaisePropertyChanged(() => IsGraphDateFilter);
                if (_isGraphDateFilter)
                {
                    SensorFilterSelectedIndex = 3;
                    SensorFilterChanged();
                }

            }
        }
        public DateTime FromDate
        {
            get => _fromDateTime;
            set
            {
                _fromDateTime = value;
                RaisePropertyChanged(() => FromDate);
            }
        }
        public DateTime GraphFromDate
        {
            get => _graphFromDate;
            set
            {
                _graphFromDate = value;
                RaisePropertyChanged(() => GraphFromDate);
                _startDate = GraphFromDate;
                _endDate = _startDate.Value.AddDays(7);

                GetDeviceData(_startDate.Value, _endDate.Value, 7);
            }
        }
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                RaisePropertyChanged(() => ToDate);
            }
        }

        public bool IsHourlyChecked
        {
            get => _isHourlyChecked;
            set
            {
                _isHourlyChecked = value;
                RaisePropertyChanged(() => IsHourlyChecked);
            }
        }


        public List<string> GraphFilterOptions
        {
            get => _graphFilterOptions;
            set
            {
                _graphFilterOptions = value;
                RaisePropertyChanged(() => GraphFilterOptions);
            }
        }

        public bool IsHourlyEnabled
        {
            get => isHourlyEnabled;
            set
            {
                isHourlyEnabled = value;
                RaisePropertyChanged(() => IsHourlyEnabled);
            }
        }

        public bool IsDailyChecked
        {
            get => _isDailyChecked;
            set
            {
                _isDailyChecked = value;
                RaisePropertyChanged(() => IsDailyChecked);
            }
        }
        public bool IsGraphDatePickerEnabled
        {
            get => _isGraphDatePickerEnabled;
            set
            {
                _isGraphDatePickerEnabled = value;
                RaisePropertyChanged(() => IsGraphDatePickerEnabled);
            }
        }

        public MoveToRegionRequest Request { get; } = new MoveToRegionRequest();


        public Command MoveToCurrentLocationCommand => new Command(async () =>
        {
            var position = await _locationService.GetLastKnownLocationAsync();
            Request.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(position.Latitude, position.Longitude),
                    Distance.FromKilometers(2)),
                Animated);
        });


        /// <summary>
        ///     Gets or sets a value indicating whether this instance is clustering enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is clustering enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsClusteringEnabled
        {
            get => _isClusteringEnabled;
            set
            {
                _isClusteringEnabled = value;
                RaisePropertyChanged(() => IsClusteringEnabled);
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether this instance is showing user.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is showing user; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowingUser { get; set; }


        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);

        public ICommand RefreshCommand => HttpRequestCommand.Create(RefreshClicked);


        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        /// <value>
        ///     The position.
        /// </value>
        public ObservableCollection<Position> Position { get; set; }

        public List<Entry> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                RaisePropertyChanged(() => Entries);
            }
        }

        private PlotModel CreateChart(AllDevices data, string dateTimeFormat, int expectedRows, DateTime startDate, DateTime endDate)
        {
            if (data == null) return null;
            var plotModel = new PlotModel();
            string xAxisTitle = "";
            if (_startDate != null)
            {
                xAxisTitle = IsHourlyChecked ? _startDate.Value.Date.ToString("dd/MM/yyyy") : "Dates".Translate();
            }
            var xaxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = xAxisTitle,
                FontSize = 8
            };

            var yaxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = 0
            };

            var areaSeries1 = new ColumnSeries();
            plotModel.Axes.Add(xaxis);
            plotModel.Axes.Add(yaxis);
            plotModel.Series.Add(areaSeries1);


            if (data?.ObservationCollection != null)
            {
                ShowEmptyGraphMessage = false;
                foreach (var member in data.ObservationCollection?.Member)
                {
                    if (member.Result.DataArray.Values.Count == 0 )
                    {
                        ShowEmptyGraphMessage = true;
                    }
                    else
                    {
                        PlotModel(dateTimeFormat, member.Result.DataArray.Values, plotModel, startDate, endDate);
                    }
                }
            }
            else
            {
                ShowEmptyGraphMessage = true;
            }
            IsChartViewVisible = true;
            return plotModel;
        }

        private static void PlotModel(string dateTimeFormat, List<List<string>> dataArrayValues, PlotModel plotModel,
            DateTime startDate, DateTime endDate)
        {
            var selectedDate = startDate;
            while (selectedDate < endDate)
            {
                var selectedDataArrayValue = dataArrayValues.FirstOrDefault(x => AreDatesSame(x[0], selectedDate));
                double value = 0;
                if (selectedDataArrayValue != null)
                {
                    value = double.Parse(selectedDataArrayValue[1], CultureInfo.InvariantCulture);
                }

                (plotModel.Series[0] as ColumnSeries)?.Items.Add(new ColumnItem(value));

                var dataTime = string.Format(dateTimeFormat, selectedDate);
                (plotModel.Axes[0] as CategoryAxis)?.Labels.Add(dataTime);

                selectedDate = selectedDate.AddDays(1);
            }
        }

        private static bool AreDatesSame(string dateTimeString, DateTime selectedDate)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            return dateTime.Day == selectedDate.Day && dateTime.Month == selectedDate.Month && dateTime.Year == selectedDate.Year;
        }


        private async void NavigateToDetails()
        {
            await SetBusyAsync(async () =>
            {
                var pinInfo = Pin.GetPinInfo();
                if (pinInfo.IsLocal)
                    await _navigationService.SetMainPage<IncidentMainPage>(pinInfo.MobileId);
                else if (pinInfo.IncidentId > 0 && !pinInfo.IsDevice)
                    await _navigationService.SetMainPage<IncidentDetailsPage>(pinInfo.IncidentId);
            });
        }

        private void CloseDetailsClicked()
        {
            IsDetailViewVisible = false;
        }

        private void CloseChartClicked()
        {
            IsChartViewVisible = false;
        }

        private async Task RefreshClicked()
        {
            Barrel.Current.EmptyAll();
            await UpdatePinsAsync();
        }

        private async Task FilterClicked(object filter)
        {
            var filterText = filter.ToString();
            if (!string.IsNullOrEmpty(filterText))
            {
                ClearAllPins();
                IsShowAll = false;
                IsLastDay = false;
                IsLastWeek = false;
                IsDateFilter = false;
                FromDate = DateTime.Today;
                ToDate = DateTime.Today;
                switch (filterText)
                {
                    case "IsShowAll":
                        IsShowAll = true;
                        await GetAllData();
                        break;
                    case "IsLastDay":
                        IsLastDay = true;
                        await GetLastDayLocation();
                        break;
                    case "IsLastWeek":
                        IsLastWeek = true;
                        await GetLastWeekLocation();
                        break;
                    case "IsDateFilter":
                        IsDateFilter = true;
                        await GetFilterLocation();
                        break;
                }
            }
        }

        private void SensorFilterChanged()
        {
            int expectedRows = 0;
            if (IsDailyChecked)
            {
                _aggregateInterval = "P1DT";
                _dateTimeFormat = "{0:dd/MM}";
            }
            else
            {
                _aggregateInterval = "PT1H";
                _dateTimeFormat = "{0:HH}";
            }

            switch (SensorFilterSelectedIndex)
            {
                case 0:
                    expectedRows = IsDailyChecked ? 1 : 24;
                    IsGraphDatePickerEnabled = false;
                    IsGraphDateFilter = false;
                    IsHourlyEnabled = true;
                    IsGraph3DaysFilter = false;
                    IsGrapLasthWeekFilter = false;
                    IsGraphDateFilter = false;
                    _startDate = DateTime.Today.AddDays(-1);
                    _endDate = DateTime.Today;
                    break;
                case 1:
                    expectedRows = 3;
                    IsGraphOneDayFilter = false;
                    IsGrapLasthWeekFilter = false;
                    IsGraphDateFilter = false;
                    IsGraphDatePickerEnabled = false;
                    IsGraphDateFilter = false;
                    IsHourlyEnabled = false;
                    IsHourlyChecked = false;
                    IsDailyChecked = true;
                    _startDate = DateTime.Today.AddDays(-3);
                    _endDate = DateTime.Today;
                    break;
                case 2:
                    expectedRows = 7;
                    IsGraphOneDayFilter = false;
                    IsGraph3DaysFilter = false;
                    IsGraphDateFilter = false;
                    IsGraphDatePickerEnabled = false;
                    IsGraphDateFilter = false;
                    IsHourlyEnabled = false;
                    IsHourlyChecked = false;
                    IsDailyChecked = true;
                    _startDate = DateTime.Today.AddDays(-7);
                    _endDate = DateTime.Today;
                    break;
                case 3:

                    IsGraphOneDayFilter = false;
                    IsGraph3DaysFilter = false;
                    IsGrapLasthWeekFilter = false;
                    IsGraphDatePickerEnabled = true;
                    IsHourlyEnabled = false;
                    IsHourlyChecked = false;
                    IsDailyChecked = true;

                    return;

            }


            if (_startDate != null && _endDate != null)
                GetDeviceData(_startDate.Value, _endDate.Value, expectedRows);
        }

        private async void GetDeviceData(DateTime startDate, DateTime endDate, int expectedRows)
        {
            await SetBusyAsync(async () =>
            {
                var data = await _observationService.GetDeviceDataAsync(GetFromDate(startDate), // a day is subtracted to allign with the webapp
                    GetToDate(endDate), SensorId, _aggregateInterval);
                ChartModel = CreateChart(data, _dateTimeFormat, expectedRows, startDate, endDate);
            });
        }

        private async Task GetFilterLocation()
        {
            await SetBusyAsync(async () =>
            {
                try
                {
                    if (IsDateFilter && FromDate != DateTime.MinValue && ToDate != DateTime.MinValue)
                    {
                        var incidents = await _locationsAppService.GetFilteredIncident(new FilterInput
                        {
                            From = GetFromDate(FromDate.AddDays(-1)),
                            To = GetToDate(ToDate)
                        });
                        foreach (var item in incidents.Items)
                        {
                            var location = item?.Location;
                            if (location != null)
                            {
                                var pin = CreateIncidentPin(location,
                                    item.IsCurrentUserLocation ? PinType.CurrentUser : PinType.OtherUser);

                                AddPin(pin);
                            }
                        }

                        UserDialogs.Instance.Toast($"{incidents.TotalCount} {"IncidentsFound".Translate()}");
                    }
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                    //throw;
                }
            });
        }

        private async Task FromClickedAsync()
        {
            var config = new DatePromptConfig();
            if (ToDate != DateTime.MinValue)
            {
                config = new DatePromptConfig { MaximumDate = ToDate };
            }

            var fromDate = await UserDialogs.Instance.DatePromptAsync(config);

            if (fromDate.Ok)
            {
                FromDate = fromDate.SelectedDate;
                await GetFilterLocation();
            }
        }
        private async Task GraphFromClickedAsync()
        {
            var config = new DatePromptConfig();
            if (ToDate != DateTime.MinValue)
            {
                config = new DatePromptConfig { MaximumDate = ToDate };
            }

            var fromDate = await UserDialogs.Instance.DatePromptAsync(config);

            if (fromDate.Ok)
            {
                GraphFromDate = fromDate.SelectedDate;

            }
        }
        private async Task GetLastWeekLocation()
        {
            await SetBusyAsync(async () =>
            {
                try
                {
                    var incidents = await _locationsAppService.GetFilteredIncident(new FilterInput
                    {
                        LastTime = GetFromDate(DateTime.Today.AddDays(-7))
                    });
                    foreach (var item in incidents.Items)
                    {
                        var location = item?.Location;
                        if (location != null)
                        {
                            var pin = CreateIncidentPin(location,
                                item.IsCurrentUserLocation ? PinType.CurrentUser : PinType.OtherUser);

                            AddPin(pin);
                        }
                    }

                    UserDialogs.Instance.Toast($"{incidents.TotalCount} {"IncidentsFound".Translate()}");
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                    //throw;
                }
            });
        }

        private async Task GetLastDayLocation()
        {
            await SetBusyAsync(async () =>
            {
                try
                {
                    var incidents = await _locationsAppService.GetFilteredIncident(new FilterInput
                    {
                        LastTime = GetFromDate(DateTime.Today.AddDays(-1))
                    });
                    foreach (var item in incidents.Items)
                    {
                        var location = item?.Location;
                        if (location != null)
                        {
                            var pin = CreateIncidentPin(location,
                                item.IsCurrentUserLocation ? PinType.CurrentUser : PinType.OtherUser);

                            AddPin(pin);
                        }
                    }

                    UserDialogs.Instance.Toast($"{incidents.TotalCount} {"IncidentsFound".Translate()}");
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                    //throw;
                }
            });
        }

        private async Task UpdatePinsAsync()
        {
            ClearAllPins();

            if (IsYourReport)
                await SetBusyAsync(async () => await GetAllCurrentUserLocation(), "GetAllLocalLocation".Translate());

            if (IsOtherReport)
                await SetBusyAsync(async () => await GetAllOtherIncidents(), "GetAllIncidents".Translate());

            if (IsSensor)
                await SetBusyAsync(async () => await GetAllDeviceLocation(), "GetAllDeviceLocation".Translate());

            UserDialogs.Instance.HideLoading();
        }

        private async Task FilterSelectedClickedAsync()
        {
            await UpdatePinsAsync();
        }

        private async Task ToClicked()
        {
            var config = new DatePromptConfig();
            if (FromDate != DateTime.MinValue)
            {
                config = new DatePromptConfig { MinimumDate = FromDate };
            }
            var toDate = await UserDialogs.Instance.DatePromptAsync(config);

            if (toDate.Ok)
            {
                ToDate = GetToDate(toDate.SelectedDate);
                await GetFilterLocation();
            }
        }

        private Task PinSelected()
        {
            return Task.CompletedTask;
        }


        /// <summary>
        ///     Gets all device location.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task GetAllDeviceLocation()
        {
            try
            {
                var devices = await _observationService.GetAllDeviceLocation();
                //var image = await ImageSource.FromResource(AssetsHelper.GetImageNamespace("sensor_primary.png"),
                //    typeof(AssetsHelper).GetTypeInfo().Assembly).GetSourceStreamAsync();

                var image = BitmapDescriptorFactory.DefaultMarker(Color.FromHex("2F3789"));
                var deviceFeatures = devices.Where(x=> !x.Attributes["name"].ToString().StartsWith("NA")).ToArray();
                foreach (var feature in deviceFeatures)
                {
                    var geom = feature.Geometry.Coordinate;
                    var point = new Position(geom.Y,geom.X);

                    var pin = new Pin
                    {
                        Label = "Device".Translate(),
                        Address = $"{feature.Attributes["name"]} ({"Click".Translate()})",
                        Position = point,
                        Icon = image
                    };
                    var samplingTime = feature.Attributes.GetOptionalValue("samplingTime") as AttributesTable;
                    var beginPosition = samplingTime?.GetOptionalValue("beginposition");
                    var endPosition = samplingTime?.GetOptionalValue("endposition");


                    pin.SetPinInfo(new PinInfo
                    {
                        IsDevice = true,
                        IsLocal = false,
                        DeviceId = feature.Attributes["name"]?.ToString(),
                        StartTime = beginPosition?.ToString(),
                        EndTime = endPosition?.ToString()
                    });

                    AddPin(pin);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        /// <summary>
        ///     The page appearing async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task PageAppearingAsync()
        {
            if (!_isPageShown)
            {
                var position = await _locationService.GetLastKnownLocationAsync();
                Request.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        new Position(position.Latitude, position.Longitude),
                        Distance.FromKilometers(2)));

                await WaitAndExecute(1000, async () =>
                {
                    await GetAllData();
                }).ConfigureAwait(false);
                _isPageShown = true;
            }
        }

        private async Task GetAllData()
        {
            ClearAllPins();
            await SetBusyAsync(async () => await GetAllDeviceLocation(), "GetAllDeviceLocation".Translate());

            if (_accessTokenManager.IsUserLoggedIn)
            {
                await SetBusyAsync(async () => await GetAllOtherIncidents(), "GetAllIncidents".Translate());
                await SetBusyAsync(async () => await GetAllCurrentUserLocation(), "GetAllLocalLocation".Translate());
            }
            else
            {
                await SetBusyAsync(async () => await GetAllAnonIncidents(), "GetAllIncidents".Translate());
            }
        }

        private async Task GetAllAnonIncidents()
        {
            try
            {
                var incidents = await _incidentsAppService.GetAll(new GetAllIncidentsInput
                {
                    MaxResultCount = 500
                });

                foreach (var item in incidents.Items)
                {
                    var location = item.Incident.LocationDtos.FirstOrDefault(x=> x.LocationType == LocationType.Incident);
                    var pin = CreateIncidentPin(location, PinType.OtherUser, item.Incident);
                    AddPin(pin);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }


        private Xamarin.Essentials.Location GetDefaultLocation()
        {
            var currentTenant = _dataStorageManager.Retrieve<string>(DataStorageKey.TenancyName);

            switch (currentTenant)
            {
                case "Birmingham":
                    return new Xamarin.Essentials.Location(52.489471, -1.898575);
                case "Brussels":
                    return new Xamarin.Essentials.Location(50.851470, 4.374362);
                case "Rotterdam":
                    return new Xamarin.Essentials.Location(51.921497, 4.490535);
                default:
                    return new Xamarin.Essentials.Location(52.4862, 1.8904);
            }
        }

        private async Task GetAllCurrentUserLocation()
        {
            try
            {
                var reports = await _incidentService.GetAllAsync();
                foreach (var report in reports)
                    if (report != null)
                    {
                        var label = "YourReport".Translate();
                        if (Enum.IsDefined(typeof(TypeOfRain), report.TypeOfRain))
                            label = EnumHelper<TypeOfRain>.GetEnumDescription((TypeOfRain)report.TypeOfRain);
                        var pin = new Pin
                        {
                            Label = label,
                            Address = $"{report.Date.DateTime:dd/MM/yyyy} ({"Click".Translate()})",
                            Position =
                                new Position(report.IncidentLocation.Latitude, report.IncidentLocation.Longitude),
                            Icon = BitmapDescriptorFactory.DefaultMarker(Color.FromHex("ffff00")),
                            ZIndex = 2
                        };

                        pin.SetPinInfo(new PinInfo
                        {
                            MobileId = report.MobileDataId,
                            IsDevice = false,
                            IsLocal = report.IsLocal,
                            IncidentId = report.IncidentId
                        });

                        AddPin(pin);
                    }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }


            try
            {
                var incidents = await _locationsAppService.GetAllCurrentUserIncident(new GetAllLocationsInput
                {
                    Sorting = "location.creationTime desc",
                    MaxResultCount = 500
                });
                RenderIncidentPins(incidents);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void RenderIncidentPins(PagedResultDto<GetAllLocationsOutput> incidents)
        {
            foreach (var item in incidents.Items)
            {
                if (Pins.Any(x => x.GetPinInfo().IncidentId == item.Location.IncidentId))
                {
                    continue;
                }
                var location = item.Location;
                if (location != null)
                {
                    var pin = CreateIncidentPin(location, PinType.CurrentUser);

                    AddPin(pin);
                }
            }
        }


        private static Pin CreateIncidentPin(LocationDto location, PinType pinType,object pinObject = null)
        {
            var label = "YourReport".Translate();
            var icon = BitmapDescriptorFactory.DefaultMarker(Color.FromHex("ffff00"));
            if (pinType == PinType.OtherUser)
            {
                label = "OtherReport".Translate();
                icon = BitmapDescriptorFactory.DefaultMarker(Color.Red);
            }

            if (Enum.IsDefined(typeof(TypeOfRain), location.TypeOfRain))
                label = EnumHelper<TypeOfRain>.GetEnumDescription(location.TypeOfRain);

            var pin = new Pin
            {
                Label = label,
                Address = $"{location.TimeCreated:dd/MM/yyyy} ({"Click".Translate()})",
                Position = new Position(location.Latitude, location.Longitude),
                Icon = icon
            };

            pin.SetPinInfo(new PinInfo
            {
                IncidentId = location.IncidentId,
                IsDevice = false,
                IsLocal = false,
                PinType = pinType,
                PinObject = pinObject
            });

            return pin;
        }

        private async Task GetAllOtherIncidents()
        {
            try
            {
                var incidents = await _locationsAppService.GetAllOtherUserIncident(new GetAllLocationsInput
                {
                    Sorting = "location.creationTime desc",
                    MaxResultCount = 500
                });
                foreach (var item in incidents.Items)
                {
                    var location = item.Location;
                    if (item.Location.IncidentId > 0 && location != null)
                    {
                        var pin = CreateIncidentPin(location, PinType.OtherUser);

                        AddPin(pin);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void AddPin(Pin pin)
        {
            var pinPosition = GetUniquePinPosition(pin.Position);
            if (_listOfPinnedLocations.ContainsKey(pinPosition.Latitude) == false)
            {
                _listOfPinnedLocations.Add(pinPosition.Latitude, new List<double>());
            }

            _listOfPinnedLocations[pinPosition.Latitude].Add(pinPosition.Longitude);
            pin.Position = pinPosition;
            Pins.Add(pin);
        }

        private Position GetUniquePinPosition(Position position)
        {
            if (_listOfPinnedLocations.ContainsKey(position.Latitude) && _listOfPinnedLocations[position.Latitude].Contains(position.Longitude))
            {
                double shiftValue = 0.00002;
                return GetUniquePinPosition(new Position(position.Latitude + shiftValue, position.Longitude + shiftValue));
            }

            return position;
        }

        private void ClearAllPins()
        {
            _listOfPinnedLocations.Clear();
            Pins.Clear();
        }

        private static DateTime GetToDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        private static DateTime GetFromDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
    }
}