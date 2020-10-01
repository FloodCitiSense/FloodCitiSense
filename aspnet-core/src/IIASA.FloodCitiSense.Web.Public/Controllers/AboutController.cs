using Microsoft.AspNetCore.Mvc;
using IIASA.FloodCitiSense.Web.Controllers;

namespace IIASA.FloodCitiSense.Web.Public.Controllers
{
    public class AboutController : FloodCitiSenseControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}