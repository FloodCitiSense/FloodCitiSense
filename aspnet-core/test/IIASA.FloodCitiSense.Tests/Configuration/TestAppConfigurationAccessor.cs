using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using IIASA.FloodCitiSense.Configuration;

namespace IIASA.FloodCitiSense.Tests.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(FloodCitiSenseTestModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
