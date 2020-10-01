//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CreateDto.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   CreateDto.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Abp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class IncidentEditDto : IExtendableObject
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string MobileDataId { get; set; }
        public DateTimeOffset Date { get; set; }
        public TypeOfRain TypeOfRain { get; set; }
        public List<FloodTypeDto> FloodTypes { get; set; }
        public bool AnySignOfDamage { get; set; }
        public bool AnySignOfObstruction { get; set; }
        public TypesOfSpaceFlooded TypesOfSpaceFlooded { get; set; }
        public FloodExtent FloodExtent { get; set; }
        public FloodDepth FloodDepth { get; set; }
        public FrequencyOfFlood FrequencyOfFlood { get; set; }
        public WaterClarity WaterClarity { get; set; }
        public bool AreYouImpacted { get; set; }

        public List<string> Images { get; set; }
        public List<LocationDto> LocationDtos { get; set; }
        public string ExtensionData { get; set; }
    }
}