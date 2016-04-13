
(function(os) {

    var AccountLoginModel = function () {

        var self = this;

        self.LoginEmail = '';
        self.Password = '';
        self.RememberMe = false;

    }

    os.AccountLoginModel = AccountLoginModel;

}(window.Demo));