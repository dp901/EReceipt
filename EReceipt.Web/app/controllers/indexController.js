angular
    .module('app.controller.indexController', [])
    .controller('indexController', [
        '$scope', '$location', 'authService', function($scope, $location, authService) {

    $scope.common = {
        client: null,
        printMultiple: false
    };

    $scope.logOut = function() {
        authService.logOut();
        $location.path('/home');
    };

    $scope.goToClients = function() {
        $location.path = "/clients";
    };

    $scope.authentication = authService.authentication;
        $scope.menubar = [
            {
                title: "Home",
                controller: "dashboard",
                action: "dashboard",
                cssClass: "fa fa-home fa-0",
                hasSubmenu: false,
                visible: true,
                inactivity: true
            },
            {
                title: "Πελάτες",
                controller: "clients",
                action: "clients",
                cssClass: "fa fa-group fa-0",
                hasSubmenu: false,
                visible: true,
                inactivity: true
            },
            {
                title: "Μαζικές Εκτυπώσεις",
                controller: "print",
                action: "print",
                cssClass: "fa fa-print fa-0",
                hasSubmenu: false,
                visible: true,
                inactivity: true
            }
        ];
    }
]);