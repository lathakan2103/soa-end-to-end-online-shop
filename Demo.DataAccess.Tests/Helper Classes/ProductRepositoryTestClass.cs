using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.DataAccess.Tests.Helper_Classes
{
    public class ProductRepositoryTestClass
    {
        #region Fields

        [Import]
        private IProductRepository _repository;

        #endregion

        #region C-Tor

        public ProductRepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public ProductRepositoryTestClass(IProductRepository repository)
        {
            this._repository = repository;
        }

        #endregion

        #region Methods

        public IEnumerable<Product> GetAllProducts()
        {
            return this._repository.Get();
        }

        #endregion
    }
}
