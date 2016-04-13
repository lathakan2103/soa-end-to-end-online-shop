using System.Collections.Generic;
using System.Linq;
using Core.Common.Contracts;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using Demo.DataAccess.Tests.Helper_Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Configuration;

namespace Demo.DataAccess.Tests
{
    [TestClass]
    public class CartRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = Business.Bootstrapper.MefLoader.Init();
        }

        private bool IntegrationTestedActivated
        {
            get
            {
                return ConfigurationSettings.AppSettings["doIntegrationTests"] == "true";
            }
        }

        #region Integration tests

        /// <summary>
        /// we will have only one integration test at the start
        /// because we just want to test the connectibility
        /// to the db and the job of ef code first here
        /// </summary>
        [TestMethod]
        public void test_repository_usage_of_the_db()
        {
            if (!IntegrationTestedActivated)
                return;

            var repositoryTestClass = new CartRepositoryTestClass();

            var carts = repositoryTestClass.GetAllCarts();

            Assert.IsTrue(carts != null);
            Assert.IsTrue(carts.Count() == 1);
        }

        /// <summary>
        /// we will have only one integration test at the start
        /// because we just want to test the connectibility
        /// to the db and the job of ef code first here
        /// </summary>
        [TestMethod]
        public void test_repository_factory_usage_of_the_db()
        {
            if (!IntegrationTestedActivated)
                return;

            var repositoryTestClass = new CartRepositoryFactoryTestClass();

            var carts = repositoryTestClass.GetAllCarts();

            Assert.IsTrue(carts != null);
            Assert.IsTrue(carts.Count() == 1);
        }

        [TestMethod]
        public void test_get_shopping_info_for_customer()
        {
            if (!IntegrationTestedActivated)
                return;

            var customer = new Customer { CustomerId = 3 };

            var repository = new CartRepositoryTestClass();

            var historyInfo = repository.GetCustomerShoppingInfo(customer.CustomerId);

            Assert.IsTrue(historyInfo != null);
            Assert.IsTrue(historyInfo.CartList.Count() == 2);
        }

        #endregion

        #region Mocks Repository

        /// <summary>
        /// I will have only one test method with mocking the 
        /// particular repository directly because it will
        /// be done by repositoryfactory in the future
        /// </summary>
        [TestMethod]
        public void test_repository_get()
        {
            var carts = new List<Cart>
            {
                new Cart
                {
                    CartId = 1
                },
                new Cart
                {
                    CartId = 2
                }
            };

            var mockRepository = new Mock<ICartRepository>();
            mockRepository.Setup(obj => obj.Get()).Returns(carts);

            var repositoryTestClass = new CartRepositoryTestClass(mockRepository.Object);

            var result = repositoryTestClass.GetAllCarts();

            Assert.IsTrue(Equals(result, carts));
        }

        #endregion

        #region Mock RepositoryFactory

        /// <summary>
        /// we don't want to test complete repository functionality 
        /// because it will be tested in our service unit tests
        /// </summary>
        [TestMethod]
        public void test_repository_factory_get()
        {
            var carts = new List<Cart>
            {
                new Cart
                {
                    CartId = 1
                },
                new Cart
                {
                    CartId = 2
                }
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Get()).Returns(carts);

            var repositoryTestClass = new CartRepositoryFactoryTestClass(mockFactory.Object);

            var result = repositoryTestClass.GetAllCarts();

            Assert.IsTrue(Equals(result, carts));
        }

        #endregion
    }
}
