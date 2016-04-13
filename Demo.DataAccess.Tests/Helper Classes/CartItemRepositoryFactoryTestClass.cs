using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Contracts;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.DataAccess.Tests.Helper_Classes
{
    public class CartItemRepositoryFactoryTestClass
    {
        #region Fields

        [Import]
        private IDataRepositoryFactory _factory;

        #endregion

        #region C-Tor

        public CartItemRepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public CartItemRepositoryFactoryTestClass(IDataRepositoryFactory factory)
        {
            this._factory = factory;
        }

        #endregion

        #region Methods

        public IEnumerable<CartItem> GetAllCartItems()
        {
            return this._factory.GetDataRepository<ICartItemRepository>().Get();
        }

        #endregion
    }
}
