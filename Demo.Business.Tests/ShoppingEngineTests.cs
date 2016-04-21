using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Business.Business_Engines;
using Demo.Data.Contracts;
using Moq;
using Demo.Business.Entities;
using Demo.Data.Contracts.Dto;
using System.Collections.Generic;
using Demo.Business.Contracts;
using System;

namespace Demo.Business.Tests
{
    [TestClass]
    public class ShoppingEngineTests
    {
        [TestMethod]
        public void test_get_shopping_history_info_for_user()
        {
            var customer = new Customer { CustomerId = 1 };
            var product1 = new Product { ProductId = 1, Price = 100 };
            var product2 = new Product { ProductId = 2, Price = 200 };

            var history = new CustomerShoppingHistoryInfoDto
            {
                Customer = customer,
                CartList = new List<CartInfoDto>
                    {
                        new CartInfoDto
                        {
                            CartId = 1,
                            CustomerId = 1,
                            Created = DateTime.Today.AddDays(-1),
                            Total = 500,
                            ShippingCost = 10,
                            CartItemList = new List<CartItemInfoDto>
                            {
                                new CartItemInfoDto
                                {
                                    CartItemId = 1,
                                    CartId = 1,
                                    Product = product1,
                                    Quantity = 1
                                },
                                new CartItemInfoDto
                                {
                                    CartItemId = 2,
                                    CartId = 1,
                                    Product = product2,
                                    Quantity = 2
                                }
                            }
                        },
                        new CartInfoDto
                        {
                            CartId = 2,
                            CustomerId = 1,
                            Created = DateTime.Today,
                            Total = 200,
                            ShippingCost = 20,
                            CartItemList = new List<CartItemInfoDto>
                            {
                                new CartItemInfoDto
                                {
                                    CartItemId = 1,
                                    CartId = 1,
                                    Product = product1,
                                    Quantity = 2
                                }
                            }
                        }
                    }
            };

            var mockCartRepository = new Mock<ICartRepository>();
            mockCartRepository.Setup(Obj => Obj.GetCustomerShoppingHistory(1)).Returns(history);

            var shoppingEngine = new ShoppingEngine();
            var result = shoppingEngine.GetShoppingHistoryInfo(1, mockCartRepository.Object) as CustomerShoppingHistoryInfo;

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.CartList.Length == 2);
            Assert.IsTrue(result.Customer == customer);
            Assert.IsTrue(result.CartList[0].Total == 500);
            Assert.IsTrue(result.CartList[0].CartItemList.Count == 2);
        }

        [TestMethod]
        public void test_check_customer_ownership()
        {
            var customer = new Customer { CustomerId = 1 };

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(obj => obj.Get(1)).Returns(customer);

            var shoppingEngine = new ShoppingEngine();
            var result = shoppingEngine.CheckCustomerOwnership(mockCustomerRepository.Object, 1);

            Assert.AreEqual(customer, result);
        }
        [TestMethod]
        public void test_closing_cart()
        {

        }
    }
}
