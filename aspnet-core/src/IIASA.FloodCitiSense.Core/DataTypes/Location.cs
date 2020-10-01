using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using JetBrains.Annotations;
using NetTopologySuite.Geometries;

namespace IIASA.FloodCitiSense.Datatypes
{
    [Table("Locations")]
    public class Location : FullAuditedEntity, IMayHaveTenant, IExtendableObject
    {
        [CanBeNull] public Incident Incident { get; set; }
        [CanBeNull] public Sensor Sensor { get; set; }
        public virtual double Latitude { get; set; }

        public virtual double Longitude { get; set; }

        public Point Point { get; set; }

        public virtual double Accuracy { get; set; }

        public double Altitude { get; set; }
        public double AltitudeAccuracy { get; set; }
        public double Heading { get; set; }
        public double Speed { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual LocationType LocationType { get; set; }
        
        public int? TenantId { get; set; }

        public string ExtensionData { get; set; }
    }
}