using Core.Common;
using Core.Common.Contracts;
using Core.Common.UI.Core;
using Demo.Client.Contracts;
using Demo.Client.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ServiceModel;

namespace Demo.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MaintainOrdersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IServiceFactory _serviceFactory;
        private ObservableCollection<Cart> _carts;

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

            WithClient(this._serviceFactory.CreateClient<IShoppingService>(), shoppingClient =>
            {
                var carts = shoppingClient.GetCarts();
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

        #region C-Tor

        [ImportingConstructor]
        public MaintainOrdersViewModel(IServiceFactory serviceFactory)
        {
            this._serviceFactory = serviceFactory;
            this.RegisterCommands();
        }

        #endregion

        #region Methods

        private void RegisterCommands()
        {
            ApproveOrderCommand = new DelegateCommand<Cart>(OnApproveOrderCommand);
            ShippOrderCommand = new DelegateCommand<Cart>(OnShippOrderCommand);
        }

        #endregion

        #region On...Command

        private void OnApproveOrderCommand(Cart cart)
        {
            try
            {
                WithClient(this._serviceFactory.CreateClient<IShoppingService>(), shoppingClient =>
                {
                    shoppingClient.SetCartAsApproved(cart.CartId);
                    cart.Approved = shoppingClient.GetCartByCartId(cart.CartId).Approved;
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
                WithClient(this._serviceFactory.CreateClient<IShoppingService>(), shoppingClient =>
                {
                    shoppingClient.SetCartAsShipped(cart.CartId);
                    cart.Shipped = shoppingClient.GetCartByCartId(cart.CartId).Shipped;
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
