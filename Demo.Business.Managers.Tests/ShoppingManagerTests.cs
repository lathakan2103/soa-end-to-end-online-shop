using Core.Common.Contracts;
using Demo.Business.Common;
using Demo.Business.Contracts;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;

namespace Demo.Business.Managers.Tests
{
    [TestClass]
    public class ShoppingManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            var principal = new GenericPrincipal(new GenericIdentity("Pingo"), new[] { "Administrators", "DemoAdmin" });
            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void test_get_shopping_history()
        {
            var customer = new Customer { CustomerId = 1, LoginEmail = "test@test.com" };
            var product1 = new Product { ProductId = 1 };
            var product2 = new Product { ProductId = 2 };

            var history = new CustomerShoppingHistoryInfo
            {
                Customer = customer,
                CartList = new[]
                {
                    new CartInfo
                    {
                        CartId = 1,
                        CustomerId = 1,
                        Created = DateTime.Today.AddDays(-1),
                        Total = 2,
                        ShippingCost = 1,
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
                    }
                }
            };

            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(mock => mock.GetByLogin("test@test.com")).Returns(customer);

            var cartRepository = new Mock<ICartRepository>();

            var dataMockRepository = new Mock<IDataRepositoryFactory>();
            dataMockRepository.Setup(mock => mock.GetDataRepository<ICustomerRepository>()).Returns(customerRepository.Object);
            dataMockRepository.Setup(mock => mock.GetDataRepository<ICartRepository>()).Returns(cartRepository.Object);

            var engineFactory = new Mock<IBusinessEngineFactory>();
            engineFactory.Setup(obj => obj.GetBusinessEngine<IShoppingEngine>().GetShoppingHistoryInfo(1, cartRepository.Object)).Returns(history);

            var manager = new ShoppingManager(dataMockRepository.Object, engineFactory.Object);
            var result = manager.GetShoppingHistory("test@test.com");

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Customer == customer);
            Assert.IsTrue(result.CartList.Length == 1);
            Assert.IsTrue(result.CartList[0].CartItemList.Count == 2);
        }

        [TestMethod]
        public void test_get_cart_by_cart_id()
        {
            var cart = new Cart
            {
                CartId = 1
            };

            var customerRepository = new Mock<ICustomerRepository>();

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Get(1)).Returns(cart);
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>()).Returns(customerRepository.Object);

            var shoppingEngine = new Mock<IShoppingEngine>();
            var engineFactory = new Mock<IBusinessEngineFactory>();
            engineFactory.Setup(obj => obj.GetBusinessEngine<IShoppingEngine>()).Returns(shoppingEngine.Object);

            var manager = new ShoppingManager(repositoryFactory.Object, engineFactory.Object);
            var result = manager.GetCartByCartId(1);

            Assert.IsTrue(result == cart);
        }

        [TestMethod]
        public void test_get_carts()
        {
            var carts = new List<Cart>
            {
                new Cart
                {
                    Created = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-3)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-4)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-6)
                }
            };

            var customerRepository = new Mock<ICustomerRepository>();

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>()).Returns(customerRepository.Object);
            repositoryFactory.Setup(Obj => Obj.GetDataRepository<ICartRepository>().Get()).Returns(carts);

            var manager = new ShoppingManager(repositoryFactory.Object);
            var result = manager.GetCartsByDateRange(DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-3), null);

            Assert.IsTrue(result.Length == 2);
        }

        [TestMethod]
        public void test_get_canceled_carts()
        {
            var carts = new List<Cart>
            {
                new Cart
                {
                    Created = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-3),
                    Canceled = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-4)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-6)
                }
            };

            var customerRepository = new Mock<ICustomerRepository>();

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            repositoryFactory.Setup(Obj => Obj.GetDataRepository<ICartRepository>().Get()).Returns(carts);

            var manager = new ShoppingManager(repositoryFactory.Object);
            var result = manager.GetCanceledCarts(DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-2), null);

            Assert.IsTrue(result.Length == 1);
        }

        [TestMethod]
        public void test_get_approved_carts()
        {
            var carts = new List<Cart>
            {
                new Cart
                {
                    Created = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-3),
                    Approved = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-4)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-6)
                }
            };

            var customerRepository = new Mock<ICustomerRepository>();

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            repositoryFactory.Setup(Obj => Obj.GetDataRepository<ICartRepository>().Get()).Returns(carts);

            var manager = new ShoppingManager(repositoryFactory.Object);
            var result = manager.GetApprovedCarts(DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-2), null);

            Assert.IsTrue(result.Length == 1);
        }

        [TestMethod]
        public void test_get_shipped_carts()
        {
            var carts = new List<Cart>
            {
                new Cart
                {
                    Created = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-3),
                    Shipped = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-4)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-6)
                }
            };

            var customerRepository = new Mock<ICustomerRepository>();

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            repositoryFactory.Setup(Obj => Obj.GetDataRepository<ICartRepository>().Get()).Returns(carts);

            var manager = new ShoppingManager(repositoryFactory.Object);
            var result = manager.GetShippedCarts(DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-2), null);

            Assert.IsTrue(result.Length == 1);
        }

        [TestMethod]
        public void test_get_newly_ceated_cars()
        {
            var carts = new List<Cart>
            {
                new Cart
                {
                    Created = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-3),
                    Canceled = DateTime.Today.AddDays(-2)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-4)
                },
                new Cart
                {
                    Created = DateTime.Today.AddDays(-6)
                }
            };

            var customerRepository = new Mock<ICustomerRepository>();

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            repositoryFactory.Setup(Obj => Obj.GetDataRepository<ICartRepository>().Get()).Returns(carts);

            var manager = new ShoppingManager(repositoryFactory.Object);
            var result = manager.GetNewCarts();

            Assert.IsTrue(result.Length == 3);
        }

        [TestMethod]
        public void test_get_carts_with_total_greater_then_amount()
        {
            var carts = new List<Cart>
            {
                new Cart
                {
                    Total = 100
                },
                new Cart
                {
                    Total = 200
                },
                new Cart
                {
                    Total = 300 
                },
                new Cart
                {
                    Total = 400
                }
            };

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Get()).Returns(carts);

            var manager = new ShoppingManager(repositoryFactory.Object);
            var result = manager.GetCartsWithTotalAmountGreaterThen(250);

            Assert.IsTrue(result.Length == 2);
            Assert.IsTrue(result[0].Total + result[1].Total == 700); 
        }

        [TestMethod]
        public void test_set_cart_as_canceled()
        {
            var cart = new Cart { CartId = 1 };
            var cartUpdated = new Cart { CartId = 1, Canceled = DateTime.Today };

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            var customerRepository = new Mock<ICustomerRepository>();

            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Get(1)).Returns(cart);
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>()).Returns(customerRepository.Object);

            var engineFactory = new Mock<IBusinessEngineFactory>();
            var shoppingEngine = new Mock<IShoppingEngine>();

            engineFactory.Setup(obj => obj.GetBusinessEngine<IShoppingEngine>()).Returns(shoppingEngine.Object);

            var manager = new ShoppingManager(repositoryFactory.Object, engineFactory.Object);
            manager.SetCartAsCanceled(1);

            repositoryFactory.VerifyAll();
        }

        [TestMethod]
        public void test_set_cart_as_approved()
        {
            var cart = new Cart { CartId = 1 };
            var cartUpdated = new Cart { CartId = 1, Approved = DateTime.Today };

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            var customerRepository = new Mock<ICustomerRepository>();

            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Get(1)).Returns(cart);

            var engineFactory = new Mock<IBusinessEngineFactory>();
            var shoppingEngine = new Mock<IShoppingEngine>();

            engineFactory.Setup(obj => obj.GetBusinessEngine<IShoppingEngine>()).Returns(shoppingEngine.Object);

            var manager = new ShoppingManager(repositoryFactory.Object, engineFactory.Object);
            manager.SetCartAsApproved(1);

            repositoryFactory.VerifyAll();
        }

        [TestMethod]
        public void test_set_cart_as_shipped()
        {
            var cart = new Cart { CartId = 1 };
            var cartUpdated = new Cart { CartId = 1, Shipped = DateTime.Today };

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            var customerRepository = new Mock<ICustomerRepository>();

            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Get(1)).Returns(cart);

            var engineFactory = new Mock<IBusinessEngineFactory>();
            var shoppingEngine = new Mock<IShoppingEngine>();

            engineFactory.Setup(obj => obj.GetBusinessEngine<IShoppingEngine>()).Returns(shoppingEngine.Object);

            var manager = new ShoppingManager(repositoryFactory.Object, engineFactory.Object);
            manager.SetCartAsShipped(1);

            repositoryFactory.VerifyAll();
        }

        [TestMethod]
        public void test_add_new_cart()
        {
            var cart = new Cart();
            var addedCart = new Cart { CartId = 1, Created = DateTime.Today };

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            var customerRepository = new Mock<ICustomerRepository>();

            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Add(cart)).Returns(addedCart);
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>()).Returns(customerRepository.Object);

            var engineFactory = new Mock<IBusinessEngineFactory>();
            var shoppingEngine = new Mock<IShoppingEngine>();

            engineFactory.Setup(obj => obj.GetBusinessEngine<IShoppingEngine>()).Returns(shoppingEngine.Object);

            var manager = new ShoppingManager(repositoryFactory.Object, engineFactory.Object);
            var result = manager.AddCart(cart);

            Assert.IsTrue(result == addedCart);
        }

        [TestMethod]
        public void test_add_cart_item_to_cart()
        {
            var cartItem = new CartItem { CartItemId = 1, CartId = 5 };
            var cart = new Cart { CartId = 3 };
            var cartItem2 = new CartItem { CartItemId = 1, CartId = 3 };

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            var customerRepository = new Mock<ICustomerRepository>();

            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Get(3)).Returns(cart);
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>()).Returns(customerRepository.Object);
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartItemRepository>().Add(cartItem)).Returns(cartItem2);

            var engineFactory = new Mock<IBusinessEngineFactory>();
            var shoppingEngine = new Mock<IShoppingEngine>();

            engineFactory.Setup(obj => obj.GetBusinessEngine<IShoppingEngine>()).Returns(shoppingEngine.Object);

            var manager = new ShoppingManager(repositoryFactory.Object, engineFactory.Object);
            manager.AddCartItemToCart(3, cartItem);

            repositoryFactory.VerifyAll();
        }

        [TestMethod]
        public void test_add_cart_items_to_cart()
        {
            var cartItem1 = new CartItem { CartItemId = 1, CartId = 5 };
            var cartItem2 = new CartItem { CartItemId = 2, CartId = 5 };
            var cart = new Cart { CartId = 3 };

            var cartItem1_2 = new CartItem { CartItemId = 1, CartId = 3 };
            var cartItem2_2 = new CartItem { CartItemId = 2, CartId = 3 };

            var repositoryFactory = new Mock<IDataRepositoryFactory>();
            var customerRepository = new Mock<ICustomerRepository>();

            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartRepository>().Get(3)).Returns(cart);
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICustomerRepository>()).Returns(customerRepository.Object);
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartItemRepository>().Add(cartItem1)).Returns(cartItem1_2);
            repositoryFactory.Setup(obj => obj.GetDataRepository<ICartItemRepository>().Add(cartItem2)).Returns(cartItem2_2);

            var engineFactory = new Mock<IBusinessEngineFactory>();
            var shoppingEngine = new Mock<IShoppingEngine>();

            engineFactory.Setup(obj => obj.GetBusinessEngine<IShoppingEngine>()).Returns(shoppingEngine.Object);

            var manager = new ShoppingManager(repositoryFactory.Object, engineFactory.Object);
            manager.AddCartItemsToCart(3, new CartItem[] { cartItem1, cartItem2 });

            repositoryFactory.VerifyAll();
        }
    }
}
