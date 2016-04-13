using Core.Common.Contracts;
using Demo.Business.Entities;

namespace Demo.Data.Contracts
{
    public interface ICustomerRepository : IDataRepository<Customer>
    {
        Customer GetByLogin(string loginEmail);
    }
}