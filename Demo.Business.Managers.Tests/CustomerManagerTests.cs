using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Principal;
using System.Threading;
using Moq;
using Core.Common.Contracts;
using Demo.Data.Contracts;
using Demo.Business.Entities;

namespace Demo.Business.Managers.Tests
{
    [TestClass]
    public class CustomerManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            var principal = new GenericPrincipal(new GenericIdentity("Pingo"), new[] { "Administrators", "DemoAdmin" });
            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void test_get_customer_by_login()
        {
            var customer = new Customer { LoginEmail = "test@test.com" };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>().GetByLogin(customer.LoginEmail)).Returns(customer);

            var manager = new CustomerManager(mockFactory.Object);
            var result = manager.GetCustomerByLogin("test@test.com");

            Assert.AreEqual(result, customer);
        }

        [TestMethod]
        public void test_get_customers()
        {
            var customers = new Customer[]
            {
                new Customer
                {
                    CustomerId = 4
                },
                new Customer
                {
                    CustomerId = 10
                }
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>().Get()).Returns(customers);

            var manager = new CustomerManager(mockFactory.Object);
            var result = manager.GetCustomers();

            Assert.IsTrue(result.Length == 2);
            Assert.IsTrue(result[0].CustomerId == 4);
            Assert.IsTrue(result[1].CustomerId == 10);
        }

        [TestMethod]
        public void test_get_active_customers()
        {
            var customers = new Customer[]
            {
                new Customer
                {
                    CustomerId = 4,
                    IsActive = true
                },
                new Customer
                {
                    CustomerId = 10
                }
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>().Get()).Returns(customers);

            var manager = new CustomerManager(mockFactory.Object);
            var result = manager.GetActiveCustomers();

            Assert.IsTrue(result.Length == 1);
            Assert.IsTrue(result[0].CustomerId == 4);
        }

        [TestMethod]
        public void test_update_customer_existed_customer()
        {
            var customer = new Customer { CustomerId = 1 };
            var updatedCustomer = new Customer { CustomerId = 1 };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>().Update(customer)).Returns(updatedCustomer);

            var manager = new CustomerManager(mockFactory.Object);
            var result = manager.UpdateCustomer(customer);

            Assert.IsTrue(result == updatedCustomer);
        }

        [TestMethod]
        public void test_update_customer_add_new_customer()
        {
            var customer = new Customer { CustomerId = 0 };
            var addedCustomer = new Customer { CustomerId = 1 };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>().Add(customer)).Returns(addedCustomer);

            var manager = new CustomerManager(mockFactory.Object);
            var result = manager.UpdateCustomer(customer);

            Assert.IsTrue(result == addedCustomer);
        }

        [TestMethod]
        public void test_delete_customer()
        {
            var customer = new Customer { CustomerId = 1, IsActive = true };
            var deletedCustomer = new Customer { CustomerId = 1, IsActive = false };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>().Get(1)).Returns(customer);

            var manager = new CustomerManager(mockFactory.Object);
            manager.DeleteCustomer(1);

            mockFactory.VerifyAll();
        }
    }
}
