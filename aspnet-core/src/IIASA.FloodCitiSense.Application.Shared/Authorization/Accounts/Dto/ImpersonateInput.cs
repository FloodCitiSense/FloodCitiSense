using System.ComponentModel.DataAnnotations;

namespace IIASA.FloodCitiSense.Authorization.Accounts.Dto
{
    public class ImpersonateInput
    {
        public int? TenantId { get; set; }

        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
    }
}