angular
    .module('app', [
        'ngRoute',
        'LocalStorageModule',
        'app.controller.loginController',
        'app.controller.signupController',
        'app.controller.receiptsController',
        'app.controller.indexController',
        'app.controller.homeController',
        'app.controller.clientsController',
        'app.service.authService',
        'app.service.authInterceptorService',
        'app.service.receiptsService',
        'trNgGrid'
    ])
    .constant('BASE_PATH', 'http://localhost:1208')
    .config([
        '$routeProvider', function($routeProvider) {
            $routeProvider.when("/dashboard", {
                controller: "homeController",
                templateUrl: "/app/views/dashboard.html"
            });

            $routeProvider.when("/login", {
                controller: "loginController",
                templateUrl: "/app/views/login.html"
            });

            $routeProvider.when("/signup", {
                controller: "signupController",
                templateUrl: "/app/views/signup.html"
            });

            $routeProvider.when("/receipts", {
                controller: "receiptsController",
                templateUrl: "/app/views/receipts.html"
            });

            $routeProvider.when("/clients", {
                controller: "clientsController",
                templateUrl: "/app/views/clients.html"
            });

            $routeProvider.when("/print", {
                controller: "receiptsController",
                templateUrl: "/app/views/print.html"
            });

            $routeProvider.when("/reports", {
                controller: "reportsController",
                templateUrl: "/app/views/reports.html"
            });

            $routeProvider.otherwise({ redirectTo: "/dashboard" });
        }
    ])
    .run([
        'authService', function(authService) {
            authService.fillAuthData();
        }
    ]);