using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.ViewModels.Incident
{
    public class IncidentDetailsPageModel : XamarinViewModel
    {
        private readonly IIncidentsAppService _incidentsAppService;

        private string _anySignOfDamage;

        private string _anySignOfObstruction;

        private string _areYouImpacted;
        private DateTime _createdTime;

        private DateTimeOffset _date;
        private string _floodDepth;
        private string _floodExtent;
        private string _frequencyOfFlood;
        private ObservableCollection<ImageItem> _listImages;

        private string _rainType;
        private ImageSource _rainTypeImage;
        private string _showAdvancedOptionsIcon;
        private bool _showAdvancedSection;
        private string _showPhotographsIcon;
        private bool _showPhotographsSection;
        private ObservableCollection<ImageItem> _typeOfFlooding;
        private string _typesOfSpaceFlooded;

        private string _userName;
        private string _waterClarity;

        public IncidentDetailsPageModel(IIncidentsAppService incidentsAppService)
        {
            _incidentsAppService = incidentsAppService;
            _showPhotographsIcon = "+";
            ShowAdvancedOptionsIcon = "+";
            TypeOfFlooding = new ObservableCollection<ImageItem>();
            ListImages = new ObservableCollection<ImageItem>();
        }

        public Command ShowAdvancedOptionsCommand => new Command(ShowAdvancedOptions);
        public Command ShowPhotographsCommand => new Command(ShowPhotographs);

        public bool ShowAdvancedSection
        {
            get => _showAdvancedSection;
            set
            {
                _showAdvancedSection = value;
                RaisePropertyChanged(() => ShowAdvancedSection);
            }
        }

        public string ShowPhotographsIcon
        {
            get => _showPhotographsIcon;
            set
            {
                _showPhotographsIcon = value;
                RaisePropertyChanged(() => ShowPhotographsIcon);
            }
        }

        public string ShowAdvancedOptionsIcon
        {
            get => _showAdvancedOptionsIcon;
            set
            {
                _showAdvancedOptionsIcon = value;
                RaisePropertyChanged(() => ShowAdvancedOptionsIcon);
            }
        }

        public bool ShowPhotographsSection
        {
            get => _showPhotographsSection;
            set
            {
                _showPhotographsSection = value;
                RaisePropertyChanged(() => ShowPhotographsSection);
            }
        }

        public int? IncidentId { get; set; }

        public DateTime CreatedTime
        {
            get => _createdTime;
            set
            {
                _createdTime = value;
                RaisePropertyChanged(() => CreatedTime);
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

        public ObservableCollection<ImageItem> ListImages
        {
            get => _listImages;
            set
            {
                _listImages = value;
                RaisePropertyChanged(() => ListImages);
            }
        }

        public string FloodDepth
        {
            get => _floodDepth;
            set
            {
                _floodDepth = value;
                RaisePropertyChanged(() => FloodDepth);
            }
        }

        public string FloodExtent
        {
            get => _floodExtent;
            set
            {
                _floodExtent = value;
                RaisePropertyChanged(() => FloodExtent);
            }
        }

        public string FrequencyOfFlood
        {
            get => _frequencyOfFlood;
            set
            {
                _frequencyOfFlood = value;
                RaisePropertyChanged(() => FrequencyOfFlood);
            }
        }


        public string TypesOfSpaceFlooded
        {
            get => _typesOfSpaceFlooded;
            set
            {
                _typesOfSpaceFlooded = value;
                RaisePropertyChanged(() => TypesOfSpaceFlooded);
            }
        }

        public string WaterClarity
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

        public string AreYouImpacted
        {
            get => _areYouImpacted;
            set
            {
                _areYouImpacted = value;
                RaisePropertyChanged(() => AreYouImpacted);
            }
        }

        public string AnySignOfDamage
        {
            get => _anySignOfDamage;
            set
            {
                _anySignOfDamage = value;
                RaisePropertyChanged(() => AnySignOfDamage);
            }
        }

        public string AnySignOfObstruction
        {
            get => _anySignOfObstruction;
            set
            {
                _anySignOfObstruction = value;
                RaisePropertyChanged(() => AnySignOfObstruction);
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


        public ICommand PageAppearingCommand => AsyncCommand.Create(PageAppearing);

        private void ShowAdvancedOptions()
        {
            if (ShowAdvancedSection)
            {
                ShowAdvancedSection = false;
                ShowAdvancedOptionsIcon = "+";
            }
            else
            {
                ShowAdvancedSection = true;
                ShowAdvancedOptionsIcon = "-";
            }
        }

        private void ShowPhotographs()
        {
            if (ShowPhotographsSection)
            {
                ShowPhotographsSection = false;
                _showPhotographsIcon = "+";
            }
            else
            {
                ShowPhotographsSection = true;
                _showPhotographsIcon = "-";
            }
        }

        private async Task PageAppearing()
        {
            try
            {
                if (IncidentId.HasValue)
                {
                    await SetBusyAsync(async () =>
                    {
                        var incidentForView = await _incidentsAppService.GetIncidentById(new EntityDto(IncidentId.Value));
                        if (incidentForView != null)
                        {
                            Username = incidentForView.Username;
                            RainType = incidentForView.Incident.TypeOfRain.ToString().Translate();
                            RainTypeImage = EnumHelper<TypeOfRain>.GetImage(incidentForView.Incident.TypeOfRain);
                            CreatedTime = incidentForView.Incident.Date.DateTime;
                            foreach (var incidentFloodType in incidentForView.Incident.FloodTypes)
                                TypeOfFlooding.Add(new ImageItem
                                {
                                    ImageSource =
                                            EnumHelper<TypeOfFlood>.GetImage(incidentFloodType.TypeOfFlood),
                                    Tooltip = incidentFloodType.TypeOfFlood.ToString().Translate()
                                });
                            FloodDepth = (int)incidentForView.Incident.FloodDepth == -1
                                    ? "NotSpecified".Translate()
                                    : EnumHelper<FloodDepth>.GetEnumDescription(incidentForView.Incident.FloodDepth);
                            FloodExtent = (int)incidentForView.Incident.FloodExtent == -1
                                    ? "NotSpecified".Translate()
                                    : EnumHelper<FloodExtent>.GetEnumDescription(incidentForView.Incident.FloodExtent);
                            FrequencyOfFlood = (int)incidentForView.Incident.FrequencyOfFlood == -1
                                    ? "NotSpecified".Translate()
                                    : EnumHelper<FrequencyOfFlood>.GetEnumDescription(incidentForView.Incident
                                        .FrequencyOfFlood);
                            TypesOfSpaceFlooded = (int)incidentForView.Incident.TypesOfSpaceFlooded == -1
                                    ? "NotSpecified".Translate()
                                    : EnumHelper<TypesOfSpaceFlooded>.GetEnumDescription(incidentForView.Incident
                                        .TypesOfSpaceFlooded);
                            WaterClarity = (int)incidentForView.Incident.WaterClarity == -1
                                    ? "NotSpecified".Translate()
                                    : EnumHelper<WaterClarity>.GetEnumDescription(incidentForView.Incident.WaterClarity);
                            AreYouImpacted = incidentForView.Incident.AreYouImpacted
                                    ? "Yes".Translate()
                                    : "No".Translate();
                            AnySignOfDamage = incidentForView.Incident.AnySignOfDamage
                                    ? "Yes".Translate()
                                    : "No".Translate();
                            AnySignOfObstruction = incidentForView.Incident.AnySignOfObstruction
                                    ? "Yes".Translate()
                                    : "No".Translate();
                            foreach (var photoItem in incidentForView.Incident.PictureDtos)
                                if (!string.IsNullOrEmpty(photoItem.URL))
                                    ListImages.Add(new ImageItem
                                    {
                                        ImageSource = ImageSource.FromUri(new Uri(photoItem.URL))
                                    });
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData != null) IncidentId = int.Parse(navigationData.ToString());
            await base.InitializeAsync(navigationData);
        }
    }
}