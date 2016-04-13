using System.ServiceModel;
using Core.Common.Exceptions;
using Demo.Business.Entities;
using Demo.Common;

namespace Demo.Business.Contracts
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        //[FaultContract(typeof(AuthorizationValidationException))]
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
    }
}
