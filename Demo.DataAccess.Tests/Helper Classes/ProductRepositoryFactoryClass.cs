using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Contracts;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.DataAccess.Tests.Helper_Classes
{
    public class ProductRepositoryFactoryClass
    {
        #region Fields

        [Import]
        private IDataRepositoryFactory _factory;

        #endregion

        #region C-Tor

        public ProductRepositoryFactoryClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public ProductRepositoryFactoryClass(IDataRepositoryFactory factory)
        {
            this._factory = factory;
        }

        #endregion

        #region Methods

        public IEnumerable<Product> GetAllProducts()
        {
            return this._factory.GetDataRepository<IProductRepository>().Get();
        }

        #endregion
    }
}
