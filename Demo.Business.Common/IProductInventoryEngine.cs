using System.Collections.Generic;
using Core.Common.Contracts;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.Business.Common
{
    public interface IProductInventoryEngine : IBusinessEngine
    {
        bool IsMostWanted(int productId, IEnumerable<Cart> carts, ICartItemRepository repo);
        string GenerateArticleNumber();
    }
}