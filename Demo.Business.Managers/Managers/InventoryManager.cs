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
using Demo.Data.Contracts;
using Demo.Common;
using System.Threading;

namespace Demo.Business.Managers
{
    /// <summary>
    /// do set initialization per call and not per session (default)
    /// because it is not scalable
    /// set concurency mode to multiple (default = single) because we have per call situation
    /// set ReleaseServiceInstanceOnTransactionComplete to true if there will be at least 
    /// one operation with attribute TransactionScopeRequired = true
    /// </summary>
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall, 
        ConcurrencyMode = ConcurrencyMode.Multiple, 
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService
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
        public InventoryManager()
        {

        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public InventoryManager(IDataRepositoryFactory repositoryFactory)
        {
            this._repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="businessFactory"></param>
        public InventoryManager(IBusinessEngineFactory businessFactory)
        {
            this._businessFactory = businessFactory;
        }

        /// <summary>
        /// for test purposes
        /// </summary>
        /// <param name="repositoryFactory"></param>
        /// <param name="businessFactory"></param>
        public InventoryManager(IDataRepositoryFactory repositoryFactory, IBusinessEngineFactory businessFactory)
        {
            this._repositoryFactory = repositoryFactory;
            this._businessFactory = businessFactory;
        }

        #endregion

        #region IInventoryManager implementation

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Product[] GetProducts()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var productRepository = this._repositoryFactory.GetDataRepository<IProductRepository>();
                var products = productRepository.Get();

                return products.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Product[] GetActiveProducts()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var productRepository = this._repositoryFactory.GetDataRepository<IProductRepository>();
                var products = productRepository.GetActiveProducts();

                return products;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Product GetProductById(int id, bool acceptNullable = false)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var productRepository = this._repositoryFactory.GetDataRepository<IProductRepository>();
                var product = productRepository.Get(id);

                if (product == null && acceptNullable)
                {
                    return null;
                }

                if (product == null)
                {
                    var ex = new NotFoundException($"Product with id: {id} not found!");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return product;
            });
        }

        /// <summary>
        /// all commands should be transaction ready
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public Product UpdateProduct(Product product)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                Product updatedEntity;
                var productRepository = this._repositoryFactory.GetDataRepository<IProductRepository>();
                if (product.ProductId == 0)
                {
                    product.ArticleNumber =
                        this._businessFactory.GetBusinessEngine<IProductInventoryEngine>().GenerateArticleNumber();
                    updatedEntity = productRepository.Add(product);
                }
                else
                {
                    updatedEntity = productRepository.Update(product);
                }
                
                return updatedEntity;
            });
        }

        /// <summary>
        /// all commands should be transaction ready
        /// </summary>
        /// <param name="productId"></param>
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public void DeleteProduct(int productId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var productRepository = this._repositoryFactory.GetDataRepository<IProductRepository>();
                var product = productRepository.Get(productId);

                if (product == null)
                {
                    var ex = new NotFoundException($"Product with id: {productId} not found!");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                product.IsActive = false;
                var result = productRepository.Update(product);
            });
        }

        /// <summary>
        /// all commands should be transaction ready
        /// </summary>
        /// <param name="productId"></param>
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        public void ActivateProduct(int productId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var productRepository = this._repositoryFactory.GetDataRepository<IProductRepository>();
                var product = productRepository.Get(productId);

                if (product == null)
                {
                    var ex = new NotFoundException($"Product with id: {productId} not found!");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                product.IsActive = true;
                var result = productRepository.Update(product);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.DemoAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.DemoUser)]
        public Product[] GetMostWanted(DateTime start, DateTime end)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var productRepository = this._repositoryFactory.GetDataRepository<IProductRepository>();
                var cartRepository = this._repositoryFactory.GetDataRepository<ICartRepository>();
                var cartItemRepository = this._repositoryFactory.GetDataRepository<ICartItemRepository>();
                var businessEngine = this._businessFactory.GetBusinessEngine<IProductInventoryEngine>();

                // 1 - get all active products
                var products = productRepository.GetActiveProducts();

                // 2 - get all carts for the particular timespan                
                var carts = cartRepository.Get().Where(c => c.Created >= start && c.Created <= end).ToList();

                var result = new List<Product>();
                foreach (var p in products)
                {
                    if (businessEngine.IsMostWanted(p.ProductId, carts, cartItemRepository))
                    {
                        result.Add(p);
                    }
                }
                
                return result.ToArray();
            });
        }

        #endregion
    }
}
