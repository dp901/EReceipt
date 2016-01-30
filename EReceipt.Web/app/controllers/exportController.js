angular
    .module('app.controller.exportController', [])
    .controller('exportController', [
        '$scope', '$location', 'receiptsService', function ($scope, $location, receiptsService) {
            $scope.client = {};
            $scope.receipt = {};

            $scope.viewReceipt = function () {
                receiptsService.getReceipt($scope.receipt.id).then(function (results) {
                    $scope.receipt = results.data;
                });
            };

            $scope.$on('myEvent', function (event, args) {
                console.log($scope.receipt);
                //$scope.viewReceipt();
            });
        }
    ]);