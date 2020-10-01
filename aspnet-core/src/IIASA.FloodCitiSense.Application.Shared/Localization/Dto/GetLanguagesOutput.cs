using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Localization.Dto
{
    public class GetLanguagesOutput : ListResultDto<ApplicationLanguageListDto>
    {
        public string DefaultLanguageName { get; set; }

        public GetLanguagesOutput()
        {

        }

        public GetLanguagesOutput(IReadOnlyList<ApplicationLanguageListDto> items, string defaultLanguageName)
            : base(items)
        {
            DefaultLanguageName = defaultLanguageName;
        }
    }
}