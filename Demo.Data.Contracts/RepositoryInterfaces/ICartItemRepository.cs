using Core.Common.Contracts;
using Demo.Business.Entities;
using Demo.Data.Contracts.Dto;

namespace Demo.Data.Contracts
{
    public interface ICartItemRepository : IDataRepository<CartItem>
    {
        CartItemInfoDto[] GetCartItemsByCartId(int cartId);
    }
}