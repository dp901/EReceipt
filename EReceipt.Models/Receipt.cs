using System;
using System.Collections.Generic;

namespace EReceipt.Models
{
    //This class includes fields from all 3 classes: Receipt, Invoice, Delivery Invoice. This is so that the Save/Create operations can exist only once for all three
    public class Receipt
    {
        public int ClientId { get; set; }
        public int IndexNumber { get; set; }
        public DateTime Date { get; set; }
        public double NetAmount { get; set; }
        public double VatAmount { get; set; }
        public double TotalAmount { get; set; }
        public int VatPercent { get; set; }
        public bool IsPrinted { get; set; }
        public string ReceiptDescription { get; set; }
        public int ReceiptMonth { get; set; }
        public int ReceiptType { get; set; }
        public string PlaceOfOrigin { get; set; }
        public string PlaceOfDelivery { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
        public List<DeliveryInvoiceItem> DeliveryInvoiceItems { get; set; }
    }
}
