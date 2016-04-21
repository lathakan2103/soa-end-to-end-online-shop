using System.Collections.Generic;
using Core.Common.Contracts;
using Demo.Business.Contracts;
using Demo.Business.Entities;
using Demo.Data.Contracts;

namespace Demo.Business.Common
{
    public interface IShoppingEngine : IBusinessEngine
    {
        CustomerShoppingHistoryInfo GetShoppingHistoryInfo(int customerId, ICartRepository cartRepository);

        Customer CheckCustomerOwnership(ICustomerRepository customerRepository, int customerId);

        void ProcessOrder(Cart cart);
    }
}