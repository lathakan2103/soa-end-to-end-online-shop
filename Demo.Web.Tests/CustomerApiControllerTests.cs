using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Principal;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Moq;
using Demo.Client.Contracts;
using Demo.Client.Entities;
using Demo.Web.Controllers.Api;

namespace Demo.Web.Tests
{
    [TestClass]
    public class CustomerApiControllerTests
    {
        #region Fields

        private HttpRequestMessage _Request = null;
        private Mock<ICustomerService> _customerService;
        private CustomerApiController _controller;

        #endregion

        [TestInitialize]
        public void Initializer()
        {
            this._Request = GetRequest();

            // security because of ValidateAuthorizedUser!!!
            var principal = new GenericPrincipal(new GenericIdentity("test@test.com"), new[] { "Administrators" });
            Thread.CurrentPrincipal = principal;

            this._customerService = new Mock<ICustomerService>();
            this._controller = new CustomerApiController(this._customerService.Object);
        }

        [TestMethod]
        public void test_get_customer_account_info()
        {
            var customer = new Customer { LoginEmail = "test@test.com", ExpirationDate = "1218" };

            this._customerService.Setup(obj => obj.GetCustomerByLogin("test@test.com")).Returns(customer);
            var response = this._controller.GetCustomerAccountInfo(this._Request);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var data = GetResponseData<Customer>(response);

            Assert.IsTrue(data == customer);
        }

        [TestMethod]
        public void test_update_customer_account_info()
        {
            var customer = new Customer
            {
                LoginEmail = "test@test.com",
                ExpirationDate = "12/18",
                State = "NY",
                ZipCode = "11010"
            };

            this._customerService.Setup(obj => obj.UpdateCustomer(customer));
            var response = this._controller.UpdateCustomerAccountInfo(this._Request, customer);

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
