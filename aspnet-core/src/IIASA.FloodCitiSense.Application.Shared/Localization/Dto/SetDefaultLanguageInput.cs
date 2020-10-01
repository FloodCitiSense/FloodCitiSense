using Abp.Localization;
using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Localization.Dto
{
    public class SetDefaultLanguageInput
    {
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }
    }
}