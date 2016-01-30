namespace EReceipt.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public double Payment { get; set; }
        public double PaymentClientBehalf { get; set; }
        public string Description { get; set; }
    }
}
