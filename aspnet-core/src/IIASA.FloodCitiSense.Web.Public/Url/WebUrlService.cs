using Abp.Dependency;
using IIASA.FloodCitiSense.Configuration;
using IIASA.FloodCitiSense.Url;
using IIASA.FloodCitiSense.Web.Url;

namespace IIASA.FloodCitiSense.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}