using System;
using System.Collections.Generic;
using EReceipt.Models;

namespace EReceipts.DAL
{
    public class DataAccessFake : IDataAccess
    {
        public Client GetClient(int clientId)
        {
            return new Client
            {
                Address = "Λιτοχώρου 6",
                AFM = "118983552",
                DOY = "ΣΤ θεσσαλονίκης",
                FirstName = "Βούλα",
                LastName = "Διονυσίου",
                Title = "Software engineer"
            };
        }

        public Receipt GetReceipt(int receiptId)
        {
            return new Receipt
            {
                //Amount = 100,
                ClientId = 1,
                Date = DateTime.Now,
                IndexNumber = 1,
            };
        }

        public Invoice GetInvoice(int invoiceId)
        {
            throw new NotImplementedException();
        }

        public DeliveryInvoice GetDeliveryInvoice(int deliveryInvoiceId)
        {
            throw new NotImplementedException();
        }

        public List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    Address = "Λιτοχώρου 6",
                    AFM = "118983552",
                    DOY = "ΣΤ θεσσαλονίκης",
                    FirstName = "Βούλα",
                    LastName = "Διονυσίου",
                    Id = 1,
                    Title = "Software engineer"
                }
            };
        }

        public List<Receipt> GetClientReceipts(int clientId)
        {
            return new List<Receipt>
            {
                new Receipt
                {
                    //Amount = 100,
                    ClientId = 1,
                    Date = DateTime.Now,
                    IndexNumber = 1,
                }
            };
        }

        public List<Invoice> GetClientInvoices(int clientId)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryInvoice> GetClientDeliveryInvoices(int clientId)
        {
            throw new NotImplementedException();
        }

        public void SaveClient(Client client)
        {
            throw new NotImplementedException();
        }

        public void InsertClient(Client client)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(int clientId)
        {
            throw new NotImplementedException();
        }

        public List<ClientReceipt> GetMultipleReceipts(int month, int year)
        {
            throw new NotImplementedException();
        }

        public List<ClientReceipt> SaveMultipleReceipts(int month, int year)
        {
            throw new NotImplementedException();
        }

        public void SaveReceipt(Receipt receipt, int receiptType)
        {
            throw new NotImplementedException();
        }

        public void CreateReceipt(Receipt receipt, int receiptType)
        {
            throw new NotImplementedException();
        }

        public List<Client> GetExpiredAlerts()
        {
            throw new NotImplementedException();
        }

        public List<Client> GetExpiringAlerts()
        {
            throw new NotImplementedException();
        }
    }
}
