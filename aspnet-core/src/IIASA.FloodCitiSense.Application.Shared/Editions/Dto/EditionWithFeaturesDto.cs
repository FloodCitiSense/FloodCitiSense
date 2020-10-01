using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Editions.Dto
{
    public class EditionWithFeaturesDto
    {
        public EditionWithFeaturesDto()
        {
            FeatureValues = new List<NameValueDto>();
        }

        public EditionSelectDto Edition { get; set; }

        public List<NameValueDto> FeatureValues { get; set; }
    }
}