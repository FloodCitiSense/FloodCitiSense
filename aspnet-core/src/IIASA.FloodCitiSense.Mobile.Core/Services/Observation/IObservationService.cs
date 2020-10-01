//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IObservationService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   IObservationService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Services.Observation.Dto;
using System;
using System.Threading.Tasks;
using NetTopologySuite.Features;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Observation
{
    /// <summary>
    /// The ObservationService interface.
    /// </summary>
    public interface IObservationService
    {
        /// <summary>
        /// The get all device location.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<FeatureCollection> GetAllDeviceLocation();
        Task<AllDevices> GetDeviceDataAsync(DateTime startTime, DateTime endTime, string name, string aggregateInterval);
    }
}