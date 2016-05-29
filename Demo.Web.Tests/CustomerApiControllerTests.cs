using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Principal;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Moq;
using Demo.Client.Contracts;
using Demo.Client.Entities;
using Demo.Web.Controllers.Api;
using MyTested.WebApi;

namespace Demo.Web.Tests
{
    [TestClass]
    public class CustomerApiControllerTests
    {
        #region Fields

        private HttpRequestMessage _request;
        private Mock<ICustomerService> _customerService;
        private readonly Customer _customer = new Customer
        {
            LoginEmail = "test@test.com",
            ExpirationDate = "12/18",
            State = "NY",
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

            this._customerService = new Mock<ICustomerService>();
        }

        [TestMethod]
        public void test_get_customer_account_info()
        {
            this._customerService.Setup(obj => obj.GetCustomerByLogin("test@test.com")).Returns(this._customer);

            MyWebApi
                .Controller<CustomerApiController>()
                .WithResolvedDependencyFor(this._customerService.Object)
                .WithAuthenticatedUser(u => u.WithUsername(this._customer.LoginEmail))
                .Calling(c => c.GetCustomerAccountInfo(this._request))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .HttpResponseMessage()
                .WithStatusCode(HttpStatusCode.OK)
                .WithResponseModelOfType<Customer>()
                .Passing(m =>
                {
                    Assert.AreSame(m, this._customer);
                });
        }

        [TestMethod]
        public void test_update_customer_account_info()
        {
            this._customerService.Setup(obj => obj.UpdateCustomer(this._customer)).Verifiable();

            MyWebApi
                .Controller<CustomerApiController>()
                .WithResolvedDependencyFor(this._customerService.Object)
                .WithAuthenticatedUser(u => u.WithUsername(this._customer.LoginEmail))
                .Calling(c => c.UpdateCustomerAccountInfo(this._request, this._customer))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .HttpResponseMessage()
                .WithStatusCode(HttpStatusCode.OK);
        }

        #region Helpers

        HttpRequestMessage GetRequest()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.Properties["MS_HttpConfiguration"] = config;
            return request;
        }

        #endregion
    }
}
