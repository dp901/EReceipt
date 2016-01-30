using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EReceipt.Models
{
    public class DeliveryInvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string Description { get; set; }
    }
}
