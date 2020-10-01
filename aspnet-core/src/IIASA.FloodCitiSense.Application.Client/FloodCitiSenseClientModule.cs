using Abp.Modules;
using Abp.Reflection.Extensions;

namespace IIASA.FloodCitiSense
{
    public class FloodCitiSenseClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FloodCitiSenseClientModule).GetAssembly());
        }
    }
}
