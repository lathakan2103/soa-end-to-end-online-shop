using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Demo.Business.Common;
using Demo.Business.Contracts;
using Demo.Business.Entities;
using Demo.Common;
using Demo.Data.Contracts;
using System.Threading;
using System.Security.Principal;

namespace Demo.Business.Managers
{
    /// <summary>
    /// do set initialization per call and not per session (default)
    /// because it is not scalable
    /// set concurency mode to multiple (default = single) because we have per call situation
    /// ReleaseServiceInstanceOnTransactionComplete because there will be operations with attribute 
    /// TransactionScopeRequired = true
    /// </summary>
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class ShoppingManager : ManagerBase, IShoppingService
    {
        #region Fields

        [Import]
        private IDataRepositoryFactory _repositoryFactory;

        [Import]
        private IBusinessEngineFactory _businessFactory;

        #endregion

        #region C-Tor

        /// <summary>
        /// default c-tor for wcf
        /// </summary>
        public ShoppingManager()
        {

        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public ShoppingManager(IDataRepositoryFactory repositoryFactory)
        {
            this._repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="businessFactory"></param>
        public ShoppingManager(IBusinessEngineFactory businessFactory)
        {
            this._businessFactory = businessFactory;
        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="repositoryFactory"></param>
        /// <param name="businessFactory"></param>
        public ShoppingManager(IDataRepositoryFactory repositoryFactory, IBusinessEngineFactory businessFactory)
        {
            this._repositoryFactory = repositoryFactory;
            this._businessFactory = businessFactory;
        }

        #endregion

        #region Overrides

        protected override Customer LoadAuthorizationValidationCustomer(string loginEmail)
        {
            var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
            var authenticatedCustomer = customerRepository.GetByLogin(loginEmail);

            if (authenticatedCustomer != null) return authenticatedCustomer;

            var exception = new NotFoundException($"Customer with login: {loginEmail} was not found");
            throw new FaultException<NotFoundException>(exception, exception.Message);
        }

        #endregion

        #region IShoppingService implementation

        /// <summary>
        /// it's a fetch operation so no need for transaction
        /// </summary>
        /// <param name="loginEmail"></param>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public CustomerShoppingHistoryInfo GetShoppingHistory(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                var customer = customerRepository.GetByLogin(loginEmail);
                if (customer == null)
                {
                    var exception = new NotFoundException($"Customer with login: {loginEmail} was not found");
                    throw new FaultException<NotFoundException>(exception, exception.Message);
                }

                // make user equality with the login email
                ValidateAuthorization(customer);

                return shoppingEngine.GetShoppingHistoryInfo(customer.CustomerId, cartRepository);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Cart GetCartByCartId(int cartId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                var cart = cartRepository.Get(cartId);
                if (cart == null)
                {
                    throw new NotFoundException($"Cart with id: {cartId} was not found");
                }

                var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, cart.CustomerId);

                // make user equality with the login email
                ValidateAuthorization(customer);

                return cart;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Cart[] GetCartsByDateRange(DateTime start, DateTime end, int? customerId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();

                // if true then it is the current usser
                // and if it is false then it musst be the admin
                if (customerId.HasValue)
                {
                    var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();
                    var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, customerId.Value);

                    // make user equality with the login email
                    ValidateAuthorization(customer);
                }

                var carts = cartRepository.Get().Where(c => c.Created >= start && c.Created <= end);

                return carts.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Cart[] GetCartsByCustomer(int customerId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, customerId);

                // make user equality with the login email
                ValidateAuthorization(customer);

                var carts = cartRepository.Get().Where(c => c.CustomerId == customerId);

                return carts.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Cart[] GetCanceledCarts(DateTime start, DateTime end, int? customerId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();

                if (customerId.HasValue)
                {
                    return cartRepository
                                .Get()
                                .Where(c => c.Created >= start && c.Created <= end && c.Canceled.HasValue && c.CustomerId == customerId.Value)
                                .ToArray();
                }

                return cartRepository
                            .Get()
                            .Where(c => c.Created >= start && c.Created <= end && c.Canceled.HasValue)
                            .ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Cart[] GetApprovedCarts(DateTime start, DateTime end, int? customerId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();

                if (customerId.HasValue)
                {
                    return cartRepository
                                .Get()
                                .Where(c => c.Created >= start && c.Created <= end && c.Approved.HasValue && c.CustomerId == customerId.Value)
                                .ToArray();
                }

                return cartRepository
                            .Get()
                            .Where(c => c.Created >= start && c.Created <= end && c.Approved.HasValue)
                            .ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Cart[] GetShippedCarts(DateTime start, DateTime end, int? customerId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();

                if (customerId.HasValue)
                {
                    return cartRepository
                                .Get()
                                .Where(c => c.Created >= start && c.Created <= end && c.Shipped.HasValue && c.CustomerId == customerId.Value)
                                .ToArray();
                }

                return cartRepository
                            .Get()
                            .Where(c => c.Created >= start && c.Created <= end && c.Shipped.HasValue)
                            .ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Cart[] GetNewCarts()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();

                var carts = cartRepository.Get().Where(c => !c.Canceled.HasValue && !c.Approved.HasValue && !c.Shipped.HasValue);

                return carts.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Cart[] GetCartsWithTotalAmountGreaterThen(decimal totalAmount)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();

                var carts = cartRepository.Get().Where(c => c.Total > totalAmount);

                return carts.ToArray();
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public void SetCartAsCanceled(int cartId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                var cart = cartRepository.Get(cartId);
                if (cart == null)
                {
                    throw new NotFoundException($"Cart with id: {cartId} was not found");
                }

                var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, cart.CustomerId);

                // make user equality with the login email
                ValidateAuthorization(customer);

                cart.Canceled = DateTime.Today;
                var result = cartRepository.Update(cart);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public void SetCartAsApproved(int cartId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();

                var cart = cartRepository.Get(cartId);
                if (cart == null)
                {
                    throw new NotFoundException($"Cart with id: {cartId} was not found");
                }

                cart.Approved = DateTime.Today;

                cartRepository.Update(cart);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public void SetCartAsShipped(int cartId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();

                var cart = cartRepository.Get(cartId);
                if (cart == null)
                {
                    throw new NotFoundException($"Cart with id: {cartId} was not found");
                }

                cart.Shipped = DateTime.Today;

                cartRepository.Update(cart);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Cart AddCart(Cart cart)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                try
                {
                    var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                    var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                    var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                    var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, cart.CustomerId);

                    ValidateAuthorization(customer);

                    return cartRepository.Add(cart);
                }
                catch (NotFoundException ex)
                {
                    var exception = new NotFoundException($"Customer with id: {cart.CustomerId} was not found");
                    throw new FaultException<NotFoundException>(exception, exception.Message);
                }
                catch (Exception ex)
                {
                    throw new FaultException(ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public void AddCartItemToCart(int cartId, CartItem item)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var cartItemRepository = this._repositoryFactory.GetDataRepository<ICartItemRepository>();
                var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                var cart = cartRepository.Get(cartId);
                if (cart == null)
                {
                    throw new NotFoundException($"Cart with id: {cartId} was not found");
                }

                var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, cart.CustomerId);

                // make user equality with the login email
                ValidateAuthorization(customer);

                item.CartId = cartId;
                var result = cartItemRepository.Add(item);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public void AddCartItemsToCart(int cartId, CartItem[] items)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var cartItemRepository = this._repositoryFactory.GetDataRepository<ICartItemRepository>();
                var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                var cart = cartRepository.Get(cartId);
                if (cart == null)
                {
                    throw new NotFoundException($"Cart with id: {cartId} was not found");
                }

                var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, cart.CustomerId);

                // make user equality with the login email
                ValidateAuthorization(customer);

                foreach (var i in items)
                {
                    i.CartId = cartId;
                    cartItemRepository.Add(i);
                }                
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public IEnumerable<CartItemInfo> GetCartItemsByCartId(int cartId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var cartItemRepository = this._repositoryFactory.GetDataRepository<ICartItemRepository>();
                var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                var cart = cartRepository.Get(cartId);
                if (cart == null)
                {
                    throw new NotFoundException($"Cart with id: {cartId} was not found");
                }

                var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, cart.CustomerId);
                var dtos = cartItemRepository.GetCartItemsByCartId(cartId);

                // make user equality with the login email
                ValidateAuthorization(customer);

                return dtos.Select(dto => new CartItemInfo
                {
                    CartItemId = dto.CartItemId,
                    Quantity = dto.Quantity,
                    Product = dto.Product,
                    CartId = dto.CartId
                }).ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public IEnumerable<Cart> GetCarts()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var cartItemRepository = this._repositoryFactory.GetDataRepository<ICartItemRepository>();

                IEnumerable<Cart> carts = cartRepository.Get();
                if (carts == null)
                {
                    throw new NotFoundException($"No carts found");
                }

                return carts;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public void CloseCart(int cartId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var customerRepository = this._repositoryFactory.GetDataRepository<ICustomerRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var shoppingEngine = this._businessFactory.GetBusinessEngine<IShoppingEngine>();

                var cart = cartRepository.Get(cartId);
                if (cart == null)
                {
                    throw new NotFoundException($"Cart with id: {cartId} was not found");
                }

                var customer = shoppingEngine.CheckCustomerOwnership(customerRepository, cart.CustomerId);

                // make user equality with the login email
                ValidateAuthorization(customer);

                // process cart and send it to the db
                // 1) stock check
                // 2) check promotions
                // 3) shipping cost
                // 4) send notofication
                shoppingEngine.ProcessOrder(cart);
            });
        }

        #endregion
    }
}
