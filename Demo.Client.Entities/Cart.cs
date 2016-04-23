using System;
using Core.Common.Core;
using FluentValidation;

namespace Demo.Client.Entities
{
    public class Cart : ObjectBase
    {
        #region Fields

        private int _cartId;
        private int _customerId;
        private int[] _cartItemId;
        private DateTime _created;
        private DateTime? _canceled;
        private DateTime? _approved;
        private DateTime? _shipped;
        private decimal _total;
        private decimal _shippingCost;
        private bool _stilOpen;

        #endregion

        #region Properties

        public int CartId
        {
            get { return this._cartId; }
            set
            {
                if (this._cartId == value) return;
                this._cartId = value;
                OnPropertyChanged(() => CartId);
            }
        }

        public int CustomerId
        {
            get { return this._customerId; }
            set
            {
                if (this._customerId == value) return;
                this._customerId = value;
                OnPropertyChanged(() => CustomerId);
            }
        }

        public int[] CartItemId
        {
            get { return this._cartItemId; }
            set
            {
                if (this._cartItemId == value) return;
                this._cartItemId = value;
                OnPropertyChanged(() => CartItemId);
            }
        }

        public decimal Total
        {
            get { return this._total; }
            set
            {
                if (this._total == value) return;
                this._total = value;
                OnPropertyChanged(() => this.Total);
            }
        }

        public decimal ShippingCost
        {
            get { return this._shippingCost; }
            set
            {
                if (this._shippingCost == value) return;
                this._shippingCost = value;
                OnPropertyChanged(() => this.ShippingCost);
            }
        }

        public bool StilOpen
        {
            get { return this._stilOpen; }
            set
            {
                if (this._stilOpen == value) return;
                this._stilOpen = value;
                OnPropertyChanged(() => this.StilOpen);
            }
        }

        public DateTime Created
        {
            get { return this._created; }
            set
            {
                if (this._created == value) return;
                this._created = value;
                OnPropertyChanged(() => Created);
            }
        }

        public DateTime? Canceled
        {
            get { return this._canceled; }
            set
            {
                if (this._canceled == value) return;
                this._canceled = value;
                OnPropertyChanged(() => Canceled);
            }
        }

        public DateTime? Approved
        {
            get { return this._approved; }
            set
            {
                if (this._approved == value) return;
                this._approved = value;
                OnPropertyChanged(() => Approved);
            }
        }

        public DateTime? Shipped
        {
            get { return this._shipped; }
            set
            {
                if (this._shipped == value) return;
                this._shipped = value;
                OnPropertyChanged(() => Shipped);
            }
        }

        #endregion

        #region Validation

        private class CartValidator : AbstractValidator<Cart>
        {
            public CartValidator()
            {
                RuleFor(person => person.CustomerId).NotEmpty();
                RuleFor(person => person.CartItemId).NotEmpty();
                RuleFor(person => person.Total).NotEmpty().GreaterThanOrEqualTo(new decimal(0.01));
                RuleFor(person => person.ShippingCost).NotEmpty();
                RuleFor(person => person.Created).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
            }
        }

        protected override IValidator GetValidator()
        {
            return new Cart.CartValidator();
        }

        #endregion
    }
}
