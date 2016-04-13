using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Core.Common.ServiceModel;
using Demo.Client.Contracts;
using Demo.Client.Entities;

namespace Demo.Client.Proxies
{
    [Export(typeof(ICustomerService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CustomerClient : UserClientBase<ICustomerService>, ICustomerService
    {
        public Customer GetCustomerByLogin(string loginEmail)
        {
            return Channel.GetCustomerByLogin(loginEmail);
        }

        public Customer[] GetCustomers()
        {
            return Channel.GetCustomers();
        }

        public Customer[] GetActiveCustomers()
        {
            return Channel.GetActiveCustomers();
        }

        public Customer UpdateCustomer(Customer customer)
        {
            return Channel.UpdateCustomer(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            Channel.DeleteCustomer(customerId);
        }

        public Task<Customer> GetCustomerByLoginAsync(string loginEmail)
        {
            return Channel.GetCustomerByLoginAsync(loginEmail);
        }

        public Task<Customer[]> GetCustomersAsync()
        {
            return Channel.GetCustomersAsync();
        }

        public Task<Customer[]> GetActiveCustomersAsync()
        {
            return Channel.GetActiveCustomersAsync();
        }

        public Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            return Channel.UpdateCustomerAsync(customer);
        }

        public Task DeleteCustomerAsync(int customerId)
        {
            return Channel.DeleteCustomerAsync(customerId);
        }
    }
}
