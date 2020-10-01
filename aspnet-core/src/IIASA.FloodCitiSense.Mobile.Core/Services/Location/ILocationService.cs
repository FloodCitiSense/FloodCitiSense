//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ILocationService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   ILocationService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Location
{
    public interface ILocationService
    {
        Task<Xamarin.Essentials.Location> GetCurrentLocation();
        Task<Xamarin.Essentials.Location> GetLastKnownLocationAsync();
        Task<bool> IsLocationAvailableAsync();
        Task<bool> IsLocationEnabledAsync();
    }
}