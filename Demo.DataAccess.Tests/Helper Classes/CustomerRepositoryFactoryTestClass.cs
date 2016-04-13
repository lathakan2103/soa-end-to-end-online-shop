using System.Collections.Generic;
using System.ComponentModel.Composition;
using Core.Common.Contracts;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.DataAccess.Tests
{
    public class CustomerRepositoryFactoryTestClass
    {
        #region Fields

        [Import]
        private IDataRepositoryFactory _factory;

        #endregion

        #region C-Tor

        public CustomerRepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public CustomerRepositoryFactoryTestClass(IDataRepositoryFactory factory)
        {
            this._factory = factory;
        }

        #endregion

        #region Methods

        public IEnumerable<Customer> GetAllCustomers()
        {
            var repository = this._factory.GetDataRepository<ICustomerRepository>();
            var customers = repository.Get();
            return customers;
        }

        public Customer GetByLogin(string loginEmail)
        {
            var repository = this._factory.GetDataRepository<ICustomerRepository>();
            return repository.GetByLogin(loginEmail);
        }

        #endregion
    }
}
