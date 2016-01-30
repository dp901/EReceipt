angular
    .module('app.controller.clientsController', [])
    .controller('clientsController', [
        '$scope', '$location', '$window', 'receiptsService', function($scope, $location, $window, receiptsService) {
        $scope.clients = [];
        $scope.clientSelectedRow = null;
        $scope.showClientForm = false;
        $scope.isNewClient = false;
        $scope.clientAlerts = [];

        $scope.getClients = function () {
            if ($scope.authentication.isAuth) {
                receiptsService.getClients().then(function(results) {
                    $scope.clients = results.data;
                }, function(error) {
                    alert(error.data.message);
                });
            }
        };

        $scope.getClients();

        $scope.createClient = function() {
            $scope.common.client = {};
            $scope.showClientForm = true;
            $scope.isNewClient = true;
        };

        $scope.multiPrintReceipts = function() {
            $scope.common.printMultiple = true;
            $location.path('/receipts');
        };

        $scope.addAlertItem = function () {
            var newItem = { description: "", name: 0, date: "" };
            if ($scope.common.client.alerts == null)
                $scope.common.client.alerts = [];
            $scope.common.client.alerts.push(newItem);
            console.log($scope.invoiceItems);
        };

        $scope.removeAlertItem = function (index) {
            $scope.common.client.alerts.splice(index, 1);
        };

        $scope.editClient = function() {
            if ($scope.common.client != null) {
                $scope.isNewClient = false;
                $scope.showClientForm = true;
            }
        };

        $scope.getAlerts = function () {
            if ($scope.authentication.isAuth) {
                receiptsService.getClientAlerts().then(function(result) {
                    $scope.clientAlerts = result.data;
                }, function(error) {
                    console.log(error.message);
                });
            }
        };

        $scope.getAlerts();

        $scope.saveClient = function () {
            if ($scope.common.client != null && $scope.common.client.defaultPrice != null)
                $scope.common.client.defaultPrice = $scope.common.client.defaultPrice.toString().replace(",", ".");
            if ($scope.common.client.indexNumber != null && $scope.common.client.indexNumber > 0) {
                if ($scope.isNewClient) {
                    receiptsService.createClient($scope.common.client).then(function(result) {
                        alert("Αποθήκευση Επιτυχής.");
                        $scope.getClients();
                    }, function(error) {
                        console.log(error.message);
                    });
                } else {
                    receiptsService.saveClient($scope.common.client).then(function(result) {
                        alert("Αποθήκευση Επιτυχής.");
                        $scope.showClientForm = false;
                        $scope.common.client = {};
                        $scope.getClients();
                    }, function(error) {
                        console.log(error.message);
                    });
                }
                $scope.showClientForm = false;
                $scope.getClients();
                $scope.common.client = null;
            } else {
                alert("Δεν έχει συμπληρώθει ο Άυξων Αριθμός. Παρακαλούμε εισάγετε μια τιμή για να προχωρήσετε παρακάτω.")
            }
        };

        $scope.deleteClient = function() {
            if ($scope.common.client != null) {
                receiptsService.deleteClient($scope.common.client.id).then(function(result) {
                    alert("Διαγραφή επιτυχης. Σημείωση: Ο πελάτης διαγράφεται μόνο αν ΔΕΝ υπάρχουν συνδεδεμένες αποδείξεις, τιμολόγια και δελτία αποστολής.");
                    $scope.getClients();
                }, function (error) {
                    console.log(error.message);
                });
            } else {
                alert("Δεν έχει επιλεγεί πελάτης προς διαγραφή.");
            }
        };

        $scope.cancel = function() {
            $scope.showClientForm = false;
            $scope.getClients();
            $scope.common.client = null;
            $scope.clientSelectedRow = null;
        };

        $scope.selectClient = function(row) {
            $scope.common.client = row;
            $scope.clientSelectedRow = row.id;
        };

        $scope.goToReceipts = function() {
            $scope.common.printMultiple = false;
            $location.path('/receipts');
        };
    }
]);