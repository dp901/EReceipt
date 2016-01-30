angular
    .module('app.service.receiptsService', [])
    .factory('receiptsService', [
        '$http', 'BASE_PATH', function($http, BASE_PATH) {

            return {
                getClients: function() {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/getclients'
                    });
                },
                getClientReceipts: function(clientid) {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/getclientreceipts?clientId=' + clientid
                    });
                },
                getClientInvoices: function (clientid) {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/getclientinvoices?clientId=' + clientid
                    });
                },
                getClientAlerts: function () {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/getexpiredalerts'
                    });
                },
                getClientDeliveryInvoices: function (clientid) {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/getclientdeliveryinvoices?clientId=' + clientid
                    });
                },
                saveClient: function(client) {
                    return $http({
                        method: 'POST',
                        url: BASE_PATH + '/api/receipts/saveclient/',
                        data: client,
                        headers: { 'Content-Type': 'application/json' }
                    });
                },
                createClient: function (client) {
                    return $http({
                        method: 'POST',
                        url: BASE_PATH + '/api/receipts/insertclient/',
                        data: client,
                        headers: { 'Content-Type': 'application/json' }
                    });
                },
                deleteClient: function (clientId) {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/deleteClient?clientId=' + clientId
                    });
                },
                getMultipleReceipts: function(month, year) {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/getmultiplereceipts?month=' + month + '&year=' + year
                    });
                },
                printMultipleReceipts: function (month, year) {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/get?month=' + month + '&year=' + year
                    });
                },
                saveMultipleReceipts: function(month, year) {
                    return $http({
                        method: 'GET',
                        url: BASE_PATH + '/api/receipts/savemultiplereceipts?month=' + month + '&year=' + year
                    });
                },
                saveReceipt: function(receipt) {
                    return $http({
                        method: 'POST',
                        url: BASE_PATH + '/api/receipts/savereceipt/',
                        data: receipt,
                        headers: { 'Content-Type': 'application/json' }
                    });
                },
                createReceipt: function (receipt) {
                    return $http({
                        method: 'POST',
                        url: BASE_PATH + '/api/receipts/insertreceipt/',
                        data: receipt,
                        headers: { 'Content-Type': 'application/json' }
                    });
                }
            };

        }
    ]);