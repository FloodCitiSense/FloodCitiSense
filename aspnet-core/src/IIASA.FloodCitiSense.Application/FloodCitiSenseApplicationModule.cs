using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using IIASA.FloodCitiSense.Authorization;

namespace IIASA.FloodCitiSense
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(FloodCitiSenseCoreModule)
        )]
    public class FloodCitiSenseApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FloodCitiSenseApplicationModule).GetAssembly());
        }
    }
}