using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading;
using Core.Common.Contracts;
using Core.Common.Core;
using Demo.Business.Entities;
using Demo.Common;

namespace Demo.Business.Managers
{
    public class ManagerBase
    {
        #region Fields

        protected readonly string LoginEmail;
        protected readonly Customer AuthorizationCustomer;

        #endregion

        #region C-Tor

        /// <summary>
        /// post construction resolving the dependencies
        /// </summary>
        protected ManagerBase()
        {
            var context = OperationContext.Current;
            if (context != null)
            {
                // get user email address from soap message header
                try
                {
                    this.LoginEmail = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("String", "System");

                    // if the user is coming from a desktop app
                    // it means we have windows authentication
                    if (this.LoginEmail.IndexOf(@"\", StringComparison.Ordinal) > 1)
                    {
                        this.LoginEmail = string.Empty;
                    }
                }
                catch
                {
                    this.LoginEmail = string.Empty;
                }
            }

            if (ObjectBase.Container != null)
            {
                ObjectBase.Container.SatisfyImportsOnce(this);
            }

            // if user comes from a website
            // check if email addresses belong the the same user
            // and therefore are equal too
            if (!string.IsNullOrWhiteSpace(this.LoginEmail))
            {
                this.AuthorizationCustomer = this.LoadAuthorizationValidationCustomer(this.LoginEmail);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// here will be validated if the email sent in and the email address of the logged in user
        /// are email addresses of the same person entity
        /// but the check will be done in the manager class
        /// </summary>
        /// <param name="loginEmail"></param>
        /// <returns></returns>
        protected virtual Customer LoadAuthorizationValidationCustomer(string loginEmail)
        {
            return null;
        }

        /// <summary>
        /// make the current user validation
        /// </summary>
        /// <param name="entity"></param>
        protected void ValidateAuthorization(ICustomerOwnedEntity entity)
        {
            // omit check if user is in admin role
            if (Thread.CurrentPrincipal.IsInRole(Security.DemoAdminRole)) return;

            // omit check if the user is windows desktop app user
            if (this.AuthorizationCustomer == null) return;

            // check if the incoming entity and owner entity are the same
            if (string.IsNullOrEmpty(this.LoginEmail) || entity.OwnerCustomerId == this.AuthorizationCustomer.CustomerId)
                return;

            var ex = new AuthorizationValidationException("Attempt to access a secure record with improper user authorization validation.");
            throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
        }

        /// <summary>
        /// centralized exception handling for service managers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected T ExecuteFaultHandledOperation<T>(Func<T> operation)
        {
            try
            {
                return operation.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        /// <summary>
        /// centralized exception handling for service managers
        /// </summary>
        /// <param name="operation"></param>
        protected void ExecuteFaultHandledOperation(Action operation)
        {
            try
            {
                operation.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion
    }
}
