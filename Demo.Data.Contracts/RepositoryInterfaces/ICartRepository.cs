using System.Collections.Generic;
using Core.Common.Contracts;
using Demo.Business.Entities;
using Demo.Data.Contracts.Dto;

namespace Demo.Data.Contracts
{
    public interface ICartRepository : IDataRepository<Cart>
    {
        CustomerShoppingHistoryInfoDto GetCustomerShoppingHistory(int id);
        void CloseCart(int cartId);
        List<CartItemInfoDto> GetCartItemsByCartId(int cartId);
    }
}