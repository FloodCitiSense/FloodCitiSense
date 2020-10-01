using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Mobile.Core;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using System;
using System.Linq;

namespace IIASA.FloodCitiSense
{
    [DependsOn(typeof(FloodCitiSenseClientModule), typeof(FloodCitiSenseXamarinCoreModule), typeof(AbpAutoMapperModule))]
    public class FloodCitiSenseXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            try
            {
                Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
                {
                    config.CreateMap<GeoCoordinate, LocationDto>()
                                .ForMember(x => x.Timestamp, opt => opt.MapFrom(x => x.Timestamp.DateTime));
                    config.CreateMap<LocationDto, GeoCoordinate>()
                                .ForPath(x => x.Timestamp.DateTime, opt => opt.MapFrom(x => x.Timestamp));
                    config.CreateMap<IncidentDto, LocalIncident>()
                                .ForMember(x => x.UserLocation, opt => opt.MapFrom(x => x.LocationDtos.FirstOrDefault(j => j.LocationType == Datatypes.LocationType.User)))
                                .ForPath(x => x.UserLocation.Timestamp, opt => opt.Ignore())
                                .ForMember(x => x.IncidentLocation, opt => opt.MapFrom(x => x.LocationDtos.FirstOrDefault(j => j.LocationType == Datatypes.LocationType.Incident)))
                                .ForPath(x => x.IncidentLocation.Timestamp, opt => opt.Ignore())
                                .ForMember(x => x.TypeOfFloodings, opt => opt.MapFrom(x => x.FloodTypes.Select(j => j.TypeOfFlood)))
                                .ForMember(x => x.Images, opt => opt.MapFrom(x => x.PictureDtos.Select(j => j.URL)))
                                .ForMember(x => x.Date, opt => opt.MapFrom(x => x.Date)).ReverseMap();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FloodCitiSenseXamarinSharedModule).GetAssembly());
        }
    }
}