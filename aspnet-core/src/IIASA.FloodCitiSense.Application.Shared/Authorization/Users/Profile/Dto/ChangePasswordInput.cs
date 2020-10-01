using Abp.Auditing;
using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Authorization.Users.Profile.Dto
{
    public class ChangePasswordInput
    {
        [Required]
        [DisableAuditing]
        public string CurrentPassword { get; set; }

        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}