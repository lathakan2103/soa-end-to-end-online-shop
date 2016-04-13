using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Demo.Web.Contracts;
using Demo.Web.Controllers.Core;
using Demo.Web.Models;

namespace Demo.Web.Controllers.Api
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/account")]
    public class AccountApiController : ApiControllerBase
    {
        #region Fields

        private readonly ISecurityAdapter _securityAdapter;

        #endregion

        #region C-Tor

        [ImportingConstructor]
        public AccountApiController(ISecurityAdapter securityAdapter)
        {
            this._securityAdapter = securityAdapter;
        }

        #endregion

        #region HttpPost

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody]AccountLoginModel model)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var success = this._securityAdapter.Login(model.LoginEmail, model.Password, model.RememberMe);
                return request.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
            });
        }

        [HttpPost]
        [Route("register")]
        public HttpResponseMessage CreateAccount(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponseMessage(request, () =>
            {
                HttpResponseMessage response = null;

                // revalidate all steps to ensure this operation is secure against hacks
                if (ValidateRegistrationStep1(request, accountModel).IsSuccessStatusCode &&
                    ValidateRegistrationStep2(request, accountModel).IsSuccessStatusCode &&
                    ValidateRegistrationStep3(request, accountModel).IsSuccessStatusCode)
                {
                    this._securityAdapter.Register(
                        accountModel.LoginEmail, 
                        accountModel.Password,
                        new
                        {
                            accountModel.FirstName,
                            accountModel.LastName,
                            accountModel.Age,
                            accountModel.City,
                            accountModel.CreditCard,
                            accountModel.ExpirationDate,
                            accountModel.Hausnumber,
                            accountModel.State,
                            accountModel.Street,
                            accountModel.ZipCode,
                            accountModel.IsActive
                        });
                    this._securityAdapter.Login(accountModel.LoginEmail, accountModel.Password, false);

                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("register/validate1")]
        public HttpResponseMessage ValidateRegistrationStep1(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var errors = new List<string>();

                var result = this.ValidateRegistrationStep1Model(accountModel);
                if (!result)
                {
                    errors.Add("Model is in invalid state.");
                }

                var states = UIHelper.GetStates();
                var state = states.FirstOrDefault(item => string.Equals(item.Abbrev, accountModel.State, StringComparison.CurrentCultureIgnoreCase));
                if (state == null)
                {
                    errors.Add("Invalid state.");
                }

                var matchZipCode = Regex.Match(accountModel.ZipCode, @"^\d{5}(?:[-\s]\d{4})?$");
                if (!matchZipCode.Success)
                {
                    errors.Add("Zip code is in an invalid format.");
                }

                var response = errors.Count == 0 ? 
                    request.CreateResponse(HttpStatusCode.OK) : 
                    request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());

                return response;
            });
        }

        [HttpPost]
        [Route("register/validate2")]
        public HttpResponseMessage ValidateRegistrationStep2(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var errors = new List<string>();

                var emailExist = this._securityAdapter.UserExists(accountModel.LoginEmail);
                if (emailExist)
                {
                    errors.Add("An account is already registered with this email address.");
                }

                var response = errors.Count == 0 ? 
                    request.CreateResponse(HttpStatusCode.OK) : 
                    request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());

                return response;
            });
        }

        [HttpPost]
        [Route("register/validate3")]
        public HttpResponseMessage ValidateRegistrationStep3(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponseMessage(request, () =>
            {
                var errors = new List<string>();

                var matchCreditCard = Regex.Match(accountModel.CreditCard, @"^\d{16}$", RegexOptions.IgnoreCase);
                if (!matchCreditCard.Success)
                {
                    errors.Add("Credit card number is in an invalid format.");
                }

                var matchExpDate = Regex.Match(accountModel.ExpirationDate, @"(0[1-9]|1[0-2])\/[0-9]{2}", RegexOptions.IgnoreCase);
                if (!matchExpDate.Success)
                {
                    errors.Add("Expiration date is invalid.");
                }

                var response = errors.Count == 0 ? 
                    request.CreateResponse(HttpStatusCode.OK) : 
                    request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());

                return response;
            });
        }

        [HttpPost]
        [Route("changepassword")]
        [Authorize]
        public HttpResponseMessage ChangePassword(HttpRequestMessage request, [FromBody]AccountChangePasswordModel passwordModel)
        {
            return GetHttpResponseMessage(request, () =>
            {
                ValidateAuthorizedUser(passwordModel.LoginEmail);

                var success = this._securityAdapter.ChangePassword(passwordModel.LoginEmail, passwordModel.OldPassword, passwordModel.NewPassword);
                var response = success ? 
                    request.CreateResponse(HttpStatusCode.OK) : 
                    request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to change password.");

                return response;
            });
        }

        #endregion

        #region Helpers

        private bool ValidateRegistrationStep1Model(AccountRegisterModel accountModel)
        {
            if (string.IsNullOrWhiteSpace(accountModel.FirstName) ||
                accountModel.FirstName.Length < 2 ||
                accountModel.FirstName.Length > 20)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(accountModel.LastName) ||
                accountModel.LastName.Length < 2 ||
                accountModel.LastName.Length > 20)
            {
                return false;
            }

            if (accountModel.Age < 18)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(accountModel.Street) ||
                accountModel.Street.Length < 2 ||
                accountModel.Street.Length > 50)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(accountModel.Hausnumber) ||
                accountModel.Hausnumber.Length > 10)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(accountModel.ZipCode))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(accountModel.City) ||
                accountModel.City.Length < 2 ||
                accountModel.City.Length > 20)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(accountModel.State))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
