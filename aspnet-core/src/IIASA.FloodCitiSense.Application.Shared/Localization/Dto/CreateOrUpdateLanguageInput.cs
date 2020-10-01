using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}