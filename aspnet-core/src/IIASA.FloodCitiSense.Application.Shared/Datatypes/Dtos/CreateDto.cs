//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CreateDto.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   CreateDto.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class IncidentViewModel
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }

        [Required] public string MobileDataId { get; set; }

        [Required] public DateTimeOffset Date { get; set; }

        [Required] public List<FloodTypeDto> FloodTypes { get; set; }
        public TypeOfRain TypeOfRain { get; set; }

        public bool AnySignOfDamage { get; set; }
        public bool AnySignOfObstruction { get; set; }
        public TypesOfSpaceFlooded TypesOfSpaceFlooded { get; set; }
        public FloodExtent FloodExtent { get; set; }
        public FloodDepth FloodDepth { get; set; }
        public FrequencyOfFlood FrequencyOfFlood { get; set; }
        public WaterClarity WaterClarity { get; set; }
        public bool AreYouImpacted { get; set; }

        public List<string> Images { get; set; }

        public List<PictureDto> PictureDtos { get; set; }

        [Required] public List<LocationDto> LocationDtos { get; set; }
    }
}