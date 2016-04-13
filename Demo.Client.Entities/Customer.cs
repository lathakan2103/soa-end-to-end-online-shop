using Core.Common.Core;
using FluentValidation;

namespace Demo.Client.Entities
{
    public class Customer : ObjectBase
    {
        #region Fields

        private int _customerId;
        private string _firstname;
        private string _lastname;
        private int _age;
        private string _loginEmail;
        private string _street;
        private string _hausnumber;
        private string _zipCode;
        private string _city;
        private string _state;
        private string _creditCard;
        private string _expirationDate;
        private bool _isActive;

        #endregion

        #region Properties

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

        public string FirstName
        {
            get { return this._firstname; }
            set
            {
                if (this._firstname == value) return;
                this._firstname = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get { return this._lastname; }
            set
            {
                if (this._lastname == value) return;
                this._lastname = value;
                OnPropertyChanged(() => LastName);
            }
        }

        public int Age
        {
            get { return this._age; }
            set
            {
                if (this._age == value) return;
                this._age = value;
                OnPropertyChanged(() => Age);
            }
        }

        public string LoginEmail
        {
            get { return this._loginEmail; }
            set
            {
                if (this._loginEmail == value) return;
                this._loginEmail = value;
                OnPropertyChanged(() => LoginEmail);
            }
        }

        public bool IsActive
        {
            get { return this._isActive; }
            set
            {
                if (this._isActive == value) return;
                this._isActive = value;
                OnPropertyChanged(() => this.IsActive);
            }
        }

        public string Street
        {
            get { return this._street; }
            set
            {
                if (this._street == value) return;
                this._street = value;
                OnPropertyChanged(() => this.Street);
            }
        }

        public string Hausnumber
        {
            get { return this._hausnumber; }
            set
            {
                if (this._hausnumber == value) return;
                this._hausnumber = value;
                OnPropertyChanged(() => this.Hausnumber);
            }
        }

        public string ZipCode
        {
            get { return this._zipCode; }
            set
            {
                if (this._zipCode == value) return;
                this._zipCode = value;
                OnPropertyChanged(() => this.ZipCode);
            }
        }

        public string City
        {
            get { return this._city; }
            set
            {
                if (this._city == value) return;
                this._city = value;
                OnPropertyChanged(() => this.City);
            }
        }

        public string State
        {
            get { return this._state; }
            set
            {
                if (this._state == value) return;
                this._state = value;
                OnPropertyChanged(() => this.State);
            }
        }

        public string CreditCard
        {
            get { return this._creditCard; }
            set
            {
                if (this._creditCard == value) return;
                this._creditCard = value;
                OnPropertyChanged(() => this.CreditCard);
            }
        }

        public string ExpirationDate
        {
            get { return this._expirationDate; }
            set
            { 
                if (this._expirationDate == value) return;
                this._expirationDate = value;
                OnPropertyChanged(() => this.ExpirationDate);
            }
        }

        #endregion

        #region Validation

        private class CustomerValidator : AbstractValidator<Customer>
        {
            public CustomerValidator()
            {
                RuleFor(person => person.FirstName).NotEmpty().Length(2, 20);
                RuleFor(person => person.LastName).NotEmpty().Length(2, 20);
                RuleFor(person => person.Age).NotEmpty().GreaterThanOrEqualTo(18);
                RuleFor(person => person.LoginEmail).NotEmpty().Length(5, 20);
                RuleFor(person => person.Street).NotEmpty().Length(2, 50);
                RuleFor(person => person.Hausnumber).NotEmpty().Length(1, 10);
                RuleFor(person => person.ZipCode).NotEmpty().Length(4, 6);
                RuleFor(person => person.City).NotEmpty().Length(2, 20);
                RuleFor(person => person.State).NotEmpty().Length(2, 20);
                RuleFor(person => person.CreditCard).NotEmpty().Length(16);
                RuleFor(person => person.ExpirationDate).NotEmpty();
            }
        }

        protected override IValidator GetValidator()
        {
            return new CustomerValidator();
        }

        #endregion
    }
}