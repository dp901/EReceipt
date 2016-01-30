using System;

namespace EReceipt.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
