using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using Demo.Data.Contracts.Dto;

namespace Demo.DataAccess.Tests.Helper_Classes
{
    public class CartRepositoryTestClass
    {
        #region Fields

        [Import]
        private ICartRepository _repository;

        #endregion

        #region C-Tor

        public CartRepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public CartRepositoryTestClass(ICartRepository repository)
        {
            this._repository = repository;
        }

        #endregion

        #region Methods

        public IEnumerable<Cart> GetAllCarts()
        {
            return this._repository.Get();
        }

        public CustomerShoppingHistoryInfoDto GetCustomerShoppingInfo(int customerId)
        {
            return this._repository.GetCustomerShoppingHistory(customerId);
        }

        #endregion
    }
}
