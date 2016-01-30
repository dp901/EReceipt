using System;
using System.Collections.Generic;

namespace EReceipt.Models
{
    public class DeliveryInvoice
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
        public List<DeliveryInvoiceItem> DeliveryInvoiceItems { get; set; }
    }
}
