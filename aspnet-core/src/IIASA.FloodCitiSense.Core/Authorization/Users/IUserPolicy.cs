using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace IIASA.FloodCitiSense.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
