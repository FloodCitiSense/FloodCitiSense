//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CreateOrEditSensorDto.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   CreateOrEditSensorDto.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class CreateOrEditSensorDto : EntityDto<int?>
    {
        [Required] public string Name { get; set; }

        [Required] public long? UserId { get; set; }

        public List<PictureDto> Pictures { get; set; }
        public List<LocationDto> Locations { get; set; }
    }
}