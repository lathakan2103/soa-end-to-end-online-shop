using Demo.Admin.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Demo.Admin.ViewModels;
using Demo.Client.Entities;
using Demo.Client.Contracts;
using GalaSoft.MvvmLight.Messaging;
using Core.Common.Contracts;

namespace Demo.Admin.Tests.ViewModels
{
    [TestClass]
    public class EditProductViewModelTests
    {
        [TestMethod]
        public void test_cancel_command()
        {
            var serviceFactory = new Mock<IServiceFactory>();

            var vm = new EditProductDialogViewModel(serviceFactory.Object);
            vm.CurrentProduct = new Product();

            Assert.IsNotNull(vm.CurrentProduct);

            vm.CancelCommand.Execute(null);

            Assert.IsNull(vm.CurrentProduct);
        }

        [TestMethod]
        public void test_add_new_product()
        {
            var productToUpdate = new Product
            {
                Name = "test",
                Description = "description",
                Price = 1,
                IsActive = true
            };
            var addedProduct = new Product
            {
                ProductId = 1,
                Name = "test",
                Description = "description",
                Price = 1,
                IsActive = true
            };

            var serviceFactory = new Mock<IServiceFactory>();
            var messenger = new Mock<IMessenger>();
            messenger.Setup(obj => obj.Send(new ProductChangedMessage())).Verifiable();

            var vm = new EditProductDialogViewModel(serviceFactory.Object, messenger.Object)
            {
                CurrentProduct = productToUpdate
            };

            serviceFactory.Setup(obj => obj.CreateClient<IInventoryService>().GetProductById(0, true)).Returns(new Product());
            serviceFactory.Setup(obj => obj.CreateClient<IInventoryService>().UpdateProduct(productToUpdate)).Returns(addedProduct);
            
            vm.SaveCommand.Execute(null);
        }

        [TestMethod]
        public void test_update_product()
        {
            var productToUpdate = new Product
            {
                ProductId = 1,
                Name = "test",
                Description = "description",
                Price = 1,
                IsActive = true
            };
            var updatedProduct = new Product
            {
                ProductId = 1,
                Name = "test",
                Description = "UPDATED",
                Price = 1,
                IsActive = true
            };

            var serviceFactory = new Mock<IServiceFactory>();
            var messenger = new Mock<IMessenger>();
            messenger.Setup(obj => obj.Send(new ProductChangedMessage())).Verifiable();

            var vm = new EditProductDialogViewModel(serviceFactory.Object, messenger.Object)
            {
                CurrentProduct = productToUpdate
            };
            vm.CurrentProduct.Name = "UPDATED";

            serviceFactory.Setup(obj => obj.CreateClient<IInventoryService>().GetProductById(1, true)).Returns(productToUpdate);
            serviceFactory.Setup(obj => obj.CreateClient<IInventoryService>().UpdateProduct(productToUpdate)).Returns(updatedProduct);

            vm.SaveCommand.Execute(null);
        }
    }
}
