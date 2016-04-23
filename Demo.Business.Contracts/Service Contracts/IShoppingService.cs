using System;
using System.Collections.Generic;
using System.ServiceModel;
using Core.Common.Exceptions;
using Demo.Business.Entities;
using Demo.Common;

namespace Demo.Business.Contracts
{
    [ServiceContract]
    public interface IShoppingService
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

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        void CloseCart(int cartId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Cart GetActiveCart(int customerId);
    }
}
