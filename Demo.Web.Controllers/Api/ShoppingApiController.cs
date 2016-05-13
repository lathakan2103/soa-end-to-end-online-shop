using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Common.Contracts;
using Demo.Client.Contracts;
using Demo.Client.Entities;
using Demo.Client.Proxies;
using System.ServiceModel.Description;
using Demo.Client.Proxies.Service_Procies;

namespace Demo.Web.Controllers.Api
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    [RoutePrefix("api/shopping")]
    [UsesDisposableService]
    public class ShoppingApiController : ApiControllerBase
    {
        #region Fields

        private readonly IShoppingService _shoppingClient;
        private readonly IInventoryService _inventoryClient;
        private readonly ICustomerService _customerClient;
        private bool _isTest = false;

        #endregion

        #region C-Tor

        [ImportingConstructor]
        public ShoppingApiController(
            IShoppingService shoppingClient,
            IInventoryService inventoryClient,
            ICustomerService customerClient)
        {
            this._shoppingClient = shoppingClient;
            this._inventoryClient = inventoryClient;
            this._customerClient = customerClient;
        }

        public ShoppingApiController(
            IShoppingService shoppingClient,
            IInventoryService inventoryClient,
            ICustomerService customerClient,
            bool isTest)
        {
            this._shoppingClient = shoppingClient;
            this._inventoryClient = inventoryClient;
            this._customerClient = customerClient;

            this._isTest = isTest;
        }

        #endregion

        #region Overrides

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(this._shoppingClient);
            disposableServices.Add(this._inventoryClient);
        }

        #endregion

        #region HttpGet

        [HttpGet]
        [Route("products")]
        public HttpResponseMessage GetActiveProducts(HttpRequestMessage request)
        {
            return GetHttpResponseMessage(request, () =>
            {
                this.SetInventoryCredentials();
                var products = this._inventoryClient.GetActiveProducts().ToArray();
                return request.CreateResponse(HttpStatusCode.OK, products);
            });
        }

        [HttpGet]
        [Route("product/{productId}")]
        public HttpResponseMessage GetProduct(HttpRequestMessage request, int productId)
        {
            return GetHttpResponseMessage(request, () =>
            {
                this.SetInventoryCredentials();
                var product = this._inventoryClient.GetProductById(productId);
                return request.CreateResponse(HttpStatusCode.OK, product);
            });
        }

        [HttpGet]
        [Route("history")]
        public HttpResponseMessage GetShoppingHistory(HttpRequestMessage request)
        {
            return GetHttpResponseMessage(request, () =>
            {
                this.SetShoppingCredentials();
                var history = this._shoppingClient.GetShoppingHistory(User.Identity.Name);
                return request.CreateResponse(HttpStatusCode.OK, history);
            });
        }

        [HttpGet]
        [Route("history/{cartId}")]
        public HttpResponseMessage GetCartItemsByCartId(HttpRequestMessage request, int cartId)
        {
            return GetHttpResponseMessage(request, () =>
            {
                this.SetShoppingCredentials();
                var cart = this._shoppingClient.GetCartItemsByCartId(cartId);
                return request.CreateResponse(HttpStatusCode.OK, cart);
            });
        }

        #endregion

        #region HttpPost

        [HttpPost]
        [Route("buy/{productId}")]
        public HttpResponseMessage BuyProduct(HttpRequestMessage request, int productId)
        {
            return GetHttpResponseMessage(request, () =>
            {
                this.SetCustomerCredentials();
                this.SetInventoryCredentials();
                this.SetShoppingCredentials();

                var product = this._inventoryClient.GetProductById(productId);
                var id = this._customerClient.GetCustomerByLogin(User.Identity.Name).CustomerId;

                var cart = this._shoppingClient.GetActiveCart(id);
                if (cart == null)
                {
                    cart = this._shoppingClient.AddCart(
                         new Cart
                         {
                             CustomerId = id,
                             Created = DateTime.Now,
                             Total = product.Price,
                             StilOpen = true
                         });
                }

                this._shoppingClient.AddCartItemToCart(
                    cart.CartId, 
                    new CartItem
                    {
                        CartId = cart.CartId,
                        ProductId = productId,
                        Quantity = 1
                    });

                return request.CreateResponse(HttpStatusCode.OK);
            });
        }

        [HttpPost]
        [Route("cancel/{cartId}")]
        public HttpResponseMessage CancelCart(HttpRequestMessage request, int cartId)
        {
            return GetHttpResponseMessage(request, () =>
            {
                this.SetShoppingCredentials();

                var cart = this._shoppingClient.GetCartByCartId(cartId);
                if (cart != null)
                {
                    this._shoppingClient.SetCartAsCanceled(cartId);
                    return request.CreateResponse(HttpStatusCode.OK);
                }

                return request.CreateResponse(HttpStatusCode.NotFound);
            });
        }

        [HttpPost]
        [Route("closecart")]
        public HttpResponseMessage CloseCart(HttpRequestMessage request)
        {
            return GetHttpResponseMessage(request, () =>
            {
                this.SetShoppingCredentials();
                this.SetCustomerCredentials();

                var id = this._customerClient.GetCustomerByLogin(User.Identity.Name).CustomerId;
                var cart = this._shoppingClient.GetActiveCart(id);

                if (cart == null)
                {
                    return request.CreateResponse(HttpStatusCode.NotFound);
                }

                this._shoppingClient.CloseCart(cart.CartId);

                return request.CreateResponse(HttpStatusCode.OK);
            });
        }

        #endregion

        #region Helpers

        private void SetShoppingCredentials()
        {
            if (this._isTest) return;

            var credentials = (this._shoppingClient as ShoppingClient).ChannelFactory.Endpoint.Behaviors.Remove<ClientCredentials>();
            credentials.UserName.UserName = "pingo";
            credentials.UserName.Password = "07061971";
            (this._shoppingClient as ShoppingClient).ChannelFactory.Endpoint.Behaviors.Add(credentials);
        }

        private void SetInventoryCredentials()
        {
            if (this._isTest) return;

            var credentials = (this._inventoryClient as InventoryClient).ChannelFactory.Endpoint.Behaviors.Remove<ClientCredentials>();
            credentials.UserName.UserName = "pingo";
            credentials.UserName.Password = "07061971";
            (this._inventoryClient as InventoryClient).ChannelFactory.Endpoint.Behaviors.Add(credentials);
        }

        private void SetCustomerCredentials()
        {
            if (this._isTest) return;

            var credentials = (this._customerClient as CustomerClient).ChannelFactory.Endpoint.Behaviors.Remove<ClientCredentials>();
            credentials.UserName.UserName = "pingo";
            credentials.UserName.Password = "07061971";
            (this._customerClient as CustomerClient).ChannelFactory.Endpoint.Behaviors.Add(credentials);
        }

        #endregion
    }
}
