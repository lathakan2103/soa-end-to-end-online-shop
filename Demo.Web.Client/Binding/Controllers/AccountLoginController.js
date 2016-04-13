
appMainModule.controller("AccountLoginController", function ($scope, $http, $location, controllerHelper, validator) {

    $scope.controllerHelper = controllerHelper;
    $scope.accountModel = new Demo.AccountLoginModel();
    $scope.returnUrl = '';

    var accountModelRules = [];

    var setupRules = function () {

        accountModelRules.push(
            new validator.PropertyRule("LoginEmail", {
                required:   { message: "Login is required!" },
                minLength:  { message: "Login musst be at least 4 characters!", params: 4 },
                maxLength:  { message: "Login musst be max 20 characters!", params: 20 },
                pattern:    { message: "Login musst be in email format!", params: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/ }
        }));
        accountModelRules.push(
            new validator.PropertyRule("Password", {
                required:   { message: "Password is required!" },
                minLength:  { message: "Password musst be at least 4 characters!", params: 4 }
        }));

    };

    $scope.login = function () {
        
        // validate model
        validator.ValidateModel($scope.accountModel, accountModelRules);

        // show errors if validation gets them
        controllerHelper.modelIsValid = $scope.accountModel.isValid;
        controllerHelper.modelErrors = $scope.accountModel.errors;

        if (controllerHelper.modelIsValid) {

            controllerHelper.apiPost(
                'api/account/login',    // url
                $scope.accountModel,    // payload
                function (result) {     // success function
                    if ($scope.returnUrl.trim() != '' && $scope.returnUrl.trim().length > 1) {
                        window.location.href = Demo.rootPath + $scope.returnUrl.substring(1);
                    }
                    else {
                        window.location.href = Demo.rootPath;
                    }
                });

        }
        else {
            controllerHelper.modelErrors = $scope.accountModel.errors;
        }

    };

    setupRules();

});