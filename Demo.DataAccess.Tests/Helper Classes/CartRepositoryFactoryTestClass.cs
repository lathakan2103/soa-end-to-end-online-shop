using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Contracts;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.DataAccess.Tests.Helper_Classes
{
    public class CartRepositoryFactoryTestClass
    {
        #region Fields

        [Import]
        private IDataRepositoryFactory _factory;

        #endregion

        #region C-Tor

        public CartRepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public CartRepositoryFactoryTestClass(IDataRepositoryFactory factory)
        {
            this._factory = factory;
        }

        #endregion

        #region Methods

        public IEnumerable<Cart> GetAllCarts()
        {
            return this._factory.GetDataRepository<ICartRepository>().Get();
        }

        #endregion
    }
}
