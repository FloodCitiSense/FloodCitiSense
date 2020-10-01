using Abp.AutoMapper;
using Abp.Modules;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using IIASA.FloodCitiSense.Mobile.Core.Services.Core;
using System.Reflection;

namespace IIASA.FloodCitiSense.Mobile.Core
{
    [DependsOn(typeof(FloodCitiSenseClientModule), typeof(AbpAutoMapperModule))]
    public class FloodCitiSenseXamarinCoreModule : AbpModule
    {
        public override void PostInitialize()
        {
            base.PostInitialize();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Register(typeof(IDbService<>),
              typeof(LiteDbService<>),
              Abp.Dependency.DependencyLifeStyle.Transient);
        }
    }
}
