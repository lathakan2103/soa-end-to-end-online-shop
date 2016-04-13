using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Web.Contracts;
using Moq;
using Demo.Web.Controllers.Mvc;
using System.Web.Mvc;
using Demo.Web.Models;

namespace Demo.Web.Tests
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void test_login()
        {
            var securityAdapter = new Mock<ISecurityAdapter>();
            securityAdapter.Setup(obj => obj.Initialize());

            string returnUrl = "/testreturnurl";

            var accountController = new AccountController(securityAdapter.Object);
            var actionResult = accountController.Login(returnUrl);

            Assert.IsTrue(actionResult is ViewResult);

            var viewResult = actionResult as ViewResult;

            Assert.IsTrue(viewResult.Model is AccountLoginModel);

            var model = viewResult.Model as AccountLoginModel;

            Assert.IsTrue(model.ReturnUrl == returnUrl);
        }

        [TestMethod]
        public void test_register()
        {
            var securityAdapter = new Mock<ISecurityAdapter>();
            securityAdapter.Setup(obj => obj.Initialize());

            var accountController = new AccountController(securityAdapter.Object);
            var actionResult = accountController.Register();

            Assert.IsTrue(actionResult is ViewResult);
        }
    }
}
