using System;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.Entity
{
    public class GeoCoordinate
    {
        /// <summary>
        ///     Gets or sets the latitude.
        /// </summary>
        /// <value>
        ///     The latitude.
        /// </value>
        public double Latitude { get; set; }

        /// <summary>
        ///     Gets or sets the longitude.
        /// </summary>
        /// <value>
        ///     The longitude.
        /// </value>
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double AltitudeAccuracy { get; set; }
        public double Accuracy { get; set; }
        public double Heading { get; set; }
        public double Speed { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public string IncidentId { get; set; }

    }
}