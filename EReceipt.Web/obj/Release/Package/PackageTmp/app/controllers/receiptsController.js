angular
    .module('app.controller.receiptsController', [])
    .filter('receiptfilter', function () {
        return function (items, options) {
            var filtered = [];

            angular.forEach(items, function (item) {
                if (options.receipt == false && options.invoice == false && options.delivery == false) {
                    filtered.push(item);
                }
                if (options.receipt == true && item.receiptType == 1) {
                    filtered.push(item);
                }
                if (options.invoice == true && item.receiptType == 2) {
                    filtered.push(item);
                }
                if (options.delivery == true && item.receiptType == 3) {
                    filtered.push(item);
                }
            });

            return filtered;
        };
    })
    .controller('receiptsController', [
        '$scope', '$location', '$window', 'receiptsService', 'BASE_PATH', function ($scope, $location, $window, receiptsService, BASE_PATH) {
        $scope.receipts = [];
        $scope.invoices = [];
        $scope.deliveryInvoices = [];
        $scope.multiReceipts = [];
        $scope.receipt = null;
        $scope.showReceipts = false;
        $scope.showReceiptView = false;
        $scope.showReceiptForm = false;
        $scope.selectedMonth = null;
        $scope.selectedYear = null;
        $scope.canPrint = true;
        $scope.canSave = false;
        $scope.isNewReceipt = true;
        $scope.dataSource = null;
        $scope.rtype = 1;
        $scope.activePanel = "receipts";
        $scope.invoiceItems = [];
        $scope.deliveryInvoiceItems = [];
        $scope.receiptTotal = 0;
        $scope.isLoading = false;

        $scope.receiptType = {
            receipt: true,
            invoice: true,
            delivery: true
        };

        $scope.back = function() {
            $location.path('/clients');
        };

        $scope.selectReceipt = function(row) {
            $scope.receipt = row;
            $scope.invoiceItems = null;
            $scope.deliveryInvoiceItems = null;
            if ($scope.receipt.invoiceItems != null)
                $scope.invoiceItems = $scope.receipt.invoiceItems;
            if ($scope.receipt.deliveryInvoiceItems != null)
                $scope.deliveryInvoiceItems = $scope.receipt.deliveryInvoiceItems;

            console.log($scope.invoiceItems);
            $scope.receiptSelectedRow = row.indexNumber;
        };

        $scope.addDeliveryInvoiceItem = function() {
            var newItem = { description: "", unitPrice: 0, quantity: 0 };
            if ($scope.receipt.deliveryInvoiceItems == null)
                $scope.receipt.deliveryInvoiceItems = [];
            $scope.receipt.deliveryInvoiceItems.push(newItem);
            console.log($scope.deliveryInvoiceItems);
        };

        $scope.addInvoiceItem = function () {
            var newItem = { description: "", payment: 0, paymentClientBehalf: 0 };
            if ($scope.receipt.invoiceItems == null)
                $scope.receipt.invoiceItems = [];
            $scope.receipt.invoiceItems.push(newItem);
            console.log($scope.invoiceItems);
        };

        $scope.removeDeliveryInvoiceItem = function(index) {
            $scope.receipt.deliveryInvoiceItems.splice(index, 1);
        };

        $scope.removeInvoiceItem = function(index) {
            $scope.receipt.invoiceItems.splice(index, 1);
        };

        $scope.$watch('receiptTotal', function () {
            $scope.calculateAmounts();
        });

        $scope.$watch('receipt.vatPercent', function () {
            $scope.calculateAmounts();
        });

        $scope.$watch('receipt.netAmount', function () {
            $scope.calculateAmounts();
        });

        var roundToTwo = function(num) {
            return +(Math.round(num + "e+2") + "e-2");
        };

        $scope.calculateAmounts = function () {
            if ($scope.receipt != null) {
                if ($scope.rtype == 2 || $scope.rtype == 3) {
                    $scope.receipt.vatAmount = roundToTwo($scope.receipt.netAmount * ($scope.receipt.vatPercent / 100) );
                    $scope.receiptTotal = $scope.receipt.vatAmount + $scope.receipt.netAmount;
                } else {
                    $scope.receipt.netAmount = roundToTwo($scope.receiptTotal.replace(",",".") * 1 / (1 + ($scope.receipt.vatPercent / 100)));
                    $scope.receipt.vatAmount = roundToTwo($scope.receiptTotal.replace(",", ".") * 1 - $scope.receipt.netAmount);
                }
            }
        };

        $scope.$watch('receipt.invoiceItems', function () {
            if ($scope.receipt != null) {
                if ($scope.receipt.invoiceItems != null) {
                    var total = 0;
                    for (var i = 0; i < $scope.receipt.invoiceItems.length; i++) {
                        var item = $scope.receipt.invoiceItems[i];
                        total = 1 * total + (1 * item.paymentClientBehalf + 1 * item.payment);
                    }
                    //$scope.receiptTotal = total;
                    $scope.receipt.netAmount = total;
                }
            }
        }, true);

        $scope.$watch('receipt.deliveryInvoiceItems', function () {
            if ($scope.receipt != null) {
                if ($scope.receipt.deliveryInvoiceItems != null) {
                    var total = 0;
                    for (var i = 0; i < $scope.receipt.deliveryInvoiceItems.length; i++) {
                        var item = $scope.receipt.deliveryInvoiceItems[i];
                        total = 1 * total + (item.quantity * item.unitPrice);
                    }
                    //$scope.receiptTotal = total;
                    $scope.receipt.netAmount = total;
                }
            }
        }, true);

        $scope.setActivePanel = function (mode) {
            $scope.receipt = null;
            $scope.receiptSelectedRow = null;
            if (mode == 'receipts') {
                $scope.dataSource = $scope.receipts;
                $scope.rtype = 1;
            }
            if (mode == 'invoices') {
                $scope.dataSource = $scope.invoices;
                $scope.rtype = 2;
            }
            if (mode == 'delinvoices') {
                $scope.dataSource = $scope.deliveryInvoices;
                $scope.rtype = 3;
            }
        };

        $scope.viewReceipts = function () {
            if (!$scope.common.printMultiple && $scope.common.client != null) {
                $scope.showReceipts = true;
                receiptsService.getClientReceipts($scope.common.client.id).then(function (results) {
                    $scope.receipts = results.data;
                    $scope.dataSource = $scope.receipts;
                });
                receiptsService.getClientInvoices($scope.common.client.id).then(function (results) {
                    $scope.invoices = results.data;
                });
                receiptsService.getClientDeliveryInvoices($scope.common.client.id).then(function (results) {
                    $scope.deliveryInvoices = results.data;
                });
            }
        };

        $scope.viewReceipts();

        $scope.getMultipleReceipts = function () {
            $scope.canSave = false;
            receiptsService.getMultipleReceipts($scope.selectedMonth, $scope.selectedYear).then(function(results) {
                $scope.multiReceipts = results.data;
                for (var i = 0; i < $scope.multiReceipts.length; i++) {
                    var item = $scope.multiReceipts[i];
                    if (!item.receipt.isPrinted) {
                        $scope.canSave = true;
                        $scope.canPrint = false;
                        break;
                    }
                    $scope.canPrint = true;
                }
            });
        };

        $scope.saveMultipleReceipts = function () {
            $scope.canSave = false;
            $scope.isLoading = true;
            receiptsService.saveMultipleReceipts($scope.selectedMonth, $scope.selectedYear).then(function (results) {
                alert("Αποθήκευση Επιτυχής. Μπορείτε τώρα να εκτυπώσετε.");
                $scope.isLoading = false;
                $scope.getMultipleReceipts();
                $scope.canPrint = true;
            }, function (error) {
                $scope.canPrint = false;
                console.log(error);
            });
        };

        $scope.viewReceipt = function() {
            $scope.showReceiptView = true;
        };

        $scope.createReceipt = function () {
            $scope.receipt = {};
            $scope.isNewReceipt = true;
            $scope.showReceiptForm = true;
            $scope.receiptTotal = 0;
        };

        $scope.editReceipt = function () {
            $scope.isNewReceipt = false;
            $scope.showReceiptForm = true;
            $scope.receiptTotal = $scope.receipt.totalAmount;
        };

        $scope.saveReceipt = function () {
            $scope.receipt.clientId = $scope.common.client.id;
            $scope.receipt.totalAmount = $scope.receiptTotal;
            if ($scope.rtype == 3 && ($scope.receipt.placeOfOrigin == "" || $scope.receipt.placeOfDelivery == "" || $scope.receipt.placeOfOrigin == null || $scope.receipt.placeOfDelivery == null)) {
                alert("Παρακαλούμε συμπληρώστε τόπο αποστολής και προορισμού");
            } else {
                if ($scope.isNewReceipt) {
                    $scope.receipt.receiptType = $scope.rtype;
                    receiptsService.createReceipt($scope.receipt).then(function (results) {
                        alert("Η απόδειξη αποθηκεύτηκε.");
                    }, function (error) {
                        alert("Αποτυχία αποθήκευσης. Παρακαλούμε ελέγξτε τα στοιχεία του παραστατικού και προσπαθήστε ξανά.");
                    });
                } else {
                    receiptsService.saveReceipt($scope.receipt).then(function (results) {
                        alert("Η απόδειξη αποθηκεύτηκε.");
                    }, function (error) {
                        alert("Αποτυχία αποθήκευσης. Παρακαλούμε ελέγξτε τα στοιχεία του παραστατικού και προσπαθήστε ξανά.");
                    });
                }
            }
        };
        $scope.cancel = function() {
            $scope.showReceiptForm = false;
            $scope.viewReceipts();
            $scope.setActivePanel('receipts');
            $scope.activePanel = "receipts";
        };

        $scope.returnToClients = function () {
            $scope.common.printMultiple = false;
            $scope.common.client = null;
            $location.path("/clients");
        };

        $scope.printMultipleReceipts = function() {
            $window.open(BASE_PATH + "/api/receipts/get?month=" + $scope.selectedMonth + "&year=" + $scope.selectedYear);
            //receiptsService.printMultipleReceipts($scope.selectedMonth);
        };

        $scope.printReceipt = function() {
            if ($scope.rtype == 1) {
                $window.open(BASE_PATH + "/api/receipts/printReceipt?receiptId=" + $scope.receipt.indexNumber);
            } else if ($scope.rtype == 2) {
                $window.open(BASE_PATH + "/api/receipts/printInvoice?invoiceId=" + $scope.receipt.indexNumber);
            } else if ($scope.rtype == 3) {
                $window.open(BASE_PATH + "/api/receipts/printDeliveryInvoice?deliveryInvoiceId=" + $scope.receipt.indexNumber);
            }
        };

        //$scope.printReceipt = function() {
        //    var printContents = document.getElementById("receiptviewdiv").innerHTML;
        //    var originalContents = document.body.innerHTML;
        //    var popupWin = window.open('', '_blank', 'width=900,height=600');
        //    popupWin.document.open();
        //    popupWin.document.write('<html><head><link href="content/css/bootstrap.min.css" rel="stylesheet" /></head><body onload="window.print()">' + printContents + '</html>');
        //    popupWin.document.close();
        //};
    }
]);