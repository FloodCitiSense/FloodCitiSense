using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
