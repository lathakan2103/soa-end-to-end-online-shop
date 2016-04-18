using System.Security.Principal;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ServiceModel;
using System.Threading;
using Demo.Business.Common;
using Demo.Common;

namespace Demo.Business.Managers.Tests
{
    [TestClass]
    public class InventoryManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            var principal = new GenericPrincipal(new GenericIdentity(Security.DemoUser), new[] { Security.DemoAdminRole });
            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void test_get_products()
        {
            var products = new[]
            {
                new Product
                {
                    ProductId = 1
                },
                new Product
                {
                    ProductId = 2
                },
                new Product
                {
                    ProductId = 3
                }
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<IProductRepository>().Get()).Returns(products);

            var manager = new InventoryManager(mockFactory.Object);
            var result = manager.GetProducts();

            Assert.AreEqual(result.Length, products.Length);
            Assert.AreEqual(result[0].ProductId, 1);
            Assert.AreEqual(result[1].ProductId, 2);
            Assert.AreEqual(result[2].ProductId, 3);
        }

        [TestMethod]
        public void test_get_active_products()
        {
            var products = new[]
            {
                new Product
                {
                    ProductId = 1,
                    IsActive = true
                },
                new Product
                {
                    ProductId = 3,
                    IsActive = true
                }
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<IProductRepository>().GetActiveProducts()).Returns(products);

            var manager = new InventoryManager(mockFactory.Object);
            var result = manager.GetActiveProducts();

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].ProductId, 1);
            Assert.AreEqual(result[1].ProductId, 3);
        }

        [TestMethod]
        public void test_get_product_by_id()
        {
            var product = new Product
            {
                ProductId = 1,
                IsActive = true
            };

            var mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<IProductRepository>().Get(1)).Returns(product);

            var manager = new InventoryManager(mockFactory.Object);
            var result = manager.GetProductById(1);

            Assert.IsTrue(result == product);
        }

        [TestMethod]
        public void test_update_product_add_new()
        {
            var newProduct = new Product();
            var addedProduct = new Product { ProductId = 1 };

            var mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(mock => mock.GetDataRepository<IProductRepository>().Add(newProduct)).Returns(addedProduct);

            var businessFactory = new Mock<IBusinessEngineFactory>();
            businessFactory.Setup(obj => obj.GetBusinessEngine<IProductInventoryEngine>().GenerateArticleNumber())
                .Returns("abcd1234");

            var manager = new InventoryManager(mockDataRepositoryFactory.Object, businessFactory.Object);

            var updateCarResults = manager.UpdateProduct(newProduct);

            Assert.IsTrue(updateCarResults == addedProduct);
        }

        [TestMethod]
        public void test_update_product_existing()
        {
            var existingProduct = new Product { ProductId = 1 };
            var updatedProduct = new Product { ProductId = 1 };

            var mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(mock => mock.GetDataRepository<IProductRepository>().Update(existingProduct)).Returns(updatedProduct);

            var manager = new InventoryManager(mockDataRepositoryFactory.Object);

            var result = manager.UpdateProduct(existingProduct);

            Assert.IsTrue(result == updatedProduct);
        }

        /// <summary>
        /// deleting is ok
        /// but it can not find the product with id 1
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException<NotFoundException>))]
        public void test_delete_product()
        {
            var mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(mock => mock.GetDataRepository<IProductRepository>().Remove(1)).Verifiable();

            var manager = new InventoryManager(mockDataRepositoryFactory.Object);
            manager.DeleteProduct(1);

            mockDataRepositoryFactory.VerifyAll();
        }

        [TestMethod]
        public void test_get_most_wanted()
        {
            // tested in:
            // InventoryManagerEngineTests => test_is_product_most_wanted()
        }
    }
}
