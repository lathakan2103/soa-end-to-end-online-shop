appMainModule.controller("HistoryController", function ($scope, $http, $window, $location, controllerHelper) {

    $scope.controllerHelper = controllerHelper;
    $scope.HistoryModel = new Demo.HistoryModel();
    $scope.carts = [];
    $scope.items = [];
    $scope.init = false;
    $scope.viewMode = 'list'; // list, details

    var loadHistory = function () {
        $scope.carts = [];
        controllerHelper.apiGet(
            'api/shopping/history',
            null,
            function (result) {
                var cart = result.data;
                for (var i = 0; i < cart.CartList.length; i++) {
                    var c = cart.CartList[i];
                    $scope.carts.push(c);
                    $scope.viewMode = 'list'; 
                }
                $scope.init = true;
            });
    };

    $scope.showItems = function (cart) {
        controllerHelper.apiGet(
            'api/shopping/history/' + cart.CartId,
            null,
            function (result) {
                $scope.items = result.data;
                $scope.viewMode = 'details';
            });
    };

    $scope.cancelCart = function (cart) {

        if (!confirm('Do you want to cancel this cart withits items?')) {
            return;
        }
        controllerHelper.apiPost(
            'api/shopping/cancel/' + cart.CartId,
            null,
            function () {
                loadHistory();
            }
        );
    };

    $scope.goToCartsList = function () {
        $scope.viewMode = 'list';
    };

    loadHistory();

});