using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.Security.Principal;
using Demo.Client.Contracts;
using Moq;
using System.Collections.Generic;
using Demo.Client.Entities;
using Demo.Web.Controllers.Api;

namespace Demo.Web.Tests
{
    [TestClass]
    public class ShoppingApiControllerTests
    {
        #region Fields

        private HttpRequestMessage _Request = null;
        private Mock<IShoppingService> _shoppingService;
        private Mock<IInventoryService> _inventoryService;
        private Mock<ICustomerService> _customerService;
        private ShoppingApiController _controller;

        #endregion

        [TestInitialize]
        public void Initializer()
        {
            this._Request = GetRequest();

            // security because of ValidateAuthorizedUser!!!
            var principal = new GenericPrincipal(new GenericIdentity("test@test.com"), new[] { "Administrators" });
            Thread.CurrentPrincipal = principal;

            this._shoppingService = new Mock<IShoppingService>();
            this._inventoryService = new Mock<IInventoryService>();
            this._customerService = new Mock<ICustomerService>();

            this._controller =
                new ShoppingApiController(
                    this._shoppingService.Object,
                    this._inventoryService.Object,
                    this._customerService.Object);
        }

        [TestMethod]
        public void test_get_active_products()
        {
            var products = new Product[]
            {
                new Product { ProductId = 1, Name = "Test product 1", Description = "---", Price = 100, IsActive = true },
                new Product { ProductId = 2, Name = "Test product 2", Description = "---", Price = 200, IsActive = true }
            };

            this._inventoryService.Setup(obj => obj.GetActiveProducts()).Returns(products);

            var response = this._controller.GetActiveProducts(this._Request);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var data = this.GetResponseData<Product[]>(response);

            Assert.IsTrue(data[0] == products[0]);
            Assert.IsTrue(data[1] == products[1]);
        }

        [TestMethod]
        public void test_get_product_by_id()
        {
            var product = new Product
            {
                ProductId = 1,
                Name = "Test product 1",
                Description = "---",
                Price = 100,
                IsActive = true
            };

            this._inventoryService.Setup(obj => obj.GetProductById(1, false)).Returns(product);

            var response = this._controller.GetProduct(this._Request, 1);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var data = this.GetResponseData<Product>(response);

            Assert.IsTrue(data == product);
        }

        [TestMethod]
        public void test_get_shopping_history()
        {
            var customer = new Customer { CustomerId = 1 };
            var product1 = new Product { ProductId = 1, Price = 100 };
            var product2 = new Product { ProductId = 2, Price = 200 };

            var history = new CustomerShoppingHistoryInfo
            {
                Customer = customer,
                CartList = new CartInfo[]
                    {
                        new CartInfo
                        {
                            CartId = 1,
                            CustomerId = 1,
                            Created = DateTime.Today.AddDays(-1),
                            Total = 500,
                            ShippingCost = 10,
                            CartItemList = new List<CartItemInfo>
                            {
                                new CartItemInfo
                                {
                                    CartItemId = 1,
                                    CartId = 1,
                                    Product = product1,
                                    Quantity = 1
                                },
                                new CartItemInfo
                                {
                                    CartItemId = 2,
                                    CartId = 1,
                                    Product = product2,
                                    Quantity = 2
                                }
                            }
                        },
                        new CartInfo
                        {
                            CartId = 2,
                            CustomerId = 1,
                            Created = DateTime.Today,
                            Total = 200,
                            ShippingCost = 20,
                            CartItemList = new List<CartItemInfo>
                            {
                                new CartItemInfo
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

            this._shoppingService.Setup(obj => obj.GetShoppingHistory("test@test.com")).Returns(history);
            var response = this._controller.GetShoppingHistory(this._Request);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var data = this.GetResponseData<CustomerShoppingHistoryInfo>(response);

            Assert.IsTrue(data == history);
        }

        [TestMethod]
        public void test_get_cart_items_by_cart_id()
        {
            var items = new List<CartItemInfo>
            {
                new CartItemInfo { CartId = 1, CartItemId = 1 },
                new CartItemInfo { CartId = 1, CartItemId = 2 },
                new CartItemInfo { CartId = 1, CartItemId = 3 },
                new CartItemInfo { CartId = 1, CartItemId = 4 }
            };

            this._shoppingService.Setup(obj => obj.GetCartItemsByCartId(1)).Returns(items);
            var response = this._controller.GetCartItemsByCartId(this._Request, 1);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var data = this.GetResponseData<IEnumerable<CartItemInfo>>(response);

            Assert.IsTrue(data == items);
        }

        [TestMethod]
        public void test_buy_product()
        {
            //var product = new Product { ProductId = 1, Name = "Test product 1", Description = "---", Price = 100, IsActive = true };
            //var customer = new Customer { CustomerId = 1, IsActive = true, LoginEmail = "test@test.com" };
            //var cart = new Cart ();
            //var addedCart = new Cart { CartId = 1 };
            //var cartItem = new CartItem { CartId = 1, CartItemId = 1, ProductId = 1, Quantity = 1 };

            //this._inventoryService.Setup(obj => obj.GetProductById(1, false)).Returns(product);
            //this._customerService.Setup(obj => obj.GetCustomerByLogin("test@test.com")).Returns(customer);
            ////this._shoppingService.Setup(obj => obj.AddCart(cart)).Returns(addedCart);
            ////this._shoppingService.Setup(obj => obj.AddCartItemToCart(1, cartItem));

            //var response = this._controller.BuyProduct(this._Request, 1);

            //Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public void test_cancel_cart()
        {
            var cart = new Cart { CartId = 1, CustomerId = 1, Total = 100 };

            this._shoppingService.Setup(obj => obj.GetCartByCartId(1)).Returns(cart);
            this._shoppingService.Setup(obj => obj.SetCartAsCanceled(1)).Verifiable();

            var response = this._controller.CancelCart(this._Request, 1);

            this._shoppingService.VerifyAll();

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        #region Helpers

        HttpRequestMessage GetRequest()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.Properties["MS_HttpConfiguration"] = config;
            return request;
        }

        T GetResponseData<T>(HttpResponseMessage result)
        {
            var content = result.Content as ObjectContent<T>;
            if (content != null)
            {
                T data = (T)(content.Value);
                return data;
            }
            else
            {
                return default(T);
            }
        }

        #endregion
    }
}
