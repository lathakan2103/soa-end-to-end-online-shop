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
    public class ProductRepositorryTests
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

            var repositoryTestClass = new ProductRepositoryTestClass();

            var products = repositoryTestClass.GetAllProducts();

            Assert.IsTrue(products != null);
            Assert.IsTrue(products.Count() == 6);
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

            var repositoryTestClass = new ProductRepositoryFactoryClass();

            var products = repositoryTestClass.GetAllProducts();

            Assert.IsTrue(products != null);
            Assert.IsTrue(products.Count() == 6);
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
            var products = new List<Product>
            {
                new Product
                {
                    ArticleNumber = "123456",
                    Name = "Shoe",
                    Price = new decimal(100)
                },
                new Product
                {
                    ArticleNumber = "78910",
                    Name = "Hat",
                    Price = new decimal(50)
                }
            };

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(obj => obj.Get()).Returns(products);

            var repositoryTestClass = new ProductRepositoryTestClass(mockRepository.Object);

            var result = repositoryTestClass.GetAllProducts();

            Assert.IsTrue(Equals(result, products));
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
            var products = new List<Product>
            {
                new Product
                {
                    ArticleNumber = "123456",
                    Name = "Shoe",
                    Price = new decimal(100)
                },
                new Product
                {
                    ArticleNumber = "78910",
                    Name = "Hat",
                    Price = new decimal(50)
                }
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<IProductRepository>().Get()).Returns(products);

            var repositoryTestClass = new ProductRepositoryFactoryClass(mockFactory.Object);

            var result = repositoryTestClass.GetAllProducts();

            Assert.IsTrue(Equals(result, products));
        }

        #endregion
    }
}
