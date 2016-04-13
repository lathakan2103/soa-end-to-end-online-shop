using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.Data.DataRepositories
{
    [Export(typeof(ICustomerRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CustomerRepository : DataRepositoryBase<Customer>, ICustomerRepository
    {
        #region IDataRepository<Customer> implementation

        protected override Customer AddEntity(DemoContext ctx, Customer entity)
        {
            return ctx.CustomerSet.Add(entity);
        }

        protected override Customer UpdateEntity(DemoContext ctx, Customer entity)
        {
            return ctx.CustomerSet.FirstOrDefault(c => c.CustomerId == entity.CustomerId);
        }

        protected override IEnumerable<Customer> GetEntities(DemoContext ctx)
        {
            return ctx.CustomerSet;
        }

        protected override Customer GetEntity(DemoContext ctx, int id)
        {
            return ctx.CustomerSet.FirstOrDefault(c => c.CustomerId == id);
        }

        #endregion

        #region ICustomerRepository implementation

        public Customer GetByLogin(string loginEmail)
        {
            using (var ctx = new DemoContext())
            {
                return ctx.CustomerSet.FirstOrDefault(c => c.LoginEmail.Equals(loginEmail));
            }
        }

        #endregion
    }
}
