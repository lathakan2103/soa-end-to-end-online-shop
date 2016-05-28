using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using Moq;
using Demo.Web.Contracts;
using Demo.Web.Controllers.Api;
using Demo.Web.Models;
using System.Security.Principal;
using System.Threading;
using Demo.Client.Entities;
using MyTested.WebApi;

namespace Demo.Web.Tests
{
    [TestClass]
    public class AccountApiControllerTests
    {
        #region Fields

        private HttpRequestMessage _request;
        private Mock<ISecurityAdapter> _securityAdapter;
        private readonly AccountLoginModel _loginModel = new AccountLoginModel
        {
            ReturnUrl = "",
            LoginEmail = "test@test.com",
            Password = "07061971",
            RememberMe = false
        };
        private readonly AccountChangePasswordModel _changePasswordModel = new AccountChangePasswordModel
        {
            LoginEmail = "test@test.com",
            OldPassword = "07061971",
            NewPassword = "123456"
        };
        private readonly AccountRegisterModel _registerModel = new AccountRegisterModel
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

        #endregion

        [TestInitialize]
        public void Initializer()
        {
            this._request = GetRequest();

            // security because of ValidateAuthorizedUser!!!
            var principal = new GenericPrincipal(new GenericIdentity("test@test.com"), new[] { "Administrators" });
            Thread.CurrentPrincipal = principal;

            this._securityAdapter = new Mock<ISecurityAdapter>();
        }

        [TestMethod]
        public void test_login_with_valid_login_email_address()
        {
            this._securityAdapter.Setup(obj => obj.Login(this._loginModel.LoginEmail, this._loginModel.Password, false)).Returns(true);

            MyWebApi
                .Controller<AccountApiController>()
                .WithResolvedDependencyFor<ISecurityAdapter>(this._securityAdapter.Object)
                .Calling(c => c.Login(this._request, this._loginModel))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .HttpResponseMessage()
                .WithStatusCode(HttpStatusCode.OK);
        }

        [TestMethod]
        public void test_login_with_invalid_login_email_address()
        {
            this._securityAdapter.Setup(obj => obj.Login("some@other.email", this._loginModel.Password, false)).Returns(false);

            MyWebApi
                .Controller<AccountApiController>()
                .WithResolvedDependencyFor<ISecurityAdapter>(this._securityAdapter.Object)
                .Calling(c => c.Login(this._request, this._loginModel))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .HttpResponseMessage()
                .WithStatusCode(HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void test_change_password_with_valid_credentials()
        {
            this._securityAdapter.Setup(obj => obj.ChangePassword("test@test.com", "07061971", "123456")).Returns(true);

            MyWebApi
                .Controller<AccountApiController>()
                .WithResolvedDependencyFor<ISecurityAdapter>(this._securityAdapter.Object)
                .WithAuthenticatedUser(u => u.WithUsername(this._changePasswordModel.LoginEmail))
                .Calling(c => c.ChangePassword(this._request, this._changePasswordModel))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .HttpResponseMessage()
                .WithStatusCode(HttpStatusCode.OK);
        }

        [TestMethod]
        public void test_change_password_with_invalid_credentials()
        {
            this._changePasswordModel.LoginEmail = "some@other.email";
            this._securityAdapter.Setup(obj => obj.ChangePassword("test@test.com", "07061971", "123456")).Returns(true);

            MyWebApi
                .Controller<AccountApiController>()
                .WithResolvedDependencyFor<ISecurityAdapter>(this._securityAdapter.Object)
                .WithAuthenticatedUser(u => u.WithUsername(this._changePasswordModel.LoginEmail))
                .Calling(c => c.ChangePassword(this._request, this._changePasswordModel))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .HttpResponseMessage()
                .WithStatusCode(HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public void test_create_account_with_valid_model()
        {
            this._securityAdapter.Setup(obj => obj.Login("test@test.com", "07061971", false)).Returns(true);

            MyWebApi
                .Controller<AccountApiController>()
                .WithResolvedDependencyFor<ISecurityAdapter>(this._securityAdapter.Object)
                .WithAuthenticatedUser(u => u.WithUsername(this._changePasswordModel.LoginEmail))
                .Calling(c => c.CreateAccount(this._request, this._registerModel))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .HttpResponseMessage()
                .WithStatusCode(HttpStatusCode.OK);
        }

        [TestMethod]
        public void test_create_account_with_invalid_model()
        {
            this._registerModel.FirstName = "F";

            MyWebApi
                .Controller<AccountApiController>()
                .WithResolvedDependencyFor<ISecurityAdapter>(this._securityAdapter.Object)
                .Calling(c => c.CreateAccount(this._request, this._registerModel))
                .ShouldReturn()
                .Null();
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
