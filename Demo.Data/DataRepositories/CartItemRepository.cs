using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using Demo.Data.Contracts.Dto;

namespace Demo.Data.DataRepositories
{
    [Export(typeof(ICartItemRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CartItemRepository : DataRepositoryBase<CartItem>, ICartItemRepository
    {
        #region IDataRepository<CartItem> implementation

        protected override CartItem AddEntity(DemoContext ctx, CartItem entity)
        {
            return ctx.CartItemSet.Add(entity);
        }

        protected override CartItem UpdateEntity(DemoContext ctx, CartItem entity)
        {
            return ctx.CartItemSet.FirstOrDefault(c => c.CartItemId == entity.CartItemId);
        }

        protected override IEnumerable<CartItem> GetEntities(DemoContext ctx)
        {
            return ctx.CartItemSet;
        }

        protected override CartItem GetEntity(DemoContext ctx, int id)
        {
            return ctx.CartItemSet.FirstOrDefault(c => c.CartItemId == id);
        }

        #endregion

        #region ICartItemRepository implementation

        public CartItemInfoDto[] GetCartItemsByCartId(int cartId)
        {
            using (var ctx = new DemoContext())
            {
                return (from i in ctx.CartItemSet
                        where i.CartId == cartId
                        join p in ctx.ProductSet on i.ProductId equals p.ProductId 
                        select new CartItemInfoDto
                        {
                            Product = p,
                            CartId = cartId,
                            Quantity = i.Quantity,
                            CartItemId = i.CartItemId
                        }
                ).ToArray();
            }
        }

        #endregion
    }
}
