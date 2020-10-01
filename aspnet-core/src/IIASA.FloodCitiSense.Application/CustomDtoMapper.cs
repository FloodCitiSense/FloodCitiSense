using IIASA.FloodCitiSense.DataTypes.Dtos;
using IIASA.FloodCitiSense.DataTypes;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Playground;
using IIASA.FloodCitiSense.Playground.Dtos;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using IIASA.FloodCitiSense.Auditing.Dto;
using IIASA.FloodCitiSense.Authorization.Accounts.Dto;
using IIASA.FloodCitiSense.Authorization.Permissions.Dto;
using IIASA.FloodCitiSense.Authorization.Roles;
using IIASA.FloodCitiSense.Authorization.Roles.Dto;
using IIASA.FloodCitiSense.Authorization.Users;
using IIASA.FloodCitiSense.Authorization.Users.Dto;
using IIASA.FloodCitiSense.Authorization.Users.Profile.Dto;
using IIASA.FloodCitiSense.Chat;
using IIASA.FloodCitiSense.Chat.Dto;
using IIASA.FloodCitiSense.Editions;
using IIASA.FloodCitiSense.Editions.Dto;
using IIASA.FloodCitiSense.Friendships;
using IIASA.FloodCitiSense.Friendships.Cache;
using IIASA.FloodCitiSense.Friendships.Dto;
using IIASA.FloodCitiSense.Localization.Dto;
using IIASA.FloodCitiSense.MultiTenancy;
using IIASA.FloodCitiSense.MultiTenancy.Dto;
using IIASA.FloodCitiSense.MultiTenancy.HostDashboard.Dto;
using IIASA.FloodCitiSense.MultiTenancy.Payments;
using IIASA.FloodCitiSense.MultiTenancy.Payments.Dto;
using IIASA.FloodCitiSense.Notifications.Dto;
using IIASA.FloodCitiSense.Organizations.Dto;
using IIASA.FloodCitiSense.Sessions.Dto;
using IIASA.FloodCitiSense.ValueResolver;
using System.Linq;
using System;
using NetTopologySuite.Geometries;

namespace IIASA.FloodCitiSense
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
           configuration.CreateMap<CreateOrEditCityDto, City>();
           configuration.CreateMap<City, CityDto>();
            configuration.CreateMap<CreateOrEditSensorDto, Sensor>();
            configuration.CreateMap<Sensor, SensorDto>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.User.Id));
            configuration.CreateMap<Sensor, CreateOrEditSensorDto>()
               .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.User.Id));
            configuration.CreateMap<CreateOrEditIncidentApprovalDto, IncidentApproval>();
            configuration.CreateMap<IncidentApproval, IncidentApprovalDto>();
            configuration.CreateMap<CreateOrEditIncidentDto, Incident>();
            configuration.CreateMap<Incident, IncidentDto>()
                .ForMember(x => x.Date, opt => opt.MapFrom(x => x.TimeCreated))
                .ForMember(x => x.PictureDtos, opt => opt.MapFrom(x => x.Pictures))
                .ForMember(x => x.LocationDtos, opt => opt.MapFrom(x => x.Locations));

            configuration.CreateMap<CreateOrEditLocationDto, Location>();
            configuration.CreateMap<FloodType, FloodTypeDto>();
            configuration.CreateMap<FloodTypeDto, FloodType>();
            configuration.CreateMap<Location, LocationDto>()
                .ForMember(x => x.TypeOfRain, opt => opt.MapFrom(x => x.Incident.TypeOfRain))
                .ForMember(x => x.IncidentId, opt => opt.MapFrom(x => x.Incident.Id))
                .ForMember(x => x.CreatorUserId, opt => opt.MapFrom(x => x.Incident.CreatorUserId))
                .ForMember(x => x.TimeCreated, opt => opt.MapFrom(x => x.Incident.TimeCreated));
            configuration.CreateMap<CreateOrEditPictureDto, Picture>();
            configuration.CreateMap<Picture, PictureDto>();
            configuration.CreateMap<CreateOrEditCreativeEntiyDto, CreativeEntiy>();
            configuration.CreateMap<CreativeEntiy, CreativeEntiyDto>();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */

            configuration.CreateMap<IncidentViewModel, Incident>()
                .ForMember(m => m.TimeCreated, opt => opt.MapFrom(input => input.Date.UtcDateTime))
                .ForMember(m => m.Locations, opt => opt.MapFrom(input => input.LocationDtos))
                .ForMember(m => m.Pictures, opt => opt.MapFrom(input => input.Images.Select(x=> new Picture { Url = x, MobileDataId = new Guid(input.MobileDataId), TenantId= input.TenantId })) );

            configuration.CreateMap<IncidentEditDto, Incident>()
                .ForMember(m => m.Pictures, opt => opt.Ignore())
                .ForMember(m => m.TimeCreated, opt => opt.Ignore())
                .ForMember(m => m.Locations, opt => opt.Ignore());

            configuration.CreateMap<PictureDto, Picture>();

            configuration.CreateMap<MobilePushNotificationViewModel, Datatypes.MobilePushNotification>();

            configuration.CreateMap<Datatypes.MobilePushNotification, MobilePushNotificationViewModel>()
                .ForMember(m => m.Date, opt => opt.MapFrom(input => input.CreationTime));

            configuration.CreateMap<LocationDto, Location>()
                .ForMember(m => m.CreatorUserId, opt => opt.MapFrom(input => input.CreatorUserId))
                .ForMember(m => m.TenantId, opt => opt.MapFrom(input => input.TenantID))
                .ForMember(m => m.Point, opt => opt.MapFrom(input => new Point(input.Latitude, input.Longitude)));
        }
    }
}