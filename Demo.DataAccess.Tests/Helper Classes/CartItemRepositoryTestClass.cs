using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.DataAccess.Tests.Helper_Classes
{
    public class CartItemRepositoryTestClass
    {
        #region Fields

        [Import]
        private ICartItemRepository _repository;

        #endregion

        #region C-Tor

        public CartItemRepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public CartItemRepositoryTestClass(ICartItemRepository repository)
        {
            this._repository = repository;
        }

        #endregion

        #region Methods

        public IEnumerable<CartItem> GetAllCartItems()
        {
            return this._repository.Get();
        }

        #endregion
    }
}
