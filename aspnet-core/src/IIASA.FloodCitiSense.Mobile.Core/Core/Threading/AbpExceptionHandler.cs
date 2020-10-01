using Abp.Web.Models;
using Acr.UserDialogs;
using Flurl.Http;
using IIASA.FloodCitiSense.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.Threading
{
    public class AbpExceptionHandler
    {
        public static async Task<bool> HandleIfAbpResponseAsync(FlurlHttpException httpException)
        {
            var errorResponse = await httpException.GetResponseStringAsync();
            if (errorResponse == null)
            {
                return false;
            }

            if (!errorResponse.Contains("__abp"))
            {
                return false;
            }

            var ajaxResponse = JsonConvert.DeserializeObject<AjaxResponse>(errorResponse);

            if (ajaxResponse?.Error == null)
            {
                return false;
            }

            UserDialogs.Instance.HideLoading();
            if (string.IsNullOrEmpty(ajaxResponse.Error.Details))
            {
                UserDialogs.Instance.Alert(ajaxResponse.Error.GetConsolidatedMessage(), "Error".Translate());
            }
            else
            {
                UserDialogs.Instance.Alert(ajaxResponse.Error.Details, ajaxResponse.Error.GetConsolidatedMessage());
            }

            return true;
        }
    }
}