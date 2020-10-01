using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Services.Location;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace IIASA.FloodCitiSense.Notification
{
    public static class PushNotificationHelper
    {
        public static async Task UpdateCurrentCountryTag()
        {
            PushNotificationConstants.UserInputComplete = false;

            MainThread.BeginInvokeOnMainThread(async () => 
            {
                try
                {
                    var locationService = Resolver.Resolve<ILocationService>();
                    var location = await locationService.GetLastKnownLocationAsync();
                    if (location == null)
                    {
                        location = await locationService.GetCurrentLocation();
                    }

                    var markers = await Geocoding.GetPlacemarksAsync(location);
                    var placeMark = markers.First(x => string.IsNullOrEmpty(x.CountryCode) == false);
                    var countryCode = placeMark.CountryCode;
                    var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                    var tag = $"{countryCode}-{lang}";
                    PushNotificationConstants.SubscriptionTags = new[] { tag };
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    Log.Warning(PushNotificationConstants.DebugTag, $"Error fetching device location - Feature not supported: {fnsEx}");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    Log.Warning(PushNotificationConstants.DebugTag, $"Error fetching device location - Feature not enabled: {fneEx}");

                }
                catch (PermissionException pEx)
                {
                    Log.Warning(PushNotificationConstants.DebugTag, $"Error fetching device location - PermissionException: {pEx}");

                }
                catch (Exception ex)
                {
                    Log.Warning(PushNotificationConstants.DebugTag, $"Error fetching device location - exception: {ex}");
                }
                finally
                {
                    PushNotificationConstants.UserInputComplete = true;
                }
            });

            while (PushNotificationConstants.UserInputComplete == false)
            {
                // waiting for user to accept location read request on main threat for Android.
                await Task.Delay(500);
            }

            var dataManager = Resolver.Resolve<IDataStorageManager>();
            await dataManager.StoreAsync(DataStorageKey.NotificationTag,
                string.Join(" ", PushNotificationConstants.SubscriptionTags));
        }
    }
}