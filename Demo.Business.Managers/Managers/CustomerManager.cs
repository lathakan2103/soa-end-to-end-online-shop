using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Demo.Business.Contracts;
using Demo.Business.Entities;
using Demo.Common;
using Demo.Data.Contracts;

namespace Demo.Business.Managers
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple, 
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class CustomerManager : ManagerBase, ICustomerService
    {
        #region Fields

        [Import]
        private IDataRepositoryFactory _repositoryFactory;

        [Import]
        private IBusinessEngineFactory _businessFactory;

        #endregion

        #region C-Tor

        public CustomerManager()
        {
            
        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public CustomerManager(IDataRepositoryFactory repositoryFactory)
        {
            this._repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="businessFactory"></param>
        public CustomerManager(IBusinessEngineFactory businessFactory)
        {
            this._businessFactory = businessFactory;
        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="repositoryFactory"></param>
        /// <param name="businessFactory"></param>
        public CustomerManager(IDataRepositoryFactory repositoryFactory, IBusinessEngineFactory businessFactory)
        {
            this._repositoryFactory = repositoryFactory;
            this._businessFactory = businessFactory;
        }

        #endregion

        #region ICustomerService implementation

        //[PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        //[PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Customer GetCustomerByLogin(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();

                var customer = customerRepository.GetByLogin(loginEmail);
                if (customer == null)
                {
                    var exception = new NotFoundException($"Customer with login: {loginEmail} was not found");
                    throw new FaultException<NotFoundException>(exception, exception.Message);
                }

                // make user equality with the login email
                ValidateAuthorization(customer);

                return customer;
            });
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Customer[] GetCustomers()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();

                var customers = customerRepository.Get();

                return customers.ToArray();
            });
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Customer[] GetActiveCustomers()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();

                var customers = customerRepository.Get().Where(c => c.IsActive);

                return customers.ToArray();
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        //[PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        //[PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Customer UpdateCustomer(Customer customer)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ValidateAuthorization(customer);

                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var result = customer.CustomerId != 0
                    ? customerRepository.Update(customer)
                    : customerRepository.Add(customer);
                return result;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        //[PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public void DeleteCustomer(int customerId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();

                var customer = customerRepository.Get(customerId);
                customer.IsActive = false;

                var result = customerRepository.Update(customer);
            });
        }

        #endregion
    }
}
