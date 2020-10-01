using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}