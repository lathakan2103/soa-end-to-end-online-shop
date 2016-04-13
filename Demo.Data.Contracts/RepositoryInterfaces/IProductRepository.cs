using Core.Common.Contracts;
using Demo.Business.Entities;

namespace Demo.Data.Contracts
{
    public interface IProductRepository : IDataRepository<Product>
    {
        Product GetProductByArticleNumber(string articleNumber);
        Product[] GetActiveProducts();
    }
}