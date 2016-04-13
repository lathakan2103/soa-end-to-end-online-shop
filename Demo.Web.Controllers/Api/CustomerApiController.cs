using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Common.Contracts;
using Demo.Client.Contracts;
using Demo.Client.Entities;
using Demo.Web.Controllers.Core;

namespace Demo.Web.Controllers.Api
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/customer")]
    [Authorize]
    [UsesDisposableService]
    public class CustomerApiController : ApiControllerBase
    {
        #region Fields

        private readonly ICustomerService _customerService;

        #endregion

        #region C-Tor

        [ImportingConstructor]
        public CustomerApiController(ICustomerService customerService)
        {
            this._customerService = customerService;
        }

        #endregion

        #region Overrides

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(this._customerService);
        }

        #endregion

        #region HttpGet

        [HttpGet]
        [Route("settings")]
        public HttpResponseMessage GetCustomerAccountInfo(HttpRequestMessage request)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var customer = this._customerService.GetCustomerByLogin(User.Identity.Name);
                customer.ExpirationDate = customer.ExpirationDate.Insert(2, @"/");
                return request.CreateResponse(HttpStatusCode.OK, customer);
            });
        }

        #endregion

        #region HttpPost

        [HttpPost]
        [Route("settings")]
        public HttpResponseMessage UpdateCustomerAccountInfo(HttpRequestMessage request, Customer settingsModel)
        {
            return GetHttpResponseMessage(request, () =>
            {
                HttpResponseMessage response;

                ValidateAuthorizedUser(settingsModel.LoginEmail);

                var errors = new List<string>();

                var states = UIHelper.GetStates();
                var state = states.FirstOrDefault(item => item.Abbrev.ToUpper() == settingsModel.State.ToUpper());
                if (state == null)
                {
                    errors.Add("Invalid state.");
                }

                // trim out the / in the exp date
                settingsModel.ExpirationDate = settingsModel.ExpirationDate.Substring(0, 2) + settingsModel.ExpirationDate.Substring(3, 2);

                if (errors.Count == 0)
                {
                    this._customerService.UpdateCustomer(settingsModel);
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());
                }

                return response;
            });
        }

        #endregion
    }
}
