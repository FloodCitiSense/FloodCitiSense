namespace IIASA.FloodCitiSense.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        public const string Pages_Cities = "Pages.Cities";
        public const string Pages_Cities_Create = "Pages.Cities.Create";
        public const string Pages_Cities_Edit = "Pages.Cities.Edit";
        public const string Pages_Cities_Delete = "Pages.Cities.Delete";

        public const string Pages_Sensors = "Pages.Sensors";
        public const string Pages_Sensors_Create = "Pages.Sensors.Create";
        public const string Pages_Sensors_Edit = "Pages.Sensors.Edit";
        public const string Pages_Sensors_Delete = "Pages.Sensors.Delete";

        public const string Pages_IncidentApprovals = "Pages.IncidentApprovals";
        public const string Pages_IncidentApprovals_Create = "Pages.IncidentApprovals.Create";
        public const string Pages_IncidentApprovals_Edit = "Pages.IncidentApprovals.Edit";
        public const string Pages_IncidentApprovals_Delete = "Pages.IncidentApprovals.Delete";

        public const string Pages_Incidents = "Pages.Incidents";
        public const string Pages_Incidents_Create = "Pages.Incidents.Create";
        public const string Pages_Incidents_Edit = "Pages.Incidents.Edit";
        public const string Pages_Incidents_Delete = "Pages.Incidents.Delete";

        public const string Pages_Locations = "Pages.Locations";
        public const string Pages_Locations_Create = "Pages.Locations.Create";
        public const string Pages_Locations_Edit = "Pages.Locations.Edit";
        public const string Pages_Locations_Delete = "Pages.Locations.Delete";

        public const string Pages_Pictures = "Pages.Pictures";
        public const string Pages_Pictures_Create = "Pages.Pictures.Create";
        public const string Pages_Pictures_Edit = "Pages.Pictures.Edit";
        public const string Pages_Pictures_Delete = "Pages.Pictures.Delete";

        public const string Pages_CreativeEntiies = "Pages.CreativeEntiies";
        public const string Pages_CreativeEntiies_Create = "Pages.CreativeEntiies.Create";
        public const string Pages_CreativeEntiies_Edit = "Pages.CreativeEntiies.Edit";
        public const string Pages_CreativeEntiies_Delete = "Pages.CreativeEntiies.Delete";

        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents= "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";

        // Mobile Push Notifications
        public const string Pages_MobilePushNotification = "Pages.MobilePushNotification";
        public const string Pages_MobilePushNotification_Send = "Pages.MobilePushNotification.Send";
        public const string Pages_MobilePushNotification_Get = "Pages.MobilePushNotification.Get";
    }
}