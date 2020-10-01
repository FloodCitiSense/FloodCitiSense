using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace IIASA.FloodCitiSense.Web.Public.Views
{
    public abstract class FloodCitiSenseRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected FloodCitiSenseRazorPage()
        {
            LocalizationSourceName = FloodCitiSenseConsts.LocalizationSourceName;
        }
    }
}
