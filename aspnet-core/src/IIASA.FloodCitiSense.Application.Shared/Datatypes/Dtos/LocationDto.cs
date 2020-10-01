
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class LocationDto : FullAuditedEntityDto, IExtendableObject
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TenantID { get; set; }
        public int IncidentId { get; set; }

        public double Altitude { get; set; }
        public double AltitudeAccuracy { get; set; }
        public double Accuracy { get; set; }
        public double Heading { get; set; }
        public double Speed { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual DateTime TimeCreated { get; set; }

        public virtual LocationType LocationType { get; set; }

        public TypeOfRain TypeOfRain { get; set; }
        public string ExtensionData { get; set; }
    }
}