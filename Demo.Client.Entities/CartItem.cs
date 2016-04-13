using System;
using Core.Common.Core;
using FluentValidation;

namespace Demo.Client.Entities
{
    public class CartItem : ObjectBase
    {
        #region Fields

        private int _cartItemId;
        private int _cartId;
        private int _productId;
        private int _quantity;

        #endregion

        #region Properties

        public int CartItemId
        {
            get { return this._cartItemId; }
            set
            {
                if (this._cartItemId == value) return;
                this._cartItemId = value;
                OnPropertyChanged(() => this.CartItemId);
            }
        }

        public int CartId
        {
            get { return this._cartId; }
            set
            {
                if (this._cartId == value) return;
                this._cartId = value;
                OnPropertyChanged(() => this.CartId);
            }
        }

        public int ProductId
        {
            get { return this._productId; }
            set
            {
                if (this._productId == value) return;
                this._productId = value;
                OnPropertyChanged(() => this.ProductId);
            }
        }

        public int Quantity
        {
            get { return this._quantity; }
            set
            {
                if (this._quantity == value) return;
                this._quantity = value;
                OnPropertyChanged(() => this.Quantity);
            }
        }
        #endregion

        #region Validation

        private class CartItemValidator : AbstractValidator<CartItem>
        {
            public CartItemValidator()
            {
                RuleFor(item => item.CartId).NotEmpty();
                RuleFor(item => item.ProductId).NotEmpty();
                RuleFor(item => item.Quantity).NotEmpty().GreaterThanOrEqualTo(1);
            }
        }

        protected override IValidator GetValidator()
        {
            return new CartItem.CartItemValidator();
        }

        #endregion
    }
}
