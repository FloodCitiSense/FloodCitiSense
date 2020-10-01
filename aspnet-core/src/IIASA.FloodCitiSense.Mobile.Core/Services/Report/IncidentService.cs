using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.ObjectMapping;
using Acr.UserDialogs;
using AutoMapper;
using IIASA.FloodCitiSense.Authorization.Users.Profile;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Mobile.Core.Controls;
using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using IIASA.FloodCitiSense.Mobile.Core.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Services.Core;
using IIASA.FloodCitiSense.Picture;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Report
{
    public class IncidentService : IIncidentService, ISingletonDependency
    {
        private readonly IIncidentsAppService _incidentsAppService;
        private readonly IProfileAppService profileAppService;
        private readonly ProxyPictureControllerService _pictureControllerService;

        public IDbService<LocalIncident> DbService { get; }

        public IncidentService(IIncidentsAppService incidentsAppService, IDbService<LocalIncident> dbService, IProfileAppService profileAppService,
            ProxyPictureControllerService pictureControllerService)
        {
            _incidentsAppService = incidentsAppService;
            DbService = dbService;
            this.profileAppService = profileAppService;
            _pictureControllerService = pictureControllerService;
        }

        public Task<bool> Create(LocalIncident report, string eventName = "IncidentCreated")
        {
            try
            {
                report.Id = Guid.NewGuid().ToString();
                report.MobileDataId = report.Id;
                report.IncidentLocation.IncidentId = report.Id;
                report.UserLocation.IncidentId = report.Id;
                report.IsLocal = true;
                DbService.CreateItem(report);
                Analytics.TrackEvent(eventName);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return Task.FromResult(false);
            }
        }

        public Task<bool> Update(LocalIncident report)
        {
            try
            {
                report.IsUploaded = false;
                report.IsEdited = true;
                report.IsLocal = true;
                DbService.UpdateItem(report);
                Analytics.TrackEvent("IncidentEdited");
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return Task.FromResult(false);
            }
        }

        public async Task<List<LocalIncident>> GetAllAsync()
        {
            try
            {
                var reports = new List<LocalIncident>();
                try
                {
                    var currentUser = await profileAppService.GetCurrentUserProfileForEdit();
                    var uploadedReports = await _incidentsAppService.GetIncidentByUserId(new EntityDto { Id = currentUser.Id });
                    if (uploadedReports?.Items != null)
                        foreach (var incidentsOutput in uploadedReports?.Items.Where(x =>
                            DbService.ReadAllItems().All(y => y.MobileDataId != x.Incident.MobileDataId)))
                        {
                            var localReport = MapLocalIncident(incidentsOutput);
                            DbService.CreateItem(localReport);
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                foreach (var item in DbService.ReadAllItems().ToList())
                {
                    item.IsLocal = true;
                    reports.Add(item);
                }
                return reports.OrderByDescending(x=> x.Date).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<List<LocalIncident>> GetAllAnonAsync()
        {
            var reports = new List<LocalIncident>();
            try
            {
                var uploadedReports =
                    await _incidentsAppService.GetAll(new GetAllIncidentsInput
                    {
                        MaxResultCount = 1000
                    });
                if (uploadedReports?.Items != null)
                    foreach (var incidentsOutput in uploadedReports?.Items.Where(x =>
                        reports.All(y => y.MobileDataId != x.Incident.MobileDataId)))
                    {
                        var localReport = MapLocalIncident(incidentsOutput);
                        reports.Add(localReport);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return reports.OrderByDescending(x => x.Date).ToList();
        }

        private static LocalIncident MapLocalIncident(GetAllIncidentsOutput incidentsOutput)
        {
            var uploadedReport = incidentsOutput.Incident;
            var mapper = Resolver.Resolve<Abp.ObjectMapping.IObjectMapper>();
            var localReport = mapper.Map<LocalIncident>(uploadedReport);
            localReport.Images = uploadedReport.PictureDtos.Select(x => x.URL).ToList();
            localReport.LocalImages = uploadedReport.PictureDtos.Select(x => new LocalImage
            {
                IsUploaded = true,
                Path = x.URL,
            }).ToList();
            localReport.TypeOfFloodings =
                uploadedReport.FloodTypes.Select(x => (int) x.TypeOfFlood).ToList();
            localReport.IncidentId = uploadedReport.Id;
            localReport.IsLocal = false;
            localReport.IsUploaded = true;
            localReport.IsCreated = true;
            return localReport;
        }

        public LocalIncident GetReportById(string id)
        {
            try
            {
                return DbService.ReadAllItems().FirstOrDefault(x => x.Id == id);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public LocalIncident GetReportByMobileId(string id)
        {
            try
            {
                return DbService.ReadAllItems().FirstOrDefault(x => x.MobileDataId == id);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<OutputDto> Upload(LocalIncident report)
        {
            OutputDto output = null;
            var uploadedImages = await UploadPictures(report);
            if (!report.IsCreated)
            {
                var incident = CreateData(report, uploadedImages);
                await Task.Delay(300);
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "UploadingIncident".Translate()))
                {
                    await WebRequestExecuter.Execute(async () =>
                    {
                        output = await _incidentsAppService.Create(incident);

                    }, () => Task.CompletedTask, exception =>
                    {
                        UserDialogs.Instance.Toast("IncidentUploadError".Translate());
                        return Task.CompletedTask;
                    });
                }
            }
            else
            {
                var incident = EditData(report, uploadedImages);

                await WebRequestExecuter.Execute(async () =>
                {
                    using (await MaterialDialog.Instance.LoadingDialogAsync(message: "UploadingIncident".Translate()))
                    {
                        output = await _incidentsAppService.Update(incident);
                    }
                }, () => Task.CompletedTask);

            }

            if (output != null && output.Success)
            {
                try
                {
                    report.IsUploaded = true;
                    report.IsCreated = true;
                    report.IncidentId = output.Id;
                    DbService.UpdateItem(report);
                    Analytics.TrackEvent("IncidentUploaded");
                    var formattedString = new FormattedString();

                    formattedString.Spans.Add(new Span
                    {
                        Text = "UploadMessage1".Translate(),
                        FontSize = 20

                    });

                    formattedString.Spans.Add(new Span
                    {
                        Text = Environment.NewLine
                    });
                    formattedString.Spans.Add(new Span
                    {
                        Text = "UploadMessage2".Translate(),
                        FontSize = 20,
                    });
                    formattedString.Spans.Add(new Span
                    {
                        Text = Environment.NewLine
                    });

                    formattedString.Spans.Add(new Span
                    {
                        Text = "UploadMessage3".Translate(),
                        FontSize = 20
                    });


                    var popup = new LottieLoader(formattedString, "success.json", false);
                    //using (new LottieLoader(formattedString, "success.json"))
                    //{
                    //    await Task.Delay(5000);
                    //}
                    //UserDialogs.Instance.Toast("UploadedSuccessfully".Translate());
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                }
            }
            else if (output != null && output.Message == "AlreadyCreated")
                try
                {
                    report.IsUploaded = true;
                    report.IsCreated = true;
                    report.IncidentId = output.Id;
                    DbService.UpdateItem(report);
                    UserDialogs.Instance.Toast("AlreadyCreated".Translate());
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                }
            else
                UserDialogs.Instance.Toast("IncidentUploadError".Translate());

            return output;

        }

        public Task<List<GeoCoordinate>> GetLocations()
        {
            try
            {
                var locations = DbService.ReadAllItems().ToList().Select(x => x.IncidentLocation)
                    .ToList();
                return Task.FromResult(locations);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string reportId)
        {
            try
            {
                DbService.DeleteItem(reportId);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }

        private IncidentViewModel CreateData(LocalIncident input, List<string> uploadedImages)
        {
            var incident = new IncidentViewModel
            {
                MobileDataId = input.MobileDataId,
                TenantId = 1,
                AnySignOfDamage = input.AnySignOfDamage,
                AnySignOfObstruction = input.AnySignOfObstruction,
                AreYouImpacted = input.AreYouImpacted,
                FloodTypes = input.TypeOfFloodings.Select(x => new FloodTypeDto
                {
                    TypeOfFlood = EnumHelper<TypeOfFlood>.ParseInt(x),
                    TenantId = 1
                }).ToList(),
                FloodDepth = EnumHelper<FloodDepth>.ParseInt(input.FloodDepth),
                FloodExtent = EnumHelper<FloodExtent>.ParseInt(input.FloodExtent),
                FrequencyOfFlood = EnumHelper<FrequencyOfFlood>.ParseInt(input.FrequencyOfFlood),
                TypeOfRain = EnumHelper<TypeOfRain>.ParseInt(input.TypeOfRain),
                TypesOfSpaceFlooded =
                    EnumHelper<TypesOfSpaceFlooded>.ParseInt(input.TypesOfSpaceFlooded),
                WaterClarity = EnumHelper<WaterClarity>.ParseInt(input.WaterClarity),
                Images = uploadedImages,
                Date = input.Date,
                LocationDtos = new List<LocationDto>
                {
                    new LocationDto
                    {
                        TenantID = 1,
                        LocationType = LocationType.User,
                        Accuracy = input.UserLocation.Accuracy,
                        Longitude = input.UserLocation.Longitude,
                        Altitude = input.UserLocation.Altitude,
                        AltitudeAccuracy = input.UserLocation.AltitudeAccuracy,
                        Heading = input.UserLocation.Heading,
                        Latitude = input.UserLocation.Latitude,
                        Speed = input.UserLocation.Speed,
                        Timestamp = input.UserLocation.Timestamp.DateTime
                    },
                    new LocationDto
                    {
                        TenantID = 1,
                        LocationType = LocationType.Incident,
                        Accuracy = input.IncidentLocation.Accuracy,
                        Longitude = input.IncidentLocation.Longitude,
                        Altitude = input.IncidentLocation.Altitude,
                        AltitudeAccuracy = input.IncidentLocation.AltitudeAccuracy,
                        Heading = input.IncidentLocation.Heading,
                        Latitude = input.IncidentLocation.Latitude,
                        Speed = input.IncidentLocation.Speed,
                        Timestamp = input.IncidentLocation.Timestamp.DateTime
                    }
                }
            };
            return incident;
        }

        private IncidentViewModel EditData(LocalIncident input, List<string> uploadedImages)
        {
            var incident = new IncidentViewModel
            {
                Id = input.IncidentId,
                TenantId = 1,
                MobileDataId = input.MobileDataId,
                AnySignOfDamage = input.AnySignOfDamage,
                AnySignOfObstruction = input.AnySignOfObstruction,
                AreYouImpacted = input.AreYouImpacted,
                FloodTypes = input.TypeOfFloodings.Select(x => new FloodTypeDto
                {
                    TypeOfFlood = EnumHelper<TypeOfFlood>.ParseInt(x),
                    TenantId = 1
                }).ToList(),
                FloodDepth = EnumHelper<FloodDepth>.ParseInt(input.FloodDepth),
                FloodExtent = EnumHelper<FloodExtent>.ParseInt(input.FloodExtent),
                FrequencyOfFlood = EnumHelper<FrequencyOfFlood>.ParseInt(input.FrequencyOfFlood),
                TypeOfRain = EnumHelper<TypeOfRain>.ParseInt(input.TypeOfRain),
                TypesOfSpaceFlooded =
                    EnumHelper<TypesOfSpaceFlooded>.ParseInt(input.TypesOfSpaceFlooded),
                WaterClarity = EnumHelper<WaterClarity>.ParseInt(input.WaterClarity),
                Images = uploadedImages,
                Date = input.Date,
                LocationDtos = new List<LocationDto>
                {
                    new LocationDto
                    {
                        TenantID = 1,
                        LocationType = LocationType.User,
                        Accuracy = input.UserLocation.Accuracy,
                        Longitude = input.UserLocation.Longitude,
                        Altitude = input.UserLocation.Altitude,
                        AltitudeAccuracy = input.UserLocation.AltitudeAccuracy,
                        Heading = input.UserLocation.Heading,
                        Latitude = input.UserLocation.Latitude,
                        Speed = input.UserLocation.Speed,
                        Timestamp = input.UserLocation.Timestamp.DateTime
                    },
                    new LocationDto
                    {
                        TenantID = 1,
                        LocationType = LocationType.Incident,
                        Accuracy = input.IncidentLocation.Accuracy,
                        Longitude = input.IncidentLocation.Longitude,
                        Altitude = input.IncidentLocation.Altitude,
                        AltitudeAccuracy = input.IncidentLocation.AltitudeAccuracy,
                        Heading = input.IncidentLocation.Heading,
                        Latitude = input.IncidentLocation.Latitude,
                        Speed = input.IncidentLocation.Speed,
                        Timestamp = input.IncidentLocation.Timestamp.DateTime
                    }
                }
            };
            return incident;
        }

        private static async Task<List<string>> UploadPictures(LocalIncident input)
        {
            try
            {
                var uploadedImages = new List<string>();

                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "UploadingPicture".Translate()))
                {
                    if (input.LocalImages == null || !input.LocalImages.Any()) return uploadedImages;
                    foreach (var image in input.LocalImages.Where(x=> !x.IsUploaded))
                    {
                        var restClient = new RestClient(Constant.ImagesUploadUri);
                        var restRequest = new RestRequest
                        {
                            RequestFormat = DataFormat.Json,
                            Method = Method.POST,
                            AlwaysMultipartFormData = true
                        };
                        restRequest.AddParameter("Authorization", "Bearer " + Constant.GeoWikiApiKey, ParameterType.HttpHeader);
                        restRequest.AddFile("image", image.Path);
                        var response = await restClient.ExecuteTaskAsync(restRequest);



                        var res = await Constant.ImagesUploadUri
                            .WithOAuthBearerToken(Constant.GeoWikiApiKey)
                            .PostMultipartAsync(x => x.AddFile("image", image.Path))
                            .ReceiveString();


                        if (res != null)
                        {

                        }

                        if (response.IsSuccessful)
                        {
                            uploadedImages.Add(response.ResponseUri.AbsoluteUri);
                            image.IsUploaded = true;
                        }
                        else
                        {
                            throw new Exception("Failed to upload image.");
                        }


                    }
                }
                return uploadedImages;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }
    }
}