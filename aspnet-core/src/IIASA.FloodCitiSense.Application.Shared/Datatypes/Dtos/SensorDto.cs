using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class SensorDto : EntityDto
    {
        public string Name { get; set; }
        public long UserId { get; set; }
        public List<PictureDto> Pictures { get; set; }
        public List<LocationDto> Locations { get; set; }
    }
}