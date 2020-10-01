// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Incident.cs" company="IIASA">
//   EOS
// </copyright>
// <summary>
//   Defines the Incident.cs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace IIASA.FloodCitiSense.Datatypes
{
    /// <summary>
    ///     Defines the <see cref="Incident" />
    /// </summary>
    [Table("Incidents")]
    public class Incident : FullAuditedEntity, IMayHaveTenant, IExtendableObject
    {
        /// <summary>
        ///     Gets or sets the MobileDataId
        /// </summary>
        public Guid MobileDataId { get; set; }

        // Mobile Data
        /// <summary>
        ///     Gets or sets the TypeOfRain
        /// </summary>
        public virtual TypeOfRain TypeOfRain { get; set; }

        /// <summary>
        ///     Gets or sets the TypeOfFlooding
        /// </summary>
        public virtual TypeOfFlood TypeOfFlooding { get; set; }

        public virtual List<FloodType> FloodTypes { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether AnySignOfDamage
        /// </summary>
        public virtual bool AnySignOfDamage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether AnySignOfObstruction
        /// </summary>
        public virtual bool AnySignOfObstruction { get; set; }

        /// <summary>
        ///     Gets or sets the TypesOfSpaceFlooded
        /// </summary>
        public virtual TypesOfSpaceFlooded TypesOfSpaceFlooded { get; set; }

        /// <summary>
        ///     Gets or sets the FloodExtent
        /// </summary>
        public virtual FloodExtent FloodExtent { get; set; }

        /// <summary>
        ///     Gets or sets the FloodDepth
        /// </summary>
        public virtual FloodDepth FloodDepth { get; set; }

        /// <summary>
        ///     Gets or sets the FrequencyOfFlood
        /// </summary>
        public virtual FrequencyOfFlood FrequencyOfFlood { get; set; }

        /// <summary>
        ///     Gets or sets the WaterClarity
        /// </summary>
        public virtual WaterClarity WaterClarity { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether AreYouImpacted
        /// </summary>
        public virtual bool AreYouImpacted { get; set; }

        /// <summary>
        ///     Gets or sets the TimeCreated
        /// </summary>
        public virtual DateTime TimeCreated { get; set; }

        /// <summary>
        ///     Gets or sets the Locations
        /// </summary>
        public virtual List<Location> Locations { get; set; }

        /// <summary>
        ///     Gets or sets the Pictures
        /// </summary>
        public virtual List<Picture> Pictures { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether IsApproved
        /// </summary>
        public virtual bool IsApproved { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether IsRejected
        /// </summary>
        public virtual bool IsRejected { get; set; }

        /// <summary>
        ///     Gets or sets the ApproveUserId
        /// </summary>
        public virtual long? ApproveUserId { get; set; }

        /// <summary>
        ///     Gets or sets the ExtensionData
        /// </summary>
        public string ExtensionData { get; set; }

        /// <summary>
        ///     Gets or sets the TenantId
        /// </summary>
        public int? TenantId { get; set; }
    }
}