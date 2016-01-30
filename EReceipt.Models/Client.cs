using System;
using System.Collections.Generic;

namespace EReceipt.Models
{
    public class Client
    {
        public int Id { get; set; }
        public int IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string AFM { get; set; }
        public string DOY { get; set; }
        public decimal DefaultPrice { get; set; }
        public string AdministrationOffice { get; set; }
        public bool January	{ get; set; }
        public bool February { get; set; }
        public bool March { get; set; }
        public bool April { get; set; }
        public bool May { get; set; }
        public bool June { get; set; }
        public bool July { get; set; }
        public bool August { get; set; }
        public bool September { get; set; }
        public bool October { get; set; }
        public bool November { get; set; }
        public bool December { get; set; }
        public List<Alert> Alerts { get; set; } 
    }
}
