using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Core.Common.ServiceModel;
using Demo.Client.Contracts;
using Demo.Client.Entities;

namespace Demo.Client.Proxies.Service_Procies
{
    [Export(typeof(IShoppingService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ShoppingClient : UserClientBase<IShoppingService>, IShoppingService
    {
        public CustomerShoppingHistoryInfo GetShoppingHistory(string loginEmail)
        {
            try
            {
                var result = Channel.GetShoppingHistory(loginEmail);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Cart GetCartByCartId(int cartId)
        {
            return Channel.GetCartByCartId(cartId);
        }

        public Cart[] GetCartsByDateRange(DateTime start, DateTime end, int? customerId)
        {
            return Channel.GetCartsByDateRange(start, end, customerId);
        }

        public Cart[] GetCartsByCustomer(int customerId)
        {
            return Channel.GetCartsByCustomer(customerId);
        }

        public Cart[] GetCanceledCarts(DateTime start, DateTime end, int? customerId)
        {
            return Channel.GetCanceledCarts(start, end, customerId);
        }

        public Cart[] GetApprovedCarts(DateTime start, DateTime end, int? customerId)
        {
            return Channel.GetApprovedCarts(start, end, customerId);
        }

        public Cart[] GetShippedCarts(DateTime start, DateTime end, int? customerId)
        {
            return Channel.GetShippedCarts(start, end, customerId);
        }

        public Cart[] GetNewCarts()
        {
            return Channel.GetNewCarts();
        }

        public Cart[] GetCartsWithTotalAmountGreaterThen(decimal totalAmount)
        {
            return Channel.GetCartsWithTotalAmountGreaterThen(totalAmount);
        }

        public void SetCartAsCanceled(int cartId)
        {
            Channel.SetCartAsCanceled(cartId);
        }

        public void SetCartAsApproved(int cartId)
        {
            Channel.SetCartAsApproved(cartId);
        }

        public void SetCartAsShipped(int cartId)
        {
            Channel.SetCartAsShipped(cartId);
        }

        public Cart AddCart(Cart cart)
        {
            return Channel.AddCart(cart);
        }

        public void AddCartItemToCart(int cartId, CartItem item)
        {
            Channel.AddCartItemToCart(cartId, item);
        }

        public void AddCartItemsToCart(int cartId, CartItem[] items)
        {
            Channel.AddCartItemsToCart(cartId, items);
        }

        public IEnumerable<CartItemInfo> GetCartItemsByCartId(int cartId)
        {
            return Channel.GetCartItemsByCartId(cartId);
        }

        public void CloseCart(int cartId)
        {
            Channel.CloseCart(cartId);
        }

        public Cart GetActiveCart(int customerId)
        {
            return Channel.GetActiveCart(customerId);
        }

        public Task<CustomerShoppingHistoryInfo> GetShoppingHistoryAsync(string loginEmail)
        {
            return Channel.GetShoppingHistoryAsync(loginEmail);
        }

        public Task<Cart> GetCartByCartIdAsync(int cartId)
        {
            return Channel.GetCartByCartIdAsync(cartId);
        }

        public Task<Cart[]> GetCartsByDateRangeAsync(DateTime start, DateTime end, int? customerId)
        {
            return Channel.GetCartsByDateRangeAsync(start, end, customerId);
        }

        public Task<Cart[]> GetCartsByCustomerAsync(int customerId)
        {
            return Channel.GetCartsByCustomerAsync(customerId);
        }

        public Task<Cart[]> GetCanceledCartsAsync(DateTime start, DateTime end, int? customerId)
        {
            return Channel.GetCanceledCartsAsync(start, end, customerId);
        }

        public Task<Cart[]> GetApprovedCartsAsync(DateTime start, DateTime end, int? customerId)
        {
            return Channel.GetApprovedCartsAsync(start, end, customerId);
        }

        public Task<Cart[]> GetShippedCartsAsync(DateTime start, DateTime end, int? customerId)
        {
            return Channel.GetShippedCartsAsync(start, end, customerId);
        }

        public Task<Cart[]> GetNewCartsAsync()
        {
            return Channel.GetNewCartsAsync();
        }

        public Task<Cart[]> GetCartsWithTotalAmountGreaterThenAsync(decimal totalAmount)
        {
            return Channel.GetCartsWithTotalAmountGreaterThenAsync(totalAmount);
        }

        public Task SetCartAsCanceledAsync(int cartId)
        {
            return Channel.SetCartAsCanceledAsync(cartId);
        }

        public Task SetCartAsApprovedAsync(int cartId)
        {
            return Channel.SetCartAsApprovedAsync(cartId);
        }

        public Task SetCartAsShippedAsync(int cartId)
        {
            return Channel.SetCartAsShippedAsync(cartId);
        }

        public Task<Cart> AddCartAsync(Cart cart)
        {
            return Channel.AddCartAsync(cart);
        }

        public Task AddCartItemToCartAsync(int cartId, CartItem item)
        {
            return Channel.AddCartItemToCartAsync(cartId, item);
        }

        public Task AddCartItemsToCartAsync(int cartId, CartItem[] items)
        {
            return Channel.AddCartItemsToCartAsync(cartId, items);
        }

        public Task<IEnumerable<CartItemInfo>> GetCartItemsByCartIdAsync(int cartId)
        {
            return Channel.GetCartItemsByCartIdAsync(cartId);
        }

        public IEnumerable<Cart> GetCarts()
        {
            return Channel.GetCarts();
        }

        public Task<IEnumerable<Cart>> GetCartsAsync()
        {
            return Channel.GetCartsAsync();
        }

        public Task CloseCartAsync(int cartId)
        {
            return Channel.CloseCartAsync(cartId);
        }

        public Task<Cart> GetActiveCartAsync(int customerId)
        {
            return Channel.GetActiveCartAsync(customerId);
        }
    }
}
