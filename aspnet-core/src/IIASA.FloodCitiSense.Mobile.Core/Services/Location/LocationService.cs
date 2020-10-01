//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LocationService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   LocationService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Abp.Dependency;
using Acr.UserDialogs;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.UI;
using Microsoft.AppCenter.Crashes;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Location
{
    /// <summary>
    ///     The location service.
    /// </summary>
    public class LocationService : ILocationService, ISingletonDependency
    {
        private CancellationTokenSource _cts;

        /// <summary>
        ///     Gets the current location.
        /// </summary>
        /// <returns></returns>
        public async Task<Xamarin.Essentials.Location> GetCurrentLocation()
        {
            try
            {
                await CheckPermissionAsync();
                UserDialogs.Instance.ShowLoading("GettingLocation".Translate());
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromMinutes(1));
                _cts = new CancellationTokenSource();
                return await Geolocation.GetLocationAsync(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
                UserDialogs.Instance.HideLoading();
            }
            return null;
        }

        public async Task<Xamarin.Essentials.Location> GetLastKnownLocationAsync()
        {
            try
            {
                await CheckPermissionAsync();
                UserDialogs.Instance.ShowLoading("GettingLocation".Translate());
                _cts = new CancellationTokenSource();
                return await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
                UserDialogs.Instance.HideLoading();
            }
            return null;
        }

        /// <summary>
        ///     Determines whether [is location available].
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is location available]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsLocationAvailableAsync()
        {
            return await GetLastKnownLocationAsync() != null;
        }

        /// <summary>
        ///     Determines whether [is location enabled].
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is location enabled]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsLocationEnabledAsync()
        {
            return await CheckPermissionAsync() && CrossGeolocator.Current.IsGeolocationEnabled;
        }

        private async Task<bool> CheckPermissionAsync()
        {
            var status =
                await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission
                    .Location);
            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions
                    .Abstractions.Permission.Location);
                var results =
                    await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission
                        .Location);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Location))
                    status = results[Plugin.Permissions.Abstractions.Permission.Location];
            }

            return status == PermissionStatus.Granted;
        }
    }
}