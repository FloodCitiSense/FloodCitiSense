using Microsoft.Extensions.Configuration;

namespace IIASA.FloodCitiSense.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
