
appMainModule.controller("AccountChangePasswordController", function ($scope, $http, controllerHelper, validator) {

    $scope.controllerHelper = controllerHelper;
    $scope.passwordModel = new Demo.AccountChangePasswordModel();
    $scope.viewMode = 'changepassword'; // changepassword, success
    
    var passwordModelRules = [];

    var setupRules = function () {
        passwordModelRules.push(new validator.PropertyRule("OldPassword", {
            required:   { message: "Password is required" },
            minLength:  { message: "Password must be at least 6 characters", value: 6 }
        }));
        passwordModelRules.push(new validator.PropertyRule("NewPassword", {
            required:   { message: "New password is required" },
            minLength:  { message: "Old Password must be at least 6 characters", value: 6 }
        }));
    }

    $scope.changePassword = function () {
        validator.ValidateModel($scope.passwordModel, passwordModelRules);
        controllerHelper.modelIsValid = $scope.passwordModel.isValid;
        controllerHelper.modelErrors = $scope.passwordModel.errors;
        if (controllerHelper.modelIsValid) {
            controllerHelper.apiPost('api/account/changepassword', $scope.passwordModel,
                function(result) {
                    $scope.viewMode = 'success';
                });
        } else {
            controllerHelper.modelErrors = passwordModel.errors;
        }
    }

    setupRules();
});
