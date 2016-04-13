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

        private readonly IShoppingService _shoppingService;
        private readonly IInventoryService _inventoryService;
        private readonly ICustomerService _customerService;

        #endregion

        #region C-Tor

        [ImportingConstructor]
        public ShoppingApiController(
            IShoppingService shoppingService,
            IInventoryService inventoryService, 
            ICustomerService customerService)
        {
            this._shoppingService = shoppingService;
            this._inventoryService = inventoryService;
            this._customerService = customerService;
        }

        #endregion

        #region Overrides

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(this._shoppingService);
            disposableServices.Add(this._inventoryService);
            disposableServices.Add(this._customerService);
        }

        #endregion

        #region HttpGet

        [HttpGet]
        [Route("products")]
        public HttpResponseMessage GetActiveProducts(HttpRequestMessage request)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var products = this._inventoryService.GetActiveProducts().ToArray();
                return request.CreateResponse(HttpStatusCode.OK, products);
            });
        }

        [HttpGet]
        [Route("product/{productId}")]
        public HttpResponseMessage GetProduct(HttpRequestMessage request, int productId)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var product = this._inventoryService.GetProductById(productId);
                return request.CreateResponse(HttpStatusCode.OK, product);
            });
        }

        [HttpGet]
        [Route("history")]
        public HttpResponseMessage GetShoppingHistory(HttpRequestMessage request)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var history = this._shoppingService.GetShoppingHistory(User.Identity.Name);

                return request.CreateResponse(HttpStatusCode.OK, history);
            });
        }

        [HttpGet]
        [Route("history/{cartId}")]
        public HttpResponseMessage GetCartItemsByCartId(HttpRequestMessage request, int cartId)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var cart = this._shoppingService.GetCartItemsByCartId(cartId);

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
                var product = this._inventoryService.GetProductById(productId);
                var id = this._customerService.GetCustomerByLogin(User.Identity.Name).CustomerId;

                var cart = this._shoppingService.AddCart(
                    new Cart
                    {
                        CustomerId = id,
                        Created = DateTime.Now,
                        Total = product.Price
                    });

                this._shoppingService.AddCartItemToCart(
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
                var cart = this._shoppingService.GetCartByCartId(cartId);
                if (cart != null)
                {
                    this._shoppingService.SetCartAsCanceled(cartId);
                    return request.CreateResponse(HttpStatusCode.OK);
                }

                return request.CreateResponse(HttpStatusCode.NotFound);
            });
        }

        #endregion
    }
}
