using System.Threading.Tasks;
using IIASA.FloodCitiSense.Sessions.Dto;

namespace IIASA.FloodCitiSense.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
