using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace Demo.Web.Controllers.Mvc
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeController : ViewControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}