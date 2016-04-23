using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Exceptions;
using Demo.Business.Common;
using Demo.Business.Contracts;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.Business.Business_Engines
{
    /// <summary>
    /// it groups all cart items by cart id
    /// because the repository returns an objects with customer details and all carts
    /// with all cart items but not grouped together
    /// </summary>
    [Export(typeof (IShoppingEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ShoppingEngine : IShoppingEngine
    {
        public CustomerShoppingHistoryInfo GetShoppingHistoryInfo(
            int customerId, ICartRepository cartRepository)
        {
            var carts = cartRepository.GetCustomerShoppingHistory(customerId);
            var cartList = new List<CartInfo>();

            foreach (var cart in carts.CartList)
            {
                var ci = new CartInfo
                {
                    CustomerId = cart.CustomerId,
                    CartId = cart.CartId,
                    Approved = cart.Approved,
                    Canceled = cart.Canceled,
                    Created = cart.Created,
                    Shipped = cart.Shipped,
                    ShippingCost = cart.ShippingCost,
                    CartItemList = new List<CartItemInfo>()
                };
                cartList.Add(ci);
                decimal total = 0;

                var cartItems = cartRepository.GetCartItemsByCartId(cart.CartId);
                foreach (var item in cartItems)
                {
                    ci.CartItemList.Add(
                        new CartItemInfo
                        {
                            CartItemId = item.CartItemId,
                            CartId = item.CartId,
                            Product = item.Product,
                            Quantity = item.Quantity
                        });
                    total += item.Product.Price * item.Quantity;
                }

                ci.Total = total;
            }

            var result = new CustomerShoppingHistoryInfo
            {
                Customer = carts.Customer,
                CartList = new CartInfo[cartList.Count]
            };
            for (var i = 0; i < cartList.Count; i++)
            {
                result.CartList[i] = cartList[i];
            }

            return result;
        }

        public Customer CheckCustomerOwnership(ICustomerRepository customerRepository, int customerId)
        {
            var customer = customerRepository.Get(customerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with id: {customerId} was not found");
            }
            return customer;
        }

        /// <summary>
        /// 1) check availability
        /// 2) check promotions
        /// 3) shipping cost
        /// 4) change stock
        /// 5) send notification
        /// </summary>
        /// <param name="cart"></param>
        public void ProcessOrder(Cart cart)
        {
            
        }
    }
}
