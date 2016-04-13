using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using Demo.Data.Contracts.Dto;

namespace Demo.Data.DataRepositories
{
    [Export(typeof(ICartRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CartRepository : DataRepositoryBase<Cart>, ICartRepository
    {
        #region IDataRepository<Cart> implementation

        protected override Cart AddEntity(DemoContext ctx, Cart entity)
        {
            return ctx.CartSet.Add(entity);
        }

        protected override Cart UpdateEntity(DemoContext ctx, Cart entity)
        {
            return ctx.CartSet.FirstOrDefault(c => c.CartId == entity.CartId);
        }

        protected override IEnumerable<Cart> GetEntities(DemoContext ctx)
        {
            return ctx.CartSet;
        }

        protected override Cart GetEntity(DemoContext ctx, int id)
        {
            return ctx.CartSet.FirstOrDefault(c => c.CartId == id);
        }

        #endregion

        #region ICartRepository implementation

        public CustomerShoppingHistoryInfoDto GetCustomerShoppingHistory(int id)
        {
            using (var ctx = new DemoContext())
            {
                var query = from cart in ctx.CartSet
                    where cart.CustomerId == id
                    join item in ctx.CartItemSet on cart.CartId equals item.CartId
                    join prod in ctx.ProductSet on item.ProductId equals prod.ProductId
                    select new CartInfoDto
                    {
                        CartId = cart.CartId,
                        CustomerId = id,
                        Approved = cart.Approved,
                        Canceled = cart.Canceled,
                        Created = cart.Created,
                        Shipped = cart.Shipped,
                        ShippingCost = cart.ShippingCost,
                        Total = 100,
                        CartItemList = new List<CartItemInfoDto>
                        {
                            new CartItemInfoDto
                            {
                                CartItemId = item.CartItemId,
                                CartId = item.CartId,
                                Quantity = item.Quantity,
                                Product = prod
                            }
                        }
                    };

                return new CustomerShoppingHistoryInfoDto
                {
                    Customer = ctx.CustomerSet.SingleOrDefault(c => c.CustomerId == id),
                    CartList = query.ToList()
                };
            }
        }

        #endregion
    }
}
