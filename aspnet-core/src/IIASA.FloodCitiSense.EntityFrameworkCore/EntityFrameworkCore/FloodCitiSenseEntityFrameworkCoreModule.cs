using Abp;
using Abp.EntityFrameworkCore.Configuration;
using Abp.IdentityServer4;
using Abp.Modules;
using Abp.Organizations;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using IIASA.FloodCitiSense.Authorization.Roles;
using IIASA.FloodCitiSense.Configuration;
using IIASA.FloodCitiSense.Migrations.Seed;
using IIASA.FloodCitiSense.MultiTenancy;

namespace IIASA.FloodCitiSense.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(FloodCitiSenseCoreModule),
        typeof(AbpZeroCoreIdentityServerEntityFrameworkCoreModule)
        )]
    public class FloodCitiSenseEntityFrameworkCoreModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<FloodCitiSenseDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        FloodCitiSenseDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        FloodCitiSenseDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }

            //Uncomment below line to write change logs for the entities below:
            //Configuration.EntityHistory.Selectors.Add("FloodCitiSenseEntities", typeof(OrganizationUnit), typeof(Role), typeof(Tenant));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FloodCitiSenseEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();
            if (!SkipDbSeed && DatabaseCheckHelper.Exist(configurationAccessor.Configuration["ConnectionStrings:Default"]))
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
