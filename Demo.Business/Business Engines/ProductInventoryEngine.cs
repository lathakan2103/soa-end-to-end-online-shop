using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Demo.Business.Common;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.Business.Business_Engines
{
    [Export(typeof(IProductInventoryEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProductInventoryEngine : IProductInventoryEngine
    {
        /// <summary>
        /// here i am going to define the rules to be most wanted
        /// for example: if product's quantity is > then 3
        /// </summary>
        /// <param name="productId">product's id to seacrh</param>
        /// <param name="carts">carts list for the particular timespan</param>
        /// <param name="repo">cart item repository</param>
        /// <returns></returns>
        public bool IsMostWanted(int productId, IEnumerable<Cart> carts, ICartItemRepository repo)
        {
            var quantity = 0;
            foreach (var c in carts)
            {
                var item = repo.GetCartItemsByCartId(c.CartId).SingleOrDefault(i => i.Product.ProductId == productId);
                if (item != null)
                {
                    quantity += item.Quantity;
                }
            }

            // i know this should not be hard coded (quantity parameter => 3)
            // but for my purposes here it is ok for me
            return quantity > 3;
        }

        public string GenerateArticleNumber()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
