using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Contracts;
using Moq;
using Demo.Client.Contracts;
using Demo.Client.Entities;
using Demo.Admin.ViewModels;

namespace Demo.Admin.Tests.ViewModels
{
    [TestClass]
    public class MaintainOrdersViewModelTests
    {
        [TestMethod]
        public void test_approve_order()
        {
            var cart = new Cart
            {
                CartId = 1
            };

            var approvedCart = new Cart
            {
                CartId = 1,
                Approved = DateTime.Today
            };

            var serviceFactory = new Mock<IServiceFactory>();
            serviceFactory.Setup(obj => obj.CreateClient<IShoppingService>().SetCartAsApproved(1)).Verifiable();
            serviceFactory.Setup(obj => obj.CreateClient<IShoppingService>().GetCartByCartId(1)).Returns(approvedCart);

            var vm = new MaintainOrdersViewModel(serviceFactory.Object);
            vm.ApproveOrderCommand.Execute(cart);

            Assert.IsNotNull(cart.Approved);
        }

        [TestMethod]
        public void test_shipp_order()
        {
            var cart = new Cart
            {
                CartId = 1
            };

            var approvedCart = new Cart
            {
                CartId = 1,
                Shipped = DateTime.Today
            };

            var serviceFactory = new Mock<IServiceFactory>();
            serviceFactory.Setup(obj => obj.CreateClient<IShoppingService>().SetCartAsShipped(1)).Verifiable();
            serviceFactory.Setup(obj => obj.CreateClient<IShoppingService>().GetCartByCartId(1)).Returns(approvedCart);

            var vm = new MaintainOrdersViewModel(serviceFactory.Object);
            vm.ShippOrderCommand.Execute(cart);

            Assert.IsNotNull(cart.Shipped);
        }
    }
}
