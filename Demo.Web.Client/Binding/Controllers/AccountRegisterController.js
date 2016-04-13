var accountRegisterModule = angular.module('accountRegister', ['common'])
    .config(function ($routeProvider, $locationProvider) {
        $routeProvider.when(
            Demo.rootPath + 'account/register/step1',
            {
                templateUrl: Demo.rootPath + 'Templates/RegisterStep1.html', 
                controller: 'AccountRegisterStep1Controller'
            });
        $routeProvider.when(
            Demo.rootPath + 'account/register/step2',
            {
                 templateUrl: Demo.rootPath + 'Templates/RegisterStep2.html', 
                 controller: 'AccountRegisterStep2Controller'
            });
        $routeProvider.when(
            Demo.rootPath + 'account/register/step3',
            {
                 templateUrl: Demo.rootPath + 'Templates/RegisterStep3.html', 
                 controller: 'AccountRegisterStep3Controller'
            });
        $routeProvider.when(
            Demo.rootPath + 'account/register/confirm',
            {
                 templateUrl: Demo.rootPath + 'Templates/RegisterConfirm.html', 
                 controller: 'AccountRegisterConfirmController'
            });
        $routeProvider.otherwise(
            {
                 redirectTo: Demo.rootPath + 'account/register/step1'
            });

        $locationProvider.html5Mode(true);

    });

accountRegisterModule.controller('AccountRegisterController', function ($scope, $http, $location, $window, controllerHelper) {

    $scope.controllerHelper = controllerHelper;

    $scope.accountModelStep1 = new Demo.AccountRegisterModelStep1();
    $scope.accountModelStep2 = new Demo.AccountRegisterModelStep2();
    $scope.accountModelStep3 = new Demo.AccountRegisterModelStep3();

    $scope.previous = function () {
        $window.history.back();
    }
});

accountRegisterModule.controller('AccountRegisterStep1Controller', function ($scope, $http, $location, controllerHelper, validator) {

    controllerHelper.modelIsValid = true;
    controllerHelper.modelErrors = [];

    var accountModelStep1Rules = [];

    var setupRules = function () {
        accountModelStep1Rules.push(new validator.PropertyRule("FirstName", {
            required:       { message: "First name is required" },
            minLength:      { message: "First name must have at least 2 characters", params: 2 },
            maxLength:      { message: "First name have max 20 characters", params: 20 }
        }));
        accountModelStep1Rules.push(new validator.PropertyRule("LastName", {
            required:       { message: "Last name is required" },
            minLength:      { message: "Last name must have at least 2 characters", params: 2 },
            maxLength:      { message: "Last name have max 20 characters", params: 20 }
        }));
        accountModelStep1Rules.push(new validator.PropertyRule("Age", {
            required:       { message: "Age is required" },
            greaterThen:    { message: "You musst be at least 18 years old", params: 17 }
        }));
        accountModelStep1Rules.push(new validator.PropertyRule("Street", {
            required:       { message: "Street is required" },
            minLength:      { message: "Street must have at least 2 characters", params: 2 },
            maxLength:      { message: "Street must have max 50 characters", params: 50 }
        }));
        accountModelStep1Rules.push(new validator.PropertyRule("Hausnumber", {
            required:       { message: "Hausnumber is required" },
            maxLength:      { message: "Hausnumber must have max 10 characters", params: 10 }
        }));
        accountModelStep1Rules.push(new validator.PropertyRule("ZipCode", {
            required:       { message: "Zip code is required" },
            pattern:        { message: "Zip code is in invalid format", params: /^\d{5}$/ }
        }));
        accountModelStep1Rules.push(new validator.PropertyRule("City", {
            required:       { message: "City is required" },
            minLength:      { message: "City must have at least 2 characters", params: 2 },
            maxLength:      { message: "Street must have max 20 characters", params: 20 }
        }));
        accountModelStep1Rules.push(new validator.PropertyRule("State", {
            required:       { message: "State is required" },
            minLength:      { message: "State must have at least 2 characters", params: 2 },
            maxLength:      { message: "State must have max 20 characters", params: 20 }
        }));
    }

    $scope.step2 = function () {
        validator.ValidateModel($scope.accountModelStep1, accountModelStep1Rules);
        controllerHelper.modelIsValid = $scope.accountModelStep1.isValid;
        controllerHelper.modelErrors = $scope.accountModelStep1.errors;
        if (controllerHelper.modelIsValid) {
            controllerHelper.apiPost(
                'api/account/register/validate1',
                $scope.accountModelStep1,
                function(result) {
                    $scope.accountModelStep1.Initialized = true;
                    $location.path(Demo.rootPath + 'account/register/step2');
                });
        } else {
            controllerHelper.modelErrors = $scope.accountModelStep1.errors;
        }
    }

    setupRules();
});

accountRegisterModule.controller("AccountRegisterStep2Controller", function ($scope, $http, $location, controllerHelper, validator) {

    if (!$scope.accountModelStep1.Initialized) {
        // got to this controller before going through step 1
        $location.path(Demo.rootPath + 'account/register/step1');
    }

    controllerHelper.modelIsValid = true;
    controllerHelper.modelErrors = [];

    var accountModelStep2Rules = [];

    var setupRules = function () {
        accountModelStep2Rules.push(new validator.PropertyRule("LoginEmail", {
            required:   { message: "Login Email is required" },
            email:      { message: "Login email has wrong format" }
        }));
        accountModelStep2Rules.push(new validator.PropertyRule("Password", {
            required:   { message: "Password is required" },
            minLength:  { message: "Password must be at least 6 characters", params: 6 }
        }));
        accountModelStep2Rules.push(new validator.PropertyRule("PasswordConfirm", {
            required:   { message: "Password confirmation is required" },
            custom:     {
                validator: Demo.mustEqual,
                message: "Password do not match",
                params: function () { return $scope.accountModelStep2.Password; } // must be function so it can be obtained on-demand
            }
        }));
    }

    $scope.step3 = function () {
        validator.ValidateModel($scope.accountModelStep2, accountModelStep2Rules);
        controllerHelper.modelIsValid = $scope.accountModelStep2.isValid;
        controllerHelper.modelErrors = $scope.accountModelStep2.errors;
        if (controllerHelper.modelIsValid) {
            controllerHelper.apiPost(
                'api/account/register/validate2',
                $scope.accountModelStep2,
                function(result) {
                    $scope.accountModelStep2.Initialized = true;
                    $location.path(Demo.rootPath + 'account/register/step3');
                });
        } else {
            controllerHelper.modelErrors = $scope.accountModelStep2.errors;
        }
    }

    setupRules();
});

accountRegisterModule.controller("AccountRegisterStep3Controller", function ($scope, $http, $location, controllerHelper, validator) {

    if (!$scope.accountModelStep2.Initialized) {
        // got to this controller before going through step 2
        $location.path(Demo.rootPath + 'account/register/step2');
    }

    var accountModelStep3Rules = [];

    var setupRules = function () {
        accountModelStep3Rules.push(new validator.PropertyRule("CreditCard", {
            required:   { message: "Credit Card # is required" },
            pattern:    { message: "Credit card is in invalid format (16 digits)", params: /\d{16}$/ }
        }));
        accountModelStep3Rules.push(new validator.PropertyRule("ExpirationDate", {
            required:   { message: "Expiration Date is required" },
            pattern:    { message: "Expiration Date is in invalid format (MM/YY)", params: /^(0[1-9]|1[0-2])\/[0-9]{2}$/ }
        }));
    }

    $scope.confirm = function () {
        validator.ValidateModel($scope.accountModelStep3, accountModelStep3Rules);
        controllerHelper.modelIsValid = $scope.accountModelStep3.isValid;
        controllerHelper.modelErrors = $scope.accountModelStep3.errors;
        if (controllerHelper.modelIsValid) {
            controllerHelper.apiPost(
                'api/account/register/validate3',
                $scope.accountModelStep3,
                function(result) {
                    $scope.accountModelStep3.Initialized = true;
                    $location.path(Demo.rootPath + 'account/register/confirm');
                });
        } else {
            controllerHelper.modelErrors = $scope.accountModelStep3.errors;
        }
    }

    setupRules();
});

accountRegisterModule.controller("AccountRegisterConfirmController", function ($scope, $http, $location, $window, controllerHelper) {

    if (!$scope.accountModelStep3.Initialized) {
        // got to this controller before going through step 3
        $location.path(Demo.rootPath + 'account/register/step3');
    }

    $scope.createAccount = function () {

        var accountModel;

        accountModel = $.extend(accountModel, $scope.accountModelStep1);
        accountModel = $.extend(accountModel, $scope.accountModelStep2);
        accountModel = $.extend(accountModel, $scope.accountModelStep3);

        controllerHelper.apiPost(
            'api/account/register',
            accountModel,
            function (result) {
                $window.location.href = Demo.rootPath;
            });
    }
});
