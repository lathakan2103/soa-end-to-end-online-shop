using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
                var carts = ctx.CartSet.Where(c => c.CustomerId == id).ToList();
                var dtoList = new List<CartInfoDto>();
                foreach (var c in carts)
                {
                    dtoList.Add(
                        new CartInfoDto
                        {
                            CustomerId = c.CustomerId,
                            CartId = c.CartId,
                            StilOpen = c.StilOpen,
                            Approved = c.Approved,
                            Created = c.Created,
                            Canceled = c.Canceled,
                            Shipped = c.Shipped,
                            ShippingCost = c.ShippingCost
                        });
                }

                return new CustomerShoppingHistoryInfoDto
                {
                    Customer = ctx.CustomerSet.SingleOrDefault(c => c.CustomerId == id),
                    CartList = dtoList
                };
            }
        }

        public List<CartItemInfoDto> GetCartItemsByCartId(int cartId)
        {
            List<CartItem> items = new List<CartItem>();
            List<CartItemInfoDto> dtoList = new List<CartItemInfoDto>();
            using (var ctx = new DemoContext())
            {
                items = ctx.CartItemSet.Where(c => c.CartId == cartId).ToList();
                
            }

            using (var ctx = new DemoContext())
            {
                foreach (var i in items)
                {
                    var product = ctx.ProductSet.SingleOrDefault(p => p.ProductId == i.ProductId);
                    dtoList.Add(
                        new CartItemInfoDto
                        {
                            CartId = i.CartId,
                            CartItemId = i.CartItemId,
                            Quantity = i.Quantity,
                            Product = product
                        });
                }
            }

            return dtoList;

        } 

        public void CloseCart(int cartId)
        {
            using (var ctx = new DemoContext())
            {
                var cart = ctx.CartSet.SingleOrDefault(c => c.CartId == cartId);
                if (cart == null || !cart.StilOpen)
                {

                }

                cart.StilOpen = false;
                ctx.SaveChanges();
            }
        }

        #endregion
    }
}
