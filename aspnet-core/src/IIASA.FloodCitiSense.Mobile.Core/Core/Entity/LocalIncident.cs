using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using System;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.Entity
{
    public class LocalIncident
    {
        public LocalIncident()
        {
            if (string.IsNullOrEmpty(Status))
            {
                Status = "Received".Translate();
            }

            TypeOfFloodings = new List<int>();
            Images = new List<string>();
        }

        public string Id { get; set; }

        public int IncidentId { get; set; }

        public string MobileDataId { get; set; }

        public int UserId { get; set; }
        public DateTimeOffset Date { get; set; }
        public int TypeOfRain { get; set; }
        public int TypeOfFlooding { get; set; }

        public List<int> TypeOfFloodings { get; set; }

        public List<string> Images { get; set; }
        public List<LocalImage> LocalImages { get; set; }
        public bool AnySignOfDamage { get; set; }
        public bool AnySignOfObstruction { get; set; }
        public int TypesOfSpaceFlooded { get; set; }
        public int FloodExtent { get; set; }
        public int FloodDepth { get; set; }
        public int FrequencyOfFlood { get; set; }
        public int WaterClarity { get; set; }
        public string Status { get; set; }
        public bool AreYouImpacted { get; set; }
        public GeoCoordinate IncidentLocation { get; set; }
        public GeoCoordinate UserLocation { get; set; }

        public bool IsUploaded { get; set; }
        public bool IsEdited { get; set; }
        public bool IsCreated { get; set; }
        public bool IsLocal { get; set; }
    }
}