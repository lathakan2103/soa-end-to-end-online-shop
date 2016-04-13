using System.Collections.Generic;
using System.Linq;
using Core.Common.Contracts;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Configuration;

namespace Demo.DataAccess.Tests
{
    /// <summary>
    /// every repository class will have 
    /// one integration test to check if
    /// connectibility to the db ok is
    /// and to test the creation of the repository
    /// through mef with and without repositoryfactory
    /// and only one test method of one functionality of the type
    /// </summary>
    [TestClass]
    public class CustomerRepositoryTests
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

            var repositoryTestClass = new CustomerRepositoryTestClass();

            var customers = repositoryTestClass.GetAllCustomers();

            Assert.IsTrue(customers != null);
            Assert.IsTrue(customers.Count() == 2);
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

            var repositoryTestClass = new CustomerRepositoryFactoryTestClass();

            var customers = repositoryTestClass.GetAllCustomers();

            Assert.IsTrue(customers != null);
            Assert.IsTrue(customers.Count() == 2);
        }

        #endregion

        #region Mocks Repsoitory

        /// <summary>
        /// I will have only one test method with mocking the 
        /// particular repository directly because it will
        /// be done by repositoryfactory in the future
        /// </summary>
        [TestMethod]
        public void test_repository_get()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Age = 44,
                    FirstName = "Aleksandar",
                    LastName = "Ristic"
                },
                new Customer
                {
                    Age = 20,
                    FirstName = "Denis",
                    LastName = "Straus"
                }
            };

            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(obj => obj.Get()).Returns(customers);

            var repositoryTestClass = new CustomerRepositoryTestClass(mockRepository.Object);

            var result = repositoryTestClass.GetAllCustomers();

            Assert.IsTrue(Equals(result, customers));
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
            var customers = new List<Customer>
            {
                new Customer
                {
                    Age = 44,
                    FirstName = "Aleksandar",
                    LastName = "Ristic"
                },
                new Customer
                {
                    Age = 20,
                    FirstName = "Denis",
                    LastName = "Straus"
                }
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>().Get()).Returns(customers);

            var repositoryTestClass = new CustomerRepositoryFactoryTestClass(mockFactory.Object);

            var result = repositoryTestClass.GetAllCustomers();

            Assert.IsTrue(Equals(result, customers));
        }

        [TestMethod]
        public void test_repository_factory_get_by_login()
        {
            var customer = new Customer
            {
                Age = 44,
                FirstName = "Aleksandar",
                LastName = "Ristic",
                LoginEmail = "al.ri@gmx.at"
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>().GetByLogin("al.ri@gmx.at")).Returns(customer);

            var repositoryTestClass = new CustomerRepositoryFactoryTestClass(mockFactory.Object);

            var result = repositoryTestClass.GetByLogin(customer.LoginEmail);

            Assert.IsTrue(Equals(result, customer));
        }

        #endregion
    }
}
