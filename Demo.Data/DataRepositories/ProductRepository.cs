using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.Data.DataRepositories
{
    [Export(typeof(IProductRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProductRepository : DataRepositoryBase<Product>, IProductRepository
    {
        #region IDataRepository<Product> implementation

        protected override Product AddEntity(DemoContext ctx, Product entity)
        {
            return ctx.ProductSet.Add(entity);
        }

        protected override Product UpdateEntity(DemoContext ctx, Product entity)
        {
            return ctx.ProductSet.FirstOrDefault(p => p.ProductId == entity.ProductId);
        }

        protected override IEnumerable<Product> GetEntities(DemoContext ctx)
        {
            return ctx.ProductSet;
        }

        protected override Product GetEntity(DemoContext ctx, int id)
        {
            return ctx.ProductSet.FirstOrDefault(p => p.ProductId == id);
        }

        #endregion

        #region IProductRepository implementation

        public Product GetProductByArticleNumber(string articleNumber)
        {
            using (var ctx = new DemoContext())
            {
                return ctx.ProductSet.FirstOrDefault(p => p.ArticleNumber.Equals(articleNumber));
            }
        }

        public Product[] GetActiveProducts()
        {
            using (var ctx = new DemoContext())
            {
                return ctx.ProductSet.Where(p => p.IsActive).ToArray();
            }
        }

        #endregion
    }
}
