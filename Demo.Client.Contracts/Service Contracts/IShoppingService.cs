using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Demo.Client.Entities;
using Demo.Common;

namespace Demo.Client.Contracts
{
    [ServiceContract]
    public interface IShoppingService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        CustomerShoppingHistoryInfo GetShoppingHistory(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        IEnumerable<Cart> GetCarts();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Cart GetCartByCartId(int cartId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Cart[] GetCartsByDateRange(DateTime start, DateTime end, int? customerId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Cart[] GetCartsByCustomer(int customerId);

        [OperationContract]
        Cart[] GetCanceledCarts(DateTime start, DateTime end, int? customerId);

        [OperationContract]
        Cart[] GetApprovedCarts(DateTime start, DateTime end, int? customerId);

        [OperationContract]
        Cart[] GetShippedCarts(DateTime start, DateTime end, int? customerId);

        [OperationContract]
        Cart[] GetNewCarts();

        [OperationContract]
        Cart[] GetCartsWithTotalAmountGreaterThen(decimal totalAmount);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void SetCartAsCanceled(int cartId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void SetCartAsApproved(int cartId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void SetCartAsShipped(int cartId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Cart AddCart(Cart cart);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void AddCartItemToCart(int cartId, CartItem item);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void AddCartItemsToCart(int cartId, CartItem[] items);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        IEnumerable<CartItemInfo> GetCartItemsByCartId(int cartId);

        #region Async

        [OperationContract]
        Task<CustomerShoppingHistoryInfo> GetShoppingHistoryAsync(string loginEmail);

        [OperationContract]
        Task<IEnumerable<Cart>> GetCartsAsync();

        [OperationContract]
        Task<Cart> GetCartByCartIdAsync(int cartId);

        [OperationContract]
        Task<Cart[]> GetCartsByDateRangeAsync(DateTime start, DateTime end, int? customerId);

        [OperationContract]
        Task<Cart[]> GetCartsByCustomerAsync(int customerId);

        [OperationContract]
        Task<Cart[]> GetCanceledCartsAsync(DateTime start, DateTime end, int? customerId);

        [OperationContract]
        Task<Cart[]> GetApprovedCartsAsync(DateTime start, DateTime end, int? customerId);

        [OperationContract]
        Task<Cart[]> GetShippedCartsAsync(DateTime start, DateTime end, int? customerId);

        [OperationContract]
        Task<Cart[]> GetNewCartsAsync();

        [OperationContract]
        Task<Cart[]> GetCartsWithTotalAmountGreaterThenAsync(decimal totalAmount);

        [OperationContract]
        Task SetCartAsCanceledAsync(int cartId);

        [OperationContract]
        Task SetCartAsApprovedAsync(int cartId);

        [OperationContract]
        Task SetCartAsShippedAsync(int cartId);

        [OperationContract]
        Task<Cart> AddCartAsync(Cart cart);

        [OperationContract]
        Task AddCartItemToCartAsync(int cartId, CartItem item);

        [OperationContract]
        Task AddCartItemsToCartAsync(int cartId, CartItem[] items);

        [OperationContract]
        Task<IEnumerable<CartItemInfo>> GetCartItemsByCartIdAsync(int cartId);

        #endregion
    }
}
