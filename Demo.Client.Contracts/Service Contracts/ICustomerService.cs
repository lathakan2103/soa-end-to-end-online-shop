using System.ServiceModel;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Demo.Client.Entities;
using Demo.Common;

namespace Demo.Client.Contracts
{
    [ServiceContract]
    public interface ICustomerService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Customer GetCustomerByLogin(string loginEmail);

        [OperationContract]
        Customer[] GetCustomers();

        [OperationContract]
        Customer[] GetActiveCustomers();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Customer UpdateCustomer(Customer customer);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        void DeleteCustomer(int customerId);

        #region Async

        [OperationContract]
        Task<Customer> GetCustomerByLoginAsync(string loginEmail);

        [OperationContract]
        Task<Customer[]> GetCustomersAsync();

        [OperationContract]
        Task<Customer[]> GetActiveCustomersAsync();

        [OperationContract]
        Task<Customer> UpdateCustomerAsync(Customer customer);

        [OperationContract]
        Task DeleteCustomerAsync(int customerId);

        #endregion
    }
}
