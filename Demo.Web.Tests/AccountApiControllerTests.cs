using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using Moq;
using Demo.Web.Contracts;
using Demo.Web.Controllers.Api;
using Demo.Web.Models;
using System.Security.Principal;
using System.Threading;

namespace Demo.Web.Tests
{
    [TestClass]
    public class AccountApiControllerTests
    {
        private HttpRequestMessage _Request = null;

        [TestInitialize]
        public void Initializer()
        {
            this._Request = GetRequest();

            // security because of ValidateAuthorizedUser!!!
            var principal = new GenericPrincipal(new GenericIdentity("test@test.com"), new[] { "Administrators" });
            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void test_login()
        {
            var accountLoginModel = new AccountLoginModel
            {
                LoginEmail = "test@test.com",
                Password = "07061971",
                RememberMe = false
            };            

            var securityAdapter = new Mock<ISecurityAdapter>();
            securityAdapter.Setup(obj => obj.Login("test@test.com", "07061971", false)).Returns(true);

            var controller = new AccountApiController(securityAdapter.Object);
            var response = controller.Login(this._Request, accountLoginModel);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            accountLoginModel.LoginEmail = "xxx@test.com";
            response = controller.Login(this._Request, accountLoginModel);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void test_change_password()
        {
            var model = new AccountChangePasswordModel
            {
                LoginEmail = "test@test.com",
                OldPassword = "07061971",
                NewPassword = "123456"
            };

            var securityAdapter = new Mock<ISecurityAdapter>();
            securityAdapter.Setup(obj => obj.ChangePassword("test@test.com", "07061971", "123456")).Returns(true);

            var controller = new AccountApiController(securityAdapter.Object);
            var response = controller.ChangePassword(this._Request, model);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            model.LoginEmail = "1111111";
            response = controller.ChangePassword(this._Request, model);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void test_create_account()
        {
            var model = new AccountRegisterModel
            {
                FirstName = "Aleksandar",
                LastName = "Ristic",
                Age = 44,
                City = "Vienna",
                CreditCard = "1234123412341234",
                ExpirationDate = "12/18",
                Hausnumber = "56",
                IsActive = true,
                LoginEmail = "test@test.com",
                Password = "07061971",
                State = "NY",
                Street = "Mainstreet",
                ZipCode = "11010" 
            };

            var securityAdapter = new Mock<ISecurityAdapter>();
            securityAdapter.Setup(obj => obj.Login("test@test.com", "07061971", false)).Returns(true);

            var controller = new AccountApiController(securityAdapter.Object);
            var response = controller.CreateAccount(this._Request, model);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        #region Helpers

        HttpRequestMessage GetRequest()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.Properties["MS_HttpConfiguration"] = config;
            return request;
        }

        T GetResponseData<T>(HttpResponseMessage result)
        {
            var content = result.Content as ObjectContent<T>;
            if (content != null)
            {
                T data = (T)(content.Value);
                return data;
            }
            else
            {
                return default(T);
            }
        }

        #endregion
    }
}
