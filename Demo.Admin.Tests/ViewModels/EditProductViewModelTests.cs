using Demo.Admin.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Demo.Admin.ViewModels;
using Demo.Client.Entities;
using Demo.Client.Contracts;
using GalaSoft.MvvmLight.Messaging;

namespace Demo.Admin.Tests.ViewModels
{
    [TestClass]
    public class EditProductViewModelTests
    {
        [TestMethod]
        public void test_cancel_command()
        {
            var inventoryService = new Mock<IInventoryService>();

            var vm = new EditProductDialogViewModel(inventoryService.Object, null);
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

            var inventoryService = new Mock<IInventoryService>();
            var messenger = new Mock<IMessenger>();
            messenger.Setup(obj => obj.Send(new ProductChangedMessage())).Verifiable();

            var vm = new EditProductDialogViewModel(inventoryService.Object, messenger.Object)
            {
                CurrentProduct = productToUpdate
            };

            inventoryService.Setup(obj => obj.GetProductById(0, true)).Returns(new Product());
            inventoryService.Setup(obj => obj.UpdateProduct(productToUpdate)).Returns(addedProduct);
            
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

            var inventoryService = new Mock<IInventoryService>();
            var messenger = new Mock<IMessenger>();
            messenger.Setup(obj => obj.Send(new ProductChangedMessage())).Verifiable();

            var vm = new EditProductDialogViewModel(inventoryService.Object, messenger.Object)
            {
                CurrentProduct = productToUpdate
            };
            vm.CurrentProduct.Name = "UPDATED";

            inventoryService.Setup(obj => obj.GetProductById(1, true)).Returns(productToUpdate);
            inventoryService.Setup(obj => obj.UpdateProduct(productToUpdate)).Returns(updatedProduct);

            vm.SaveCommand.Execute(null);
        }
    }
}
