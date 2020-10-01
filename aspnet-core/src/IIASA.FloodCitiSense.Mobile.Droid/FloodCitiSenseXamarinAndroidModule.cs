﻿using Abp.Modules;
using Abp.Reflection.Extensions;

namespace IIASA.FloodCitiSense
{
    [DependsOn(typeof(FloodCitiSenseXamarinSharedModule))]
    public class FloodCitiSenseXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FloodCitiSenseXamarinAndroidModule).GetAssembly());
        }
    }
}