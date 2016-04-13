using Demo.Business.Business_Engines;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Demo.Data.Contracts.Dto;

namespace Demo.Business.Tests
{
    [TestClass]
    public class ProductInventoryEngineTests
    {
        [TestMethod]
        public void test_is_product_most_wanted()
        {
            var cartList = new List<Cart>
            {
                new Cart
                {
                    CartId = 1,
                    CartItemId = new[] { 1, 2, 3 }
                },
                new Cart
                {
                    CartId = 2,
                    CartItemId = new[] { 4, 5, 6 }
                }
            };

            var product1 = new Product { ProductId = 1 };
            var product2 = new Product { ProductId = 2 };
            var product3 = new Product { ProductId = 3 };

            var cartItems1 = new List<CartItemInfoDto>
            {
                new CartItemInfoDto
                {
                    CartId = 1,
                    CartItemId = 1,
                    Quantity = 1,
                    Product = product1
                },
                new CartItemInfoDto
                {
                    CartId = 1,
                    CartItemId = 2,
                    Quantity = 1,
                    Product = product2
                }               
            };
            var cartItems2 = new List<CartItemInfoDto>
            {
                new CartItemInfoDto
                {
                    CartId = 2,
                    CartItemId = 4,
                    Quantity = 1,
                    Product = product1
                },
                new CartItemInfoDto
                {
                    CartId = 2,
                    CartItemId = 5,
                    Quantity = 3,
                    Product = product2
                },
                new CartItemInfoDto
                {
                    CartId = 2,
                    CartItemId = 6,
                    Quantity = 3,
                    Product = product3
                }
            };

            var mockCartItemRepository = new Mock<ICartItemRepository>();
            mockCartItemRepository.Setup(obj => obj.GetCartItemsByCartId(1)).Returns(cartItems1.ToArray);
            mockCartItemRepository.Setup(obj => obj.GetCartItemsByCartId(2)).Returns(cartItems2.ToArray);

            var productInventoryEngine = new ProductInventoryEngine();
            var isMostWanted1 = productInventoryEngine.IsMostWanted(1, cartList, mockCartItemRepository.Object);
            var isMostWanted2 = productInventoryEngine.IsMostWanted(2, cartList, mockCartItemRepository.Object);
            var isMostWanted3 = productInventoryEngine.IsMostWanted(3, cartList, mockCartItemRepository.Object);

            Assert.IsFalse(isMostWanted1);  // ProductId 1 - quantity = 2 => FALSE
            Assert.IsTrue(isMostWanted2);   // ProductId 2 - quantity = 4 => TRUE
            Assert.IsFalse(isMostWanted3);  // ProductId 3 - quantity = 3 => FALSE
        }
    }
}
