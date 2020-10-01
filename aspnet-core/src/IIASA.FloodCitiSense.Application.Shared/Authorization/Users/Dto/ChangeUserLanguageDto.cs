using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
