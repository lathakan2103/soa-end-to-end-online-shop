using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace Demo.Web.Controllers.Mvc
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("customer")]
    [Authorize]
    public class CustomerController : ViewControllerBase
    {
        #region HttpGet

        [HttpGet]
        [Route("settings")]
        public ActionResult Settings()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Buy()
        {
            return View();
        }

        [HttpGet]
        [Route("shoppinghistory")]
        public ActionResult ShoppingHistory()
        {
            return View();
        }

        #endregion
    }
}
