using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.DataTypes.Dtos
{
    public class CreateOrEditCityDto : EntityDto<int?>
    {

        public string Name { get; set; }


        public long? UserId { get; set; }


    }
}