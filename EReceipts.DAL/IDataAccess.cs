using System.Collections.Generic;
using EReceipt.Models;

namespace EReceipts.DAL
{
    public interface IDataAccess
    {
        Client GetClient(int clientId);
        Receipt GetReceipt(int receiptId);
        Invoice GetInvoice(int invoiceId);
        DeliveryInvoice GetDeliveryInvoice(int deliveryInvoiceId);
        List<Client> GetClients();
        List<Receipt> GetClientReceipts(int clientId);
        List<Invoice> GetClientInvoices(int clientId);
        List<DeliveryInvoice> GetClientDeliveryInvoices(int clientId);
        void SaveClient(Client client);
        void InsertClient(Client client);
        void DeleteClient(int clientId);
        List<ClientReceipt> GetMultipleReceipts(int month, int year);
        List<ClientReceipt> SaveMultipleReceipts(int month, int year);
        void SaveReceipt(Receipt receipt, int receiptType);
        void CreateReceipt(Receipt receipt, int receiptType);
        List<Client> GetExpiredAlerts();
        List<Client> GetExpiringAlerts();
    }
}
