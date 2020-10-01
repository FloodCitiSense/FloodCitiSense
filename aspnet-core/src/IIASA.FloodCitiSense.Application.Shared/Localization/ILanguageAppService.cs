using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Localization.Dto;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Localization
{
    public interface ILanguageAppService : IApplicationService
    {
        Task<GetLanguagesOutput> GetLanguages();

        Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input);

        Task CreateOrUpdateLanguage(CreateOrUpdateLanguageInput input);

        Task DeleteLanguage(EntityDto input);

        Task SetDefaultLanguage(SetDefaultLanguageInput input);

        Task<PagedResultDto<LanguageTextListDto>> GetLanguageTexts(GetLanguageTextsInput input);

        Task UpdateLanguageText(UpdateLanguageTextInput input);
    }
}
