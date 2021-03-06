﻿angular
    .module('app.service.authService', [])
    .factory('authService', [
        '$http',
        '$q',
        '$location',
        'localStorageService',
        'BASE_PATH',
        function ($http, $q, $location, localStorageService, BASE_PATH) {
            var authServiceFactory = {};

            var authentication = {
                isAuth: false,
                userName: ""
            };

            var saveRegistration = function (registration) {
                logOut();
                return $http.post(BASE_PATH + '/api/account/register', registration).then(function (response) {
                    return response;
                });

            };

            var login = function (loginData) {
                var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
                var deferred = $q.defer();
                $http.post(BASE_PATH + '/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
                    localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName });
                    authentication.isAuth = true;
                    authentication.userName = loginData.userName;
                    deferred.resolve(response);
                }).error(function (err, status) {
                    logOut();
                    deferred.reject(err);
                });

                return deferred.promise;
            };

            var logOut = function () {
                localStorageService.remove('authorizationData');
                authentication.isAuth = false;
                authentication.userName = "";
            };

            var fillAuthData = function () {
                var authData = localStorageService.get('authorizationData');
                if (authData) {
                    authentication.isAuth = true;
                    authentication.userName = authData.userName;
                }
            };

            authServiceFactory.saveRegistration = saveRegistration;
            authServiceFactory.login = login;
            authServiceFactory.logOut = logOut;
            authServiceFactory.fillAuthData = fillAuthData;
            authServiceFactory.authentication = authentication;

            return authServiceFactory;
        }
    ]);