using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace IIASA.FloodCitiSense.Web.Controllers
{
    public class HomeController : FloodCitiSenseControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
