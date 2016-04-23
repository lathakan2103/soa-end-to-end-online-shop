var buyModule = angular.module('buy', ['common'])
    .config(function($routeProvider, $locationProvider) {

        $routeProvider.when(
            Demo.rootPath + 'customer/buy',
            {
                templateUrl: Demo.rootPath + 'Templates/ProductSelection.html',
                controller: 'ProductListController'
            });
        $routeProvider.when(
            Demo.rootPath + 'customer/buy/product',
            {
                templateUrl: Demo.rootPath + 'Templates/Confirm.html',
                controller: 'ConfirmController'
            });
        $routeProvider.otherwise({ redirectTo: Demo.rootPath + 'customer/buy' });

        $locationProvider.html5Mode(true);

    });

buyModule.controller("BuyController", function ($scope, $http, $window, $location, controllerHelper, validator) {

    $scope.controllerHelper = controllerHelper;
    $scope.buyModel = new Demo.BuyModel();

    $scope.previous = function () {
        $window.history.back();
    }

    $scope.closeCart = function () {
        controllerHelper.apiPost(
            'api/shopping/closecart',
            null,
            function (result) {
                $location.path(Demo.rootPath + 'customer/buy');
            });
    };

});

buyModule.controller("ProductListController", function ($scope, $http, $window, $location, controllerHelper, validator) {

    controllerHelper.modelIsValid = true;
    controllerHelper.modelErrors = [];

    $scope.viewMode = 'list'; // list, detail
    $scope.products = [];
    $scope.selectedProduct = {};
    $scope.init = false;

    $scope.getAvailableProducts = function() {
        controllerHelper.apiGet(
            'api/shopping/products',
            null,
            function (result) {
                $scope.products = result.data;
                $scope.init = true;
                $scope.viewMode = 'list';
            });
    };

    $scope.selectProduct = function(product) {
        controllerHelper.apiGet(
            'api/shopping/product/' + product.ProductId,
            null,
            function(result) {
                $scope.selectedProduct = result.data;
                $scope.viewMode = 'detail';
            });
    };

    $scope.goToProductList = function() {
        $scope.viewMode = 'list';
    };

    $scope.confirm = function () {
        controllerHelper.apiPost(
            'api/shopping/buy/' + $scope.selectedProduct.ProductId,
            null,
            function(result) {
                $location.path(Demo.rootPath + 'customer/buy/product');
            });
    };

    $scope.getAvailableProducts();

});

buyModule.controller("ConfirmController", function ($scope, $http, $window, $location, controllerHelper, validator) {

    $scope.goToProductList = function () {
        $location.path(Demo.rootPath + 'customer/buy');
    };

    $scope.closeCart = function () {
        controllerHelper.apiPost(
            'api/shopping/closecart',
            null,
            function (result) {
                $location.path(Demo.rootPath + 'customer/buy');
            });
    };

});