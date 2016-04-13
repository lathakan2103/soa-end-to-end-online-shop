using Demo.Web.Contracts;
using Demo.Web.Models;
using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace Demo.Web.Controllers.Mvc
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("account")]
    public class AccountController : ViewControllerBase
    {
        #region Fields

        private readonly ISecurityAdapter _securityAdapter;

        #endregion

        #region C-Tor

        [ImportingConstructor]
        public AccountController(ISecurityAdapter securityAdapter)
        {
            this._securityAdapter = securityAdapter;
        }

        #endregion

        #region HttpGet

        [HttpGet]
        [Route("login")]
        public ActionResult Login(string returnUrl)
        {
            this._securityAdapter.Initialize();
            return View(new AccountLoginModel { ReturnUrl = returnUrl });
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            this._securityAdapter.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            this._securityAdapter.Initialize();
            return View();
        }

        [HttpGet]
        [Route("changepassword")]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        #endregion
    }
}
