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
    public class CartItemRepositoryTests
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

            var repositoryTestClass = new CartItemRepositoryTestClass();

            var items = repositoryTestClass.GetAllCartItems();

            Assert.IsTrue(items != null);
            Assert.IsTrue(items.Count() == 2);
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

            var repositoryTestClass = new CartItemRepositoryFactoryTestClass();

            var items = repositoryTestClass.GetAllCartItems();

            Assert.IsTrue(items != null);
            Assert.IsTrue(items.Count() == 2);
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
            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    CartId = 1
                },
                new CartItem
                {
                    CartId = 1
                }
            };

            // cart items
            var mockRepository = new Mock<ICartItemRepository>();
            mockRepository.Setup(obj => obj.Get()).Returns(cartItems);
            var repositoryTestClass = new CartItemRepositoryTestClass(mockRepository.Object);
            var resultItems = repositoryTestClass.GetAllCartItems();

            Assert.IsNotNull(resultItems);
            Assert.IsTrue(resultItems.Count() == 2);
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
            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    CartId = 1
                },
                new CartItem
                {
                    CartId = 1
                }
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICartItemRepository>().Get()).Returns(cartItems);

            var repositoryTestClass = new CartItemRepositoryFactoryTestClass(mockFactory.Object);

            var result = repositoryTestClass.GetAllCartItems();

            Assert.IsTrue(Equals(result, cartItems));
        }

        #endregion
    }
}
