//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ObservationService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   ObservationService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Abp.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Services.Observation.Dto;
using MonkeyCache.LiteDB;
using RestSharp;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Flurl.Http;
using NetTopologySuite.Features;
using NetTopologySuite.IO;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Observation
{
    /// <summary>
    /// The observation service.
    /// </summary>
    public class ObservationService : IObservationService, ISingletonDependency
    {
        /// <summary>
        /// The get all device location.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<FeatureCollection> GetAllDeviceLocation()
        {
            var reader = new GeoJsonReader();
            if (!Barrel.Current.IsExpired(nameof(GetAllDeviceLocation)))
            {
                var cachedData = Barrel.Current.Get<string>(nameof(GetAllDeviceLocation));
                var resultData = reader.Read<FeatureCollection>(cachedData);
                return resultData;
            }
            var data = await Constant.DeviceGeoJsonApi.GetStringAsync();
            var result = reader.Read<FeatureCollection>(data);
            Barrel.Current.Add(nameof(GetAllDeviceLocation), data, TimeSpan.FromDays(1));
            return result;
        }

        public async Task<AllDevices> GetDeviceDataAsync(DateTime startTime, DateTime endTime, string name, string aggregateInterval)
        {
            startTime = GetDeviceStartTime(startTime); // since we are aggregating values starting from 12hr
            endTime = GetDeviceDateTime(endTime);

            var client = new RestClient(Constant.DeviceApiHost);
            var request = new RestRequest(Method.GET);
            request.AddParameter("service", "SOS");
            request.AddParameter("version", "1.0.0");
            request.AddParameter("request", "GetObservation");
            request.AddParameter("offering", "temporary");
            request.AddParameter("procedure", name);
            request.AddParameter("eventTime", $"{startTime:s}Z/{endTime:s}Z");
            Debug.WriteLine($"{startTime:s}Z/{endTime:s}Z");
            request.AddParameter("aggregateInterval", aggregateInterval);
            request.AddParameter("aggregateFunction", "SUM");
            request.AddParameter("observedProperty", "rainfall");
            request.AddParameter("responseFormat", "application/json");
            var urlStr = startTime.Date.ToString("dd/MM/yyyy") + endTime.Date.ToString("dd/MM/yyyy") + name + aggregateInterval;
            if (!Barrel.Current.IsExpired(urlStr))
            {
                var res = Barrel.Current.Get<AllDevices>(urlStr);
                return res;
            }
            var response = await client.ExecuteGetTaskAsync<AllDevices>(request);

            if (!response.IsSuccessful) return null;
            Barrel.Current.Add(urlStr, response.Data, TimeSpan.FromHours(1));
            return response.Data;
        }

        private static DateTime GetDeviceStartTime(DateTime startTime)
        {
            var dateTime = GetDeviceDateTime(startTime);
            return dateTime.AddDays(-1);
        }

        private static DateTime GetDeviceDateTime(DateTime startTime)
        {
            // since we are aggregating values starting from 12hr, fixing the time as 12:00:00
            return new DateTime(startTime.Year, startTime.Month, startTime.Day, 12, 0, 0);
        }
    }
}