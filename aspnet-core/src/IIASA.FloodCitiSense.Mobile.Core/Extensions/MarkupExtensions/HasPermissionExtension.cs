using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Services.Permission;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = Resolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}