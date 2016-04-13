
appMainModule.controller("SettingsController", function ($scope, $http, controllerHelper, validator) {

    $scope.viewMode = ''; // settings, success
    $scope.settingsModel = null;
    $scope.controllerHelper = controllerHelper;

    var settingsModelRules = [];

    var setupRules = function () {
        settingsModelRules.push(new validator.PropertyRule("FirstName", {
            required: { message: "First name is required" }
        }));
        settingsModelRules.push(new validator.PropertyRule("LastName", {
            required: { message: "Last name is required" }
        }));
        settingsModelRules.push(new validator.PropertyRule("Street", {
            required: { message: "Street is required" }
        }));
        settingsModelRules.push(new validator.PropertyRule("Hausnumber", {
            required: { message: "Hausnumber is required" }
        }));
        settingsModelRules.push(new validator.PropertyRule("City", {
            required: { message: "City is required" }
        }));
        settingsModelRules.push(new validator.PropertyRule("State", {
            required: { message: "State is required" }
        }));
        settingsModelRules.push(new validator.PropertyRule("ZipCode", {
            required: { message: "Zip code is required" },
            pattern: { message: "Zip code is in invalid format", params: /^\d{5}$/ }
        }));
        settingsModelRules.push(new validator.PropertyRule("CreditCard", {
            required: { message: "Credit card is required" },
            pattern: { message: "Credit card is in invalid format", params: /^\d{16}$/ }
        }));
        settingsModelRules.push(new validator.PropertyRule("ExpirationDate", {
            required: { message: "Expiration date is required" },
            pattern: { message: "Expiration date is in invalid format", params: /^(0[1-9]|1[0-2])\/[0-9]{2}$/ }
        }));
    }

    $scope.initialize = function () {
        controllerHelper.apiGet('api/customer/settings', null,
            function (result) {
                $scope.settingsModel = result.data;
                $scope.viewMode = 'settings';
            });
    }
        
    $scope.save = function () {
        validator.ValidateModel($scope.settingsModel, settingsModelRules);
        controllerHelper.modelIsValid = $scope.settingsModel.isValid;
        controllerHelper.modelErrors = $scope.settingsModel.errors;
        if (controllerHelper.modelIsValid) {
            controllerHelper.apiPost('api/customer/settings', $scope.settingsModel,
                function(result) {
                    $scope.viewMode = 'success';
                });
        } else {
            controllerHelper.modelErrors = settingsModel.errors;
        }
    }

    var validationErrors = function () {
        var errors = [];
        for (var i = 0; i < propertyBag.length; i++) {
            if (propertyBag[i].Invalid) {
                errors.push(propertyBag[i].PropertyName);
            }
        }
        return errors;
    }

    $scope.validate = function (field, invalid) {
         for (var i = 0; i < propertyBag.length; i++)
         {
             if (propertyBag[i].PropertyName == field) {
                 propertyBag[i].Invalid = invalid;
                 break;
             }
         }
    }
    
    setupRules();
    $scope.initialize();

});
