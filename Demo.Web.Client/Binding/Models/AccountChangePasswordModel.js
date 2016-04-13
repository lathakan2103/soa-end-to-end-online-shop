(function (os) {

    var AccountChangePasswordModel = function () {

        var self = this;

        self.LoginEmail = '';
        self.OldPassword = '';
        self.NewPassword = '';
    }

    os.AccountChangePasswordModel = AccountChangePasswordModel;

}(window.Demo));
