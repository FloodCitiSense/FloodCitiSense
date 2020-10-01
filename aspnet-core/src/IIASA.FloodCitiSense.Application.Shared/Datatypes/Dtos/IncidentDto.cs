
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class IncidentDto : FullAuditedEntityDto, IExtendableObject
    {
        public int? TenantId { get; set; }
        public string MobileDataId { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }
        [Required]
        public TypeOfRain TypeOfRain { get; set; }

        [Required]
        public List<FloodTypeDto> FloodTypes { get; set; }
        public bool AnySignOfDamage { get; set; }
        public bool AnySignOfObstruction { get; set; }
        public TypesOfSpaceFlooded TypesOfSpaceFlooded { get; set; }
        public FloodExtent FloodExtent { get; set; }
        public FloodDepth FloodDepth { get; set; }
        public FrequencyOfFlood FrequencyOfFlood { get; set; }
        public WaterClarity WaterClarity { get; set; }
        public bool AreYouImpacted { get; set; }

        public List<PictureDto> PictureDtos { get; set; }
        [Required]
        public List<LocationDto> LocationDtos { get; set; }
        public string ExtensionData { get; set; }
    }
}