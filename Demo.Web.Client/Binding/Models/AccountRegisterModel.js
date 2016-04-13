
(function (os) {
    var AccountRegisterModelStep1 = function () {

        var self = this;

        self.FirstName = '';
        self.LastName = '';
        self.Age = '';
        self.Street = '';
        self.City = '';
        self.Hausnumber = '';
        self.State = '';
        self.Street = '';
        self.ZipCode = '';

        self.Initialized = false;
    }
    os.AccountRegisterModelStep1 = AccountRegisterModelStep1;
}(window.Demo));

(function (os) {
    var AccountRegisterModelStep2 = function () {

        var self = this;

        self.LoginEmail = '';
        self.Password = '';
        self.PasswordConfirm = '';
        self.IsActive = true;

        self.Initialized = false;
    }
    os.AccountRegisterModelStep2 = AccountRegisterModelStep2;
}(window.Demo));

(function (os) {
    var AccountRegisterModelStep3 = function () {

        var self = this;

        self.CreditCard = '';
        self.ExpirationDate = '';
        
        self.Initialized = false;
    }
    os.AccountRegisterModelStep3 = AccountRegisterModelStep3;
}(window.Demo));
