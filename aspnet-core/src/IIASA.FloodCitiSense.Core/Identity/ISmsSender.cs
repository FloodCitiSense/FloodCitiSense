using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Identity
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}