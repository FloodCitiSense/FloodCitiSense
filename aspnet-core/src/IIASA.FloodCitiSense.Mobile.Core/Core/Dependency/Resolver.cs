using Abp.Dependency;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.Dependency
{
    public static class Resolver
    {
        public static IIocManager IocManager => ApplicationBootstrapper.AbpBootstrapper.IocManager;

        public static T Resolve<T>()
        {
            return ApplicationBootstrapper.AbpBootstrapper.IocManager.Resolve<T>();
        }

        public static object Resolve(System.Type type)
        {
            return ApplicationBootstrapper.AbpBootstrapper.IocManager.Resolve(type);
        }
    }
}