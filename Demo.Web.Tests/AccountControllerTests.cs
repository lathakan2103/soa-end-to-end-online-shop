using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Web.Contracts;
using Moq;
using Demo.Web.Controllers.Mvc;
using Demo.Web.Models;
using TestStack.FluentMVCTesting;

namespace Demo.Web.Tests
{
    [TestClass]
    public class AccountControllerTests
    {
        #region Field

        private Mock<ISecurityAdapter> _securityAdapter;
        private AccountController _controller;
        private const string ReturnUrl = "/testreturnurl";

        #endregion

        [TestInitialize]
        public void TestInitializer()
        {
            this._securityAdapter = new Mock<ISecurityAdapter>();
            this._securityAdapter.Setup(obj => obj.Initialize());
            this._controller = new AccountController(this._securityAdapter.Object);
        }

        [TestMethod]
        public void test_login()
        {
            this._controller
                .WithCallTo(x => x.Login(ReturnUrl))
                .ShouldRenderDefaultView() // Login View
                .WithModel<AccountLoginModel>(m => m.ReturnUrl.Equals(ReturnUrl));
        }

        [TestMethod]
        public void test_register()
        {
            this._controller
                .WithCallTo(c => c.Register())
                .ShouldRenderDefaultView(); // Register View
        }
    }
}
