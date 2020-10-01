using Abp.AspNetCore.Mvc.ViewComponents;

namespace IIASA.FloodCitiSense.Web.Public.Views
{
    public abstract class FloodCitiSenseViewComponent : AbpViewComponent
    {
        protected FloodCitiSenseViewComponent()
        {
            LocalizationSourceName = FloodCitiSenseConsts.LocalizationSourceName;
        }
    }
}