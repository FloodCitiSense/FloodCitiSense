using Abp.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Authorization.Accounts.Dto
{
    public class IsTenantAvailableInput
    {
        [Required]
        [MaxLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}