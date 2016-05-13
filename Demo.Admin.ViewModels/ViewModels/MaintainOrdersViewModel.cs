using Core.Common;
using Core.Common.Contracts;
using Core.Common.UI.Core;
using Demo.Client.Contracts;
using Demo.Client.Entities;
using Demo.Client.Proxies.Service_Procies;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Demo.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MaintainOrdersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IServiceFactory _serviceFactory;
        private ObservableCollection<Cart> _carts;
        private bool _isTest = false;

        #endregion

        #region Properties

        public ObservableCollection<Cart> Carts
        {
            get { return this._carts; }
            set
            {
                if (this._carts == value) return;
                this._carts = value;
                OnPropertyChanged(() => this.Carts);
            }
        }

        #endregion

        #region C-Tor

        [ImportingConstructor]
        public MaintainOrdersViewModel(IServiceFactory serviceFactory)
        {
            this._serviceFactory = serviceFactory;
            this.RegisterCommands();
        }

        public MaintainOrdersViewModel(IServiceFactory serviceFactory, bool isTest)
        {
            this._serviceFactory = serviceFactory;
            this.RegisterCommands();

            this._isTest = true;
        }

        #endregion

        #region Events

        public event EventHandler<ErrorMessageEventArgs> ErrorOccured;

        #endregion

        #region Commands

        public DelegateCommand<Cart> ApproveOrderCommand { get; private set; }
        public DelegateCommand<Cart> ShippOrderCommand { get; private set; }

        #endregion

        #region Overrides

        public override string ViewTitle
        {
            get
            {
                return "Orders";
            }
        }

        protected override void OnViewLoaded()
        {
            this._carts = new ObservableCollection<Cart>();

            var proxy = this._serviceFactory.CreateClient<IShoppingService>();
            WithClient(proxy, shoppingClient =>
            {
                this.SetCredentials(proxy);

                var carts = proxy.GetCarts();
                if (carts != null)
                {
                    foreach (var c in carts)
                    {
                        this._carts.Add(c);
                    }
                }
            });
        }

        #endregion

        #region Methods

        private void RegisterCommands()
        {
            ApproveOrderCommand = new DelegateCommand<Cart>(OnApproveOrderCommand);
            ShippOrderCommand = new DelegateCommand<Cart>(OnShippOrderCommand);
        }

        private void SetCredentials(IShoppingService shoppingService)
        {
            if (this._isTest) return;

            // Remove the ClientCredentials behavior. 
            var credentials = (shoppingService as ShoppingClient).ChannelFactory.Endpoint.Behaviors.Remove<ClientCredentials>();

            credentials.UserName.UserName = "pingo";
            credentials.UserName.Password = "07061971";

            // Add a custom client credentials instance to the behaviors collection. 
            (shoppingService as ShoppingClient).ChannelFactory.Endpoint.Behaviors.Add(credentials);
        }

        #endregion

        #region On...Command

        private void OnApproveOrderCommand(Cart cart)
        {
            try
            {
                var proxy = this._serviceFactory.CreateClient<IShoppingService>();
                WithClient(proxy, shoppingClient =>
                {
                    this.SetCredentials(proxy);

                    proxy.SetCartAsApproved(cart.CartId);
                    cart.Approved = proxy.GetCartByCartId(cart.CartId).Approved;
                });
            }
            catch (FaultException ex)
            {
                ErrorOccured?.Invoke(this, new ErrorMessageEventArgs(ex.Message));
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(this, new ErrorMessageEventArgs(ex.Message));
            }
        }

        private void OnShippOrderCommand(Cart cart)
        {
            try
            {
                var proxy = this._serviceFactory.CreateClient<IShoppingService>();
                WithClient(proxy, shoppingClient =>
                {
                    this.SetCredentials(proxy);

                    proxy.SetCartAsShipped(cart.CartId);
                    cart.Shipped = proxy.GetCartByCartId(cart.CartId).Shipped;
                });
            }
            catch (FaultException ex)
            {
                ErrorOccured?.Invoke(this, new ErrorMessageEventArgs(ex.Message));
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(this, new ErrorMessageEventArgs(ex.Message));
            }
        }

        #endregion
    }
}
