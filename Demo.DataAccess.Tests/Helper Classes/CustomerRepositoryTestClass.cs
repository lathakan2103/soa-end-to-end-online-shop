using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.DataAccess.Tests
{
    public class CustomerRepositoryTestClass
    {
        #region Fields

        [Import]
        private ICustomerRepository _repository;

        #endregion

        #region C-Tor

        public CustomerRepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public CustomerRepositoryTestClass(ICustomerRepository repository)
        {
            this._repository = repository;
        }

        #endregion

        #region Methods

        public IEnumerable<Customer> GetAllCustomers()
        {
            var customers = this._repository.Get();
            return customers;
        }

        public Customer GetByLogin(string loginEmail)
        {
            return this._repository.GetByLogin(loginEmail);
        }

        #endregion
    }
}
