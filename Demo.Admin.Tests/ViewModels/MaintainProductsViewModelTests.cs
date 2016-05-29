using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Core.Common.Contracts;
using Demo.Client.Contracts;
using Demo.Client.Entities;
using Demo.Admin.ViewModels;

namespace Demo.Admin.Tests.ViewModels
{
    [TestClass]
    public class MaintainProductsViewModelTests
    {
        [TestMethod]
        public void test_deactivate_product()
        {
            var product = new Product
            {
                ProductId = 1,
                IsActive = true
            };

            var serviceFactory = new Mock<IServiceFactory>();
            serviceFactory.Setup(obj => obj.CreateClient<IInventoryService>().DeleteProduct(1)).Verifiable();

            var vm = new MaintainProductsViewModel(serviceFactory.Object);
            vm.DeactivateProductCommand.Execute(product);

            Assert.IsTrue(product.IsActive == false);
        }

        [TestMethod]
        public void test_activate_product()
        {
            var product = new Product
            {
                ProductId = 1,
                IsActive = false
            };

            var serviceFactory = new Mock<IServiceFactory>();
            serviceFactory.Setup(obj => obj.CreateClient<IInventoryService>().ActivateProduct(1)).Verifiable();

            var vm = new MaintainProductsViewModel(serviceFactory.Object);
            vm.ActivateProductCommand.Execute(product);

            Assert.IsTrue(product.IsActive);
        }
    }
}
