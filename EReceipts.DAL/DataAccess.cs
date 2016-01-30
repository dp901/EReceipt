using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using EReceipt.Models;

namespace EReceipts.DAL
{
    public class DataAccess : IDataAccess
    {
        private readonly string _connectionString;
        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Client GetClient(int clientId)
        {
            var client = new Client();
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                var sql = "Select * from Clients where Id=@cid";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cid", clientId);
                var rdr = cmd.ExecuteReader();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        client.AFM = rdr["AFM"].ToString();
                        client.DOY = rdr["DOY"].ToString();
                        client.Address = rdr["Address"].ToString();
                        client.FirstName = rdr["FirstName"].ToString();
                        client.LastName = rdr["LastName"].ToString();
                        client.Email = rdr["Email"].ToString();
                        client.Title = rdr["Title"].ToString();
                        client.DefaultPrice = Convert.ToDecimal(rdr["DefaultPrice"]);
                        client.January = Convert.ToBoolean(rdr["January"]);
                        client.February = Convert.ToBoolean(rdr["February"]);
                        client.March = Convert.ToBoolean(rdr["March"]);
                        client.April = Convert.ToBoolean(rdr["April"]);
                        client.May = Convert.ToBoolean(rdr["May"]);
                        client.June = Convert.ToBoolean(rdr["June"]);
                        client.July = Convert.ToBoolean(rdr["July"]);
                        client.August = Convert.ToBoolean(rdr["August"]);
                        client.September = Convert.ToBoolean(rdr["September"]);
                        client.October = Convert.ToBoolean(rdr["October"]);
                        client.November = Convert.ToBoolean(rdr["November"]);
                        client.December = Convert.ToBoolean(rdr["December"]);
                        client.Id = clientId;
                    }
                }

                const string sqlAlerts = "Select * from Alerts where ClientId=@cid";
                var cmdAlert = new SqlCommand(sqlAlerts, con);
                cmdAlert.Parameters.AddWithValue("@cid", clientId);
                var rdrAlert = cmdAlert.ExecuteReader();
                client.Alerts = new List<Alert>();
                using (rdrAlert)
                {
                    while (rdrAlert.Read())
                    {
                        var alert = new Alert
                        {
                            ClientId = clientId,
                            Date = Convert.ToDateTime(rdrAlert["Date"]),
                            Description = rdrAlert["Description"].ToString(),
                            Name = rdrAlert["Name"].ToString()
                        };
                        client.Alerts.Add(alert);
                    }
                }
            }
            return client;
        }

        public Receipt GetReceipt(int receiptId)
        {
            var receipt = new Receipt();
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                var sql = "Select * from Receipts where IndexNumber=@rid";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@rid", receiptId);
                var rdr = cmd.ExecuteReader();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        receipt.ClientId = Convert.ToInt32(rdr["ClientId"].ToString());
                        receipt.Date = Convert.ToDateTime(rdr["Date"].ToString());
                        receipt.IndexNumber = Convert.ToInt32(rdr["IndexNumber"]);
                        receipt.TotalAmount = Convert.ToDouble(rdr["TotalAmount"]);
                        receipt.NetAmount = Convert.ToDouble(rdr["NetAmount"]);
                        receipt.VatAmount = Convert.ToDouble(rdr["VatAmount"]);
                        receipt.VatPercent = Convert.ToInt32(rdr["VatPercent"]);
                        receipt.IsPrinted = Convert.ToBoolean(rdr["IsPrinted"]);
                        receipt.ReceiptDescription = rdr["ReceiptDescription"].ToString();
                    }
                }
            }
            return receipt;
        }

        public Invoice GetInvoice(int invoiceId)
        {
            var receipt = new Invoice();
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                var sql = "Select * from Invoices where IndexNumber=@rid";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@rid", invoiceId);
                var rdr = cmd.ExecuteReader();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        receipt.ClientId = Convert.ToInt32(rdr["ClientId"].ToString());
                        receipt.Date = Convert.ToDateTime(rdr["Date"].ToString());
                        receipt.IndexNumber = Convert.ToInt32(rdr["IndexNumber"]);
                        receipt.TotalAmount = Convert.ToDouble(rdr["TotalAmount"]);
                        receipt.NetAmount = Convert.ToDouble(rdr["NetAmount"]);
                        receipt.VatAmount = Convert.ToDouble(rdr["VatAmount"]);
                        receipt.VatPercent = Convert.ToInt32(rdr["VatPercent"]);
                        receipt.IsPrinted = Convert.ToBoolean(rdr["IsPrinted"]);
                        receipt.ReceiptDescription = rdr["ReceiptDescription"].ToString();
                    }
                }
                var sqlItems = "Select * from InvoiceItems where InvoiceId=@id";
                var cmdItems = new SqlCommand(sqlItems, con);
                cmdItems.Parameters.AddWithValue("@id", invoiceId);
                var rdrItems = cmdItems.ExecuteReader();
                var items = new List<InvoiceItem>();
                using (rdrItems)
                {
                    while (rdrItems.Read())
                    {
                        items.Add(new InvoiceItem
                        {
                            Description = rdrItems["Description"].ToString(),
                            Payment = Convert.ToDouble(rdrItems["Payment"]),
                            PaymentClientBehalf = Convert.ToDouble(rdrItems["PaymentClientBehalf"])
                        });
                    }
                }
                receipt.InvoiceItems = items;
            }
            return receipt;
        }

        public DeliveryInvoice GetDeliveryInvoice(int deliveryInvoiceId)
        {
            var receipt = new DeliveryInvoice();
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                var sql = "Select * from DeliveryInvoices where IndexNumber=@rid";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@rid", deliveryInvoiceId);
                var rdr = cmd.ExecuteReader();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        receipt.ClientId = Convert.ToInt32(rdr["ClientId"].ToString());
                        receipt.Date = Convert.ToDateTime(rdr["Date"].ToString());
                        receipt.IndexNumber = Convert.ToInt32(rdr["IndexNumber"]);
                        receipt.TotalAmount = Convert.ToDouble(rdr["TotalAmount"]);
                        receipt.NetAmount = Convert.ToDouble(rdr["NetAmount"]);
                        receipt.VatAmount = Convert.ToDouble(rdr["VatAmount"]);
                        receipt.VatPercent = Convert.ToInt32(rdr["VatPercent"]);
                        receipt.IsPrinted = Convert.ToBoolean(rdr["IsPrinted"]);
                        receipt.ReceiptDescription = rdr["ReceiptDescription"].ToString();
                    }
                }
                var sqlItems = "Select * from DeliveryInvoiceItems where DeliveryInvoiceId=@id";
                var cmdItems = new SqlCommand(sqlItems, con);
                cmdItems.Parameters.AddWithValue("@id", deliveryInvoiceId);
                var rdrItems = cmdItems.ExecuteReader();
                var items = new List<DeliveryInvoiceItem>();
                using (rdrItems)
                {
                    while (rdrItems.Read())
                    {
                        items.Add(new DeliveryInvoiceItem
                        {
                            Description = rdrItems["Description"].ToString(),
                            Quantity = Convert.ToInt32(rdrItems["Quantity"]),
                            UnitPrice = Convert.ToDouble(rdrItems["UnitPrice"])
                        });
                    }
                }
                receipt.DeliveryInvoiceItems = items;
            }
            return receipt;
        }

        public List<Client> GetClients()
        {
            var clients = new List<Client>();
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                var sql = "Select * from Clients order by IndexNumber";
                var cmd = new SqlCommand(sql, con);
                var rdr = cmd.ExecuteReader();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        var client = new Client
                        {
                            Id = Convert.ToInt32(rdr["Id"].ToString()),
                            IndexNumber = Convert.ToInt32(rdr["IndexNumber"].ToString()),
                            FirstName = rdr["FirstName"].ToString(),
                            LastName = rdr["LastName"].ToString(),
                            Address = rdr["Address"].ToString(),
                            AFM = rdr["AFM"].ToString(),
                            DOY = rdr["DOY"].ToString(),
                            Title = rdr["Title"].ToString(),
                            Email = rdr["Email"].ToString(),
                            DefaultPrice = Convert.ToDecimal(rdr["DefaultPrice"]),
                            AdministrationOffice = rdr["AdministrationOffice"].ToString(),
                            January = Convert.ToBoolean(rdr["January"]),
                            February = Convert.ToBoolean(rdr["February"]),
                            March = Convert.ToBoolean(rdr["March"]),
                            April = Convert.ToBoolean(rdr["April"]),
                            May = Convert.ToBoolean(rdr["May"]),
                            June = Convert.ToBoolean(rdr["June"]),
                            July = Convert.ToBoolean(rdr["July"]),
                            August = Convert.ToBoolean(rdr["August"]),
                            September = Convert.ToBoolean(rdr["September"]),
                            October = Convert.ToBoolean(rdr["October"]),
                            November = Convert.ToBoolean(rdr["November"]),
                            December = Convert.ToBoolean(rdr["December"])
                        };
                        clients.Add(client);
                    }
                }

                foreach (var client in clients)
                {
                    const string sqlAlerts = "Select * from Alerts where ClientId=@cid";
                    var cmdAlert = new SqlCommand(sqlAlerts, con);
                    cmdAlert.Parameters.AddWithValue("@cid", client.Id);
                    var rdrAlert = cmdAlert.ExecuteReader();
                    client.Alerts = new List<Alert>();
                    using (rdrAlert)
                    {
                        while (rdrAlert.Read())
                        {
                            var alert = new Alert
                            {
                                ClientId = client.Id,
                                Date = Convert.ToDateTime(rdrAlert["Date"]),
                                Description = rdrAlert["Description"].ToString(),
                                Name = rdrAlert["Name"].ToString()
                            };
                            client.Alerts.Add(alert);
                        }
                    }
                }
            }
            return clients;
        }

        public List<Invoice> GetClientInvoices(int clientId)
        {
            var con = new SqlConnection(_connectionString);
            var invoices = new List<Invoice>();
            using (con)
            {
                con.Open();
                const string sqlInvoices = "Select * from Invoices where ClientId=@cid";
                var cmdInvoices = new SqlCommand(sqlInvoices, con);
                cmdInvoices.Parameters.AddWithValue("cid", clientId);
                var rdrInvoices = cmdInvoices.ExecuteReader();
                using (rdrInvoices)
                {
                    while (rdrInvoices.Read())
                    {
                        var invoice = new Invoice
                        {
                            Date = Convert.ToDateTime(rdrInvoices["Date"]),
                            IndexNumber = Convert.ToInt32(rdrInvoices["IndexNumber"]),
                            TotalAmount = Convert.ToDouble(rdrInvoices["TotalAmount"]),
                            NetAmount = Convert.ToDouble(rdrInvoices["NetAmount"]),
                            VatAmount = Convert.ToDouble(rdrInvoices["VatAmount"]),
                            IsPrinted = Convert.ToBoolean(rdrInvoices["IsPrinted"]),
                            VatPercent = Convert.ToInt32(rdrInvoices["VatPercent"]),
                            ReceiptDescription = rdrInvoices["ReceiptDescription"].ToString(),
                            ReceiptType = 2
                        };
                        invoices.Add(invoice);
                    }
                }

                foreach (var inv in invoices)
                {
                    const string sqlSelectItems = "Select * from InvoiceItems where InvoiceId=@invoiceid";
                    var cmdSelectItems = new SqlCommand(sqlSelectItems, con);
                    cmdSelectItems.Parameters.AddWithValue("@invoiceid", inv.IndexNumber);
                    var rdrSelectItems = cmdSelectItems.ExecuteReader();
                    inv.InvoiceItems = new List<InvoiceItem>();
                    using (rdrSelectItems)
                    {
                        while (rdrSelectItems.Read())
                        {
                            var item = new InvoiceItem
                            {
                                Description = rdrSelectItems["Description"].ToString(),
                                Id = Convert.ToInt32(rdrSelectItems["Id"]),
                                InvoiceId = Convert.ToInt32(rdrSelectItems["InvoiceId"]),
                                Payment = Convert.ToInt32(rdrSelectItems["Payment"]),
                                PaymentClientBehalf = Convert.ToDouble(rdrSelectItems["PaymentClientBehalf"])
                            };
                            inv.InvoiceItems.Add(item);
                        }
                    }
                }
            }
            return invoices;
        }

        public List<DeliveryInvoice> GetClientDeliveryInvoices(int clientId)
        {
            var con = new SqlConnection(_connectionString);
            var delInvoices = new List<DeliveryInvoice>();
            using (con)
            {
                con.Open();
                const string sqlDelivery = "Select * from DeliveryInvoices where ClientId=@cid";
                var cmdDelivery = new SqlCommand(sqlDelivery, con);
                cmdDelivery.Parameters.AddWithValue("cid", clientId);
                var rdrDelivery = cmdDelivery.ExecuteReader();

                using (rdrDelivery)
                {
                    while (rdrDelivery.Read())
                    {
                        var receipt = new DeliveryInvoice
                        {
                            Date = Convert.ToDateTime(rdrDelivery["Date"]),
                            IndexNumber = Convert.ToInt32(rdrDelivery["IndexNumber"]),
                            TotalAmount = Convert.ToDouble(rdrDelivery["TotalAmount"]),
                            NetAmount = Convert.ToDouble(rdrDelivery["NetAmount"]),
                            VatAmount = Convert.ToDouble(rdrDelivery["VatAmount"]),
                            IsPrinted = Convert.ToBoolean(rdrDelivery["IsPrinted"]),
                            VatPercent = Convert.ToInt32(rdrDelivery["VatPercent"]),
                            ReceiptDescription = rdrDelivery["ReceiptDescription"].ToString(),
                            PlaceOfDelivery = rdrDelivery["PlaceOfDelivery"].ToString(),
                            PlaceOfOrigin = rdrDelivery["PlaceOfOrigin"].ToString(),
                            ReceiptType = 3
                        };
                        delInvoices.Add(receipt);
                    }
                }

                foreach (var inv in delInvoices)
                {
                    const string sqlSelectItems = "Select * from DeliveryInvoiceItems where DeliveryInvoiceId=@invoiceid";
                    var cmdSelectItems = new SqlCommand(sqlSelectItems, con);
                    cmdSelectItems.Parameters.AddWithValue("@invoiceid", inv.IndexNumber);
                    var rdrSelectItems = cmdSelectItems.ExecuteReader();
                    inv.DeliveryInvoiceItems = new List<DeliveryInvoiceItem>();
                    using (rdrSelectItems)
                    {
                        while (rdrSelectItems.Read())
                        {
                            var item = new DeliveryInvoiceItem
                            {
                                Description = rdrSelectItems["Description"].ToString(),
                                Id = Convert.ToInt32(rdrSelectItems["Id"]),
                                InvoiceId = Convert.ToInt32(rdrSelectItems["DeliveryInvoiceId"]),
                                Quantity = Convert.ToInt32(rdrSelectItems["Quantity"]),
                                UnitPrice = Convert.ToDouble(rdrSelectItems["UnitPrice"])
                            };
                            inv.DeliveryInvoiceItems.Add(item);
                        }
                    }
                }
            }
            return delInvoices;
        }

        public List<Receipt> GetClientReceipts(int clientId)
        {
            var receipts = new List<Receipt>();
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                const string sql = "Select * from Receipts where ClientId=@cid";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("cid", clientId);
                var rdr = cmd.ExecuteReader();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        var receipt = new Receipt
                        {
                            Date = Convert.ToDateTime(rdr["Date"]),
                            IndexNumber = Convert.ToInt32(rdr["IndexNumber"]),
                            TotalAmount = Convert.ToDouble(rdr["TotalAmount"]),
                            NetAmount = Convert.ToDouble(rdr["NetAmount"]),
                            VatAmount = Convert.ToDouble(rdr["VatAmount"]),
                            VatPercent = Convert.ToInt32(rdr["VatPercent"]),
                            IsPrinted = Convert.ToBoolean(rdr["IsPrinted"]),
                            ReceiptDescription = rdr["ReceiptDescription"].ToString(),
                            ReceiptType = 1
                        };
                        receipts.Add(receipt);
                    }
                }
            }

            return receipts;
        }

        public void InsertClient(Client client)
        {
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
               
                const string sql = "Insert Into Clients (IndexNumber, FirstName, LastName, Email, Address, AdministrationOffice, AFM, DOY, Title,DefaultPrice,January,February,March,April,May,June,July,August,September,October,November,December) " +
                                   "Values (@indexnumber, @fname, @lname, @email, @address, @adminoffice, @afm, @doy, @title, @defaultPrice, @jan, @feb, @mar, @apr, @may, @jun, @jul, @aug, @sep, @oct, @nov, @dec)";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@lname", client.LastName ?? "");
                cmd.Parameters.AddWithValue("@indexnumber", client.IndexNumber);
                cmd.Parameters.AddWithValue("@email", client.Email ?? "");
                cmd.Parameters.AddWithValue("@adminoffice", client.AdministrationOffice ?? "");
                cmd.Parameters.AddWithValue("@fname", client.FirstName ?? "");
                cmd.Parameters.AddWithValue("@address", client.Address ?? "");
                cmd.Parameters.AddWithValue("@afm", client.AFM ?? "");
                cmd.Parameters.AddWithValue("@doy", client.DOY ?? "");
                cmd.Parameters.AddWithValue("@title", client.Title ?? "");
                cmd.Parameters.AddWithValue("@defaultPrice", Convert.ToDecimal(client.DefaultPrice));
                cmd.Parameters.AddWithValue("@jan", client.January);
                cmd.Parameters.AddWithValue("@feb", client.February);
                cmd.Parameters.AddWithValue("@mar", client.March);
                cmd.Parameters.AddWithValue("@apr", client.April);
                cmd.Parameters.AddWithValue("@may", client.May);
                cmd.Parameters.AddWithValue("@jun", client.June);
                cmd.Parameters.AddWithValue("@jul", client.July);
                cmd.Parameters.AddWithValue("@aug", client.August);
                cmd.Parameters.AddWithValue("@sep", client.September);
                cmd.Parameters.AddWithValue("@oct", client.October);
                cmd.Parameters.AddWithValue("@nov", client.November);
                cmd.Parameters.AddWithValue("@dec", client.December);
                cmd.ExecuteNonQuery();

                const string sqlSelectIndex = "Select * from Clients where IndexNumber >= @indexnumber";
                var cmdSelectIndex = new SqlCommand(sqlSelectIndex, con);
                cmdSelectIndex.Parameters.AddWithValue("@indexnumber", client.IndexNumber);
                var rdr = cmdSelectIndex.ExecuteReader();
                var indexes = new Dictionary<int, int>();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        indexes.Add(Convert.ToInt32(rdr["Id"]), Convert.ToInt32(rdr["IndexNumber"]) + 1);   
                    }
                }

                const string strMaxIndexNumber = "SELECT MAX(Id) from Clients";
                var cmdMaxIndexNumber = new SqlCommand(strMaxIndexNumber, con);
                var maxIndex = cmdMaxIndexNumber.ExecuteScalar();
                client.Id = Convert.ToInt32(maxIndex);
                foreach (var key in indexes.Keys)
                {
                    if (key != Convert.ToInt32(maxIndex))
                    {
                        const string sqlUpdateIndex = "Update Clients set IndexNumber=@indexnumbernew where Id = @id";
                        var cmdUpdateIndex = new SqlCommand(sqlUpdateIndex, con);
                        cmdUpdateIndex.Parameters.AddWithValue("@id", key);
                        cmdUpdateIndex.Parameters.AddWithValue("@indexnumbernew", indexes[key]);
                        cmdUpdateIndex.ExecuteNonQuery();
                    }
                }
            }

            SaveClientAlerts(client);
        }

        public void SaveClient(Client client)
        {
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                var sql = @"Update Clients SET FirstName = @fname
                      ,LastName = @lname
                      ,Email = @email
                      ,Address = @address
                      ,AFM = @afm
                      ,DOY = @doy
                      ,AdministrationOffice=@adminOffice
                      ,Title = @title
                      ,DefaultPrice = @price
                      ,January = @jan
                      ,February = @feb
                      ,March = @mar
                      ,April = @apr
                      ,May = @may
                      ,June = @jun
                      ,July = @jul
                      ,August = @aug
                      ,September = @sep
                      ,October = @oct
                      ,November = @nov
                      ,December = @dec WHERE Id=@id";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", client.Id);
                cmd.Parameters.AddWithValue("@lname", client.LastName ?? "");
                cmd.Parameters.AddWithValue("@fname", client.FirstName ?? "");
                cmd.Parameters.AddWithValue("@email", client.Email ?? "");
                cmd.Parameters.AddWithValue("@adminOffice", client.AdministrationOffice ?? "");
                cmd.Parameters.AddWithValue("@address", client.Address ?? "");
                cmd.Parameters.AddWithValue("@afm", client.AFM ?? "");
                cmd.Parameters.AddWithValue("@doy", client.DOY ?? "");
                cmd.Parameters.AddWithValue("@title", client.Title ?? "");
                cmd.Parameters.AddWithValue("@price", client.DefaultPrice);
                cmd.Parameters.AddWithValue("@jan", client.January);
                cmd.Parameters.AddWithValue("@feb", client.February);
                cmd.Parameters.AddWithValue("@mar", client.March);
                cmd.Parameters.AddWithValue("@apr", client.April);
                cmd.Parameters.AddWithValue("@may", client.May);
                cmd.Parameters.AddWithValue("@jun", client.June);
                cmd.Parameters.AddWithValue("@jul", client.July);
                cmd.Parameters.AddWithValue("@aug", client.August);
                cmd.Parameters.AddWithValue("@sep", client.September);
                cmd.Parameters.AddWithValue("@oct", client.October);
                cmd.Parameters.AddWithValue("@nov", client.November);
                cmd.Parameters.AddWithValue("@dec", client.December);

                cmd.ExecuteNonQuery();
            }
            SaveClientAlerts(client);
        }

        public void DeleteClient(int clientId)
        {
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                const string selectReceipts = "Select COUNT(*) from Receipts where ClientId=@cid";
                const string selectInvoices = "Select COUNT(*) from Invoices where ClientId=@cid";
                const string selectDeliveryInvoices = "Select COUNT(*) from DeliveryInvoices where ClientId=@cid";

                var cmdReceipts = new SqlCommand(selectReceipts, con);
                cmdReceipts.Parameters.AddWithValue("cid", clientId);
                var cmdInvoices = new SqlCommand(selectInvoices, con);
                cmdInvoices.Parameters.AddWithValue("cid", clientId);
                var cmdDeliveryInvoices = new SqlCommand(selectDeliveryInvoices, con);
                cmdDeliveryInvoices.Parameters.AddWithValue("cid", clientId);

                var receiptsNum = cmdReceipts.ExecuteScalar();
                var invoicesNum = cmdInvoices.ExecuteScalar();
                var delInvoicesNum = cmdDeliveryInvoices.ExecuteScalar();

                if (Convert.ToInt32(receiptsNum) == 0 && Convert.ToInt32(invoicesNum) == 0 && Convert.ToInt32(delInvoicesNum) == 0)
                {
                    const string deleteClient = "Delete from Clients where Id=@id";
                    var cmd = new SqlCommand(deleteClient, con);
                    cmd.Parameters.AddWithValue("id", clientId);
                    cmd.ExecuteNonQuery();
                    ReIndexClients(con);
                }
            }
        }

        private void ReIndexClients(SqlConnection con)
        {
            const string strReindex = @"UPDATE Clients
                                        SET Clients.IndexNumber = Clients.New_IndexNumber
                                        FROM (
                                              SELECT IndexNumber, ROW_NUMBER() OVER (ORDER BY [IndexNumber]) AS New_IndexNumber
                                              FROM Clients
                                              ) Clients";
            var cmd = new SqlCommand(strReindex, con);
            cmd.ExecuteNonQuery();
        }

        private void SaveClientAlerts(Client client)
        {
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                const string delete = "Delete from Alerts where ClientId=@cid";
                var cmdDelete = new SqlCommand(delete, con);
                cmdDelete.Parameters.AddWithValue("@cid", client.Id);
                cmdDelete.ExecuteNonQuery();

                const string sql = "Insert into Alerts (ClientId, Name, Description, Date) Values (@cid, @name, @description, @date)";

                if (client.Alerts != null)
                {
                    foreach (var it in client.Alerts)
                    {
                        var cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@cid", client.Id);
                        cmd.Parameters.AddWithValue("@name", it.Name ?? "");
                        cmd.Parameters.AddWithValue("@description", it.Description ?? "");
                        cmd.Parameters.AddWithValue("@date", it.Date);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<ClientReceipt> GetMultipleReceipts(int month, int year)
        {
            var receipts = new List<ClientReceipt>();
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                string filter = "";
                switch (month)
                {
                    case 1:
                        filter = "January=1";
                        break;
                    case 2:
                        filter = "February=1";
                        break;
                    case 3:
                        filter = "March=1";
                        break;
                    case 4:
                        filter = "April=1";
                        break;
                    case 5:
                        filter = "May=1";
                        break;
                    case 6:
                        filter = "June=1";
                        break;
                    case 7:
                        filter = "July=1";
                        break;
                    case 8:
                        filter = "August=1";
                        break;
                    case 9:
                        filter = "September=1";
                        break;
                    case 10:
                        filter = "October=1";
                        break;
                    case 11:
                        filter = "November=1";
                        break;
                    case 12:
                        filter = "December=1";
                        break;
                }
                const string strMaxIndexNumber = "SELECT MAX(IndexNumber) from Receipts";
                var cmdMaxIndexNumber = new SqlCommand(strMaxIndexNumber, con);
                var maxIndex = cmdMaxIndexNumber.ExecuteScalar();
                int indexNumber = maxIndex == null || maxIndex == DBNull.Value ? 1 : Convert.ToInt32(maxIndex);

                var sql = string.Format("Select * from Clients where {0} order by IndexNumber", filter);
                var cmd = new SqlCommand(sql, con);
                var rdr = cmd.ExecuteReader();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        var item = new ClientReceipt
                        {
                            Client = new Client
                            {
                                Id = Convert.ToInt32(rdr["Id"].ToString()),
                                IndexNumber = Convert.ToInt32(rdr["IndexNumber"]),
                                FirstName = rdr["FirstName"].ToString(),
                                LastName = rdr["LastName"].ToString(),
                                Email = rdr["Email"].ToString(),
                                Address = rdr["Address"].ToString(),
                                AFM = rdr["AFM"].ToString(),
                                DOY = rdr["DOY"].ToString(),
                                Title = rdr["Title"].ToString(),
                                AdministrationOffice = rdr["AdministrationOffice"].ToString(),
                                DefaultPrice = Convert.ToDecimal(rdr["DefaultPrice"]),
                                January = Convert.ToBoolean(rdr["January"]),
                                February = Convert.ToBoolean(rdr["February"]),
                                March = Convert.ToBoolean(rdr["March"]),
                                April = Convert.ToBoolean(rdr["April"]),
                                May = Convert.ToBoolean(rdr["May"]),
                                June = Convert.ToBoolean(rdr["June"]),
                                July = Convert.ToBoolean(rdr["July"]),
                                August = Convert.ToBoolean(rdr["August"]),
                                September = Convert.ToBoolean(rdr["September"]),
                                October = Convert.ToBoolean(rdr["October"]),
                                November = Convert.ToBoolean(rdr["November"]),
                                December = Convert.ToBoolean(rdr["December"])
                            }
                        };
                        receipts.Add(item);
                    }
                }

                foreach (var r in receipts)
                {
                    string sqlGetPrinted = "Select count(*) from Receipts where ClientId=@cid and ReceiptMonth=@month and IsPrinted=1";
                    var cmdCountPrinted = new SqlCommand(sqlGetPrinted, con);
                    cmdCountPrinted.Parameters.AddWithValue("@cid", r.Client.Id);
                    cmdCountPrinted.Parameters.AddWithValue("@month", Convert.ToInt32(string.Format("{0}{1}", month, year)));
                    object count = cmdCountPrinted.ExecuteScalar();
                    if (Convert.ToInt32(count) > 0)
                    {
                        sqlGetPrinted = "Select * from Receipts where ClientId=@cid and ReceiptMonth=@month and IsPrinted=1";
                        var cmdGetPrinted = new SqlCommand(sqlGetPrinted, con);
                        cmdGetPrinted.Parameters.AddWithValue("@cid", r.Client.Id);
                        cmdGetPrinted.Parameters.AddWithValue("@month", Convert.ToInt32(string.Format("{0}{1}", month, year)));
                        var rdrGetPrinted = cmdGetPrinted.ExecuteReader();
                        using (rdrGetPrinted)
                        {
                            while (rdrGetPrinted.Read())
                            {
                                r.Receipt = new Receipt
                                {
                                    TotalAmount = Convert.ToDouble(rdrGetPrinted["TotalAmount"]),
                                    NetAmount = Convert.ToDouble(rdrGetPrinted["NetAmount"]),
                                    VatAmount = Convert.ToDouble(rdrGetPrinted["VatAmount"]),
                                    VatPercent = Convert.ToInt32(rdrGetPrinted["VatPercent"]),
                                    IsPrinted = true,
                                    ClientId = r.Client.Id,
                                    Date = Convert.ToDateTime(rdrGetPrinted["Date"]),
                                    IndexNumber = Convert.ToInt32(rdrGetPrinted["IndexNumber"]),
                                    ReceiptDescription = rdrGetPrinted["ReceiptDescription"].ToString()
                                };
                            }
                        }
                    }
                    else
                    {
                        indexNumber++;
                        var price = Convert.ToDecimal(r.Client.DefaultPrice);
                        int vatPercent = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultVatPercent"]);
                        var netamount = price/(1 + (vatPercent/100));
                        var vat = price - netamount;

                        r.Receipt = new Receipt
                        {
                            TotalAmount = Convert.ToDouble(r.Client.DefaultPrice),
                            VatAmount = Convert.ToDouble(vat),
                            NetAmount = Convert.ToDouble(netamount),
                            VatPercent = vatPercent,
                            IsPrinted = false,
                            ClientId = r.Client.Id,
                            Date = DateTime.Now,
                            IndexNumber = indexNumber,
                            ReceiptDescription = "Συντήρηση Μηνός " + Enum.GetName(typeof(Enums.Months), month)
                        };
                    }
                    
                }
            }
            return receipts;
        }

        public List<ClientReceipt> SaveMultipleReceipts(int month, int year)
        {
            var receipts = GetMultipleReceipts(month, year);
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                //Always reseed the identity to ensure continuous invoice numbers.
                const string strMaxIndex = "SELECT MAX(IndexNumber) from Receipts";
                var cmdMaxIndex = new SqlCommand(strMaxIndex, con);
                var max = cmdMaxIndex.ExecuteScalar();

                string sqlIdentity = "DBCC CHECKIDENT ( Receipts, RESEED, " + Convert.ToInt32(max) + " )";
                var cmdIdentity = new SqlCommand(sqlIdentity, con);
                cmdIdentity.ExecuteNonQuery();

                const string sql = "Insert into Receipts (ClientId, Date, NetAmount, VatAmount, TotalAmount, VatPercent, IsPrinted, ReceiptDescription, ReceiptMonth) Values (@cid, @date, @netamount, @vatamount, @totalamount, @vatpercent, @isprinted, @descr, @month)";
                foreach (var r in receipts)
                {
                    if (!r.Receipt.IsPrinted)
                    {
                        r.Receipt.NetAmount = Math.Round(r.Receipt.TotalAmount / (1 + (Convert.ToDouble(r.Receipt.VatPercent) / 100)), 2);
                        r.Receipt.VatAmount = Math.Round(r.Receipt.TotalAmount - r.Receipt.NetAmount, 2);
                        var cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@cid", r.Client.Id);
                        cmd.Parameters.AddWithValue("@date", r.Receipt.Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        cmd.Parameters.AddWithValue("@totalamount", r.Receipt.TotalAmount);
                        cmd.Parameters.AddWithValue("@netamount", r.Receipt.NetAmount);
                        cmd.Parameters.AddWithValue("@vatpercent", r.Receipt.VatPercent);
                        cmd.Parameters.AddWithValue("@vatAmount", r.Receipt.VatAmount);
                        cmd.Parameters.AddWithValue("@isprinted", "1");
                        cmd.Parameters.AddWithValue("@descr", "Συντήρηση Μηνός " + Enum.GetName(typeof(Enums.Months), month));
                        cmd.Parameters.AddWithValue("@month", Convert.ToInt32(string.Format("{0}{1}", month, year)));
                        cmd.ExecuteNonQuery();
                        const string sqlSelectIdentity = "select @@IDENTITY";
                        var cmdSelectIdentity = new SqlCommand(sqlSelectIdentity, con);
                        var identity = cmdSelectIdentity.ExecuteScalar();
                        r.Receipt.IndexNumber = Convert.ToInt32(identity);
                    }
                }
            }
            return receipts;
        }

        public void SaveReceipt(Receipt receipt, int receiptType)
        {
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                string table = "";
                switch (receiptType)
                {
                    case 1:
                        table = "Receipts";
                        break;
                    case 2:
                        table = "Invoices";
                        break;
                    case 3:
                        table = "DeliveryInvoices";
                        break;
                }
                string sql = "Update " + table + " set ClientId = @cid, Date = @date, TotalAmount = @totalamount, NetAmount=@netamount, VatAmount=@vatAmount, VatPercent=@vatPercent, IsPrinted = @isprinted, ReceiptDescription = @descr where IndexNumber=@id";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", receipt.IndexNumber);
                cmd.Parameters.AddWithValue("@cid", receipt.ClientId);
                cmd.Parameters.AddWithValue("@date", receipt.Date.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@totalamount", receipt.TotalAmount);
                cmd.Parameters.AddWithValue("@netamount", receipt.NetAmount);
                cmd.Parameters.AddWithValue("@vatAmount", receipt.VatAmount);
                cmd.Parameters.AddWithValue("@vatPercent", receipt.VatPercent);
                cmd.Parameters.AddWithValue("@isprinted", "1");
                cmd.Parameters.AddWithValue("@descr", receipt.ReceiptDescription ?? "");
                cmd.ExecuteNonQuery();

                if (receiptType == 3)
                {
                    const string updateDeliveryInvoices = "Update DeliveryInvoices set PlaceOfOrigin=@origin, PlaceofDelivery=@delivery where IndexNumber=@id";
                    var cmdUpdateDel = new SqlCommand(updateDeliveryInvoices, con);
                    cmdUpdateDel.Parameters.AddWithValue("@id", receipt.IndexNumber);
                    cmdUpdateDel.Parameters.AddWithValue("@origin", receipt.PlaceOfOrigin ?? "");
                    cmdUpdateDel.Parameters.AddWithValue("@delivery", receipt.PlaceOfDelivery ?? "");
                    cmdUpdateDel.ExecuteNonQuery();

                    SaveDeliveryInvoiceItems(receipt.DeliveryInvoiceItems, receipt.IndexNumber);
                }

                if (receiptType == 2)
                {
                    SaveInvoiceItems(receipt.InvoiceItems, receipt.IndexNumber);
                }
            }
        }

        public void CreateReceipt(Receipt receipt, int receiptType)
        {
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                string table = "";
                switch (receiptType)
                {
                    case 1:
                        table = "Receipts";
                        break;
                    case 2:
                        table = "Invoices";
                        break;
                    case 3:
                        table = "DeliveryInvoices";
                        break;
                }

                //Always reseed the identity to ensure continuous invoice numbers.
                string strMaxIndex = "SELECT MAX(IndexNumber) from " + table;
                var cmdMaxIndex = new SqlCommand(strMaxIndex, con);
                var max = cmdMaxIndex.ExecuteScalar();

                string sqlIdentity = "DBCC CHECKIDENT ( " + table + ", RESEED, " + Convert.ToInt32(max) + " )";
                var cmdIdentity = new SqlCommand(sqlIdentity, con);
                cmdIdentity.ExecuteNonQuery();

                string sql = "Insert into " + table + " (ClientId, Date, TotalAmount, NetAmount, VatAmount, VatPercent, IsPrinted, ReceiptDescription) Values (@cid, @date, @totalamount, @netamount, @vatamount, @vatPercent, @isprinted, @descr)";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cid", receipt.ClientId);
                cmd.Parameters.AddWithValue("@date", DateTime.MinValue == receipt.Date ? DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") : receipt.Date.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@totalamount", receipt.TotalAmount);
                cmd.Parameters.AddWithValue("@netamount", receipt.NetAmount);
                cmd.Parameters.AddWithValue("@vatAmount", receipt.VatAmount);
                cmd.Parameters.AddWithValue("@vatPercent", receipt.VatPercent);
                cmd.Parameters.AddWithValue("@isprinted", "1");
                cmd.Parameters.AddWithValue("@descr", receipt.ReceiptDescription ?? "");
                cmd.ExecuteNonQuery();

                if (receiptType == 3)
                {
                    const string strMaxIndexNumber = "SELECT MAX(IndexNumber) from DeliveryInvoices";
                    var cmdMaxIndexNumber = new SqlCommand(strMaxIndexNumber, con);
                    var maxIndex = cmdMaxIndexNumber.ExecuteScalar();

                    const string updateDeliveryInvoices = "Update DeliveryInvoices set PlaceOfOrigin=@origin, PlaceofDelivery=@delivery where IndexNumber=@id";
                    var cmdUpdateDel = new SqlCommand(updateDeliveryInvoices, con);
                    cmdUpdateDel.Parameters.AddWithValue("@id", Convert.ToInt32(maxIndex));
                    cmdUpdateDel.Parameters.AddWithValue("@origin", receipt.PlaceOfOrigin);
                    cmdUpdateDel.Parameters.AddWithValue("@delivery", receipt.PlaceOfDelivery);
                    cmdUpdateDel.ExecuteNonQuery();

                    if (receipt.DeliveryInvoiceItems != null && receipt.DeliveryInvoiceItems.Count > 0)
                        SaveDeliveryInvoiceItems(receipt.DeliveryInvoiceItems, Convert.ToInt32(maxIndex));
                }

                if (receiptType == 2)
                {
                    const string strMaxIndexNumber = "SELECT MAX(IndexNumber) from Invoices";
                    var cmdMaxIndexNumber = new SqlCommand(strMaxIndexNumber, con);
                    var maxIndex = cmdMaxIndexNumber.ExecuteScalar();

                    SaveInvoiceItems(receipt.InvoiceItems, Convert.ToInt32(maxIndex));                    
                }
            }
        }

        public List<Client> GetExpiredAlerts()
        {
            var con = new SqlConnection(_connectionString);
            var result = new List<Client>();
            using (con)
            {
                con.Open();
                const string sql = "Select * from Alerts where Date < getDate()";
                var cmd = new SqlCommand(sql, con);
                var rdr = cmd.ExecuteReader();
                var alerts = new List<Alert>();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        var alert = new Alert
                        {
                            ClientId = Convert.ToInt32(rdr["ClientId"]),
                            Name = rdr["Name"].ToString(),
                            Description = rdr["Description"].ToString(),
                            Date = Convert.ToDateTime(rdr["Date"])
                        };
                        alerts.Add(alert);
                    }
                }
                var clientIds = alerts.Select(x => x.ClientId).ToList();
                foreach (var id in clientIds)
                {
                    var sqlClients = "Select * from Clients where Id=@id";
                    var cmdClients = new SqlCommand(sqlClients, con);
                    cmdClients.Parameters.AddWithValue("@id", id);
                    var rdrClients = cmdClients.ExecuteReader();
                    using (rdrClients)
                    {
                        while (rdrClients.Read())
                        {
                            var client = new Client
                            {
                                Id = Convert.ToInt32(rdrClients["Id"].ToString()),
                                IndexNumber = Convert.ToInt32(rdrClients["IndexNumber"].ToString()),
                                FirstName = rdrClients["FirstName"].ToString(),
                                LastName = rdrClients["LastName"].ToString(),
                                Email = rdrClients["Email"].ToString(),
                                Address = rdrClients["Address"].ToString(),
                                AFM = rdrClients["AFM"].ToString(),
                                DOY = rdrClients["DOY"].ToString(),
                                Title = rdrClients["Title"].ToString(),
                                DefaultPrice = Convert.ToDecimal(rdrClients["DefaultPrice"]),
                                AdministrationOffice = rdrClients["AdministrationOffice"].ToString(),
                                January = Convert.ToBoolean(rdrClients["January"]),
                                February = Convert.ToBoolean(rdrClients["February"]),
                                March = Convert.ToBoolean(rdrClients["March"]),
                                April = Convert.ToBoolean(rdrClients["April"]),
                                May = Convert.ToBoolean(rdrClients["May"]),
                                June = Convert.ToBoolean(rdrClients["June"]),
                                July = Convert.ToBoolean(rdrClients["July"]),
                                August = Convert.ToBoolean(rdrClients["August"]),
                                September = Convert.ToBoolean(rdrClients["September"]),
                                October = Convert.ToBoolean(rdrClients["October"]),
                                November = Convert.ToBoolean(rdrClients["November"]),
                                December = Convert.ToBoolean(rdrClients["December"])
                            };
                            client.Alerts = alerts.Where(x => x.ClientId == id).ToList();
                            result.Add(client);
                        }
                    }
                }
            }
            return result;
        }

        public List<Client> GetExpiringAlerts()
        {
            var con = new SqlConnection(_connectionString);
            var result = new List<Client>();
            using (con)
            {
                con.Open();
                const string sql = "select * from Alerts where DATEDIFF(day, Date, getDate()) < 20 and DATEDIFF(day, Date, getDate()) > 0";
                var cmd = new SqlCommand(sql, con);
                var rdr = cmd.ExecuteReader();
                var alerts = new List<Alert>();
                using (rdr)
                {
                    while (rdr.Read())
                    {
                        var alert = new Alert
                        {
                            ClientId = Convert.ToInt32(rdr["ClientId"]),
                            Name = rdr["Name"].ToString(),
                            Description = rdr["Description"].ToString(),
                            Date = Convert.ToDateTime(rdr["Date"])
                        };
                        alerts.Add(alert);
                    }
                }
                var clientIds = alerts.Select(x => x.ClientId).ToList();
                foreach (var id in clientIds)
                {
                    var sqlClients = "Select * from Clients where Id=@id";
                    var cmdClients = new SqlCommand(sqlClients, con);
                    cmdClients.Parameters.AddWithValue("@id", id);
                    var rdrClients = cmdClients.ExecuteReader();
                    using (rdrClients)
                    {
                        while (rdrClients.Read())
                        {
                            var client = new Client
                            {
                                Id = Convert.ToInt32(rdrClients["Id"].ToString()),
                                IndexNumber = Convert.ToInt32(rdrClients["IndexNumber"].ToString()),
                                FirstName = rdrClients["FirstName"].ToString(),
                                LastName = rdrClients["LastName"].ToString(),
                                Email = rdrClients["Email"].ToString(),
                                Address = rdrClients["Address"].ToString(),
                                AFM = rdrClients["AFM"].ToString(),
                                DOY = rdrClients["DOY"].ToString(),
                                Title = rdrClients["Title"].ToString(),
                                DefaultPrice = Convert.ToDecimal(rdrClients["DefaultPrice"]),
                                AdministrationOffice = rdrClients["AdministrationOffice"].ToString(),
                                January = Convert.ToBoolean(rdrClients["January"]),
                                February = Convert.ToBoolean(rdrClients["February"]),
                                March = Convert.ToBoolean(rdrClients["March"]),
                                April = Convert.ToBoolean(rdrClients["April"]),
                                May = Convert.ToBoolean(rdrClients["May"]),
                                June = Convert.ToBoolean(rdrClients["June"]),
                                July = Convert.ToBoolean(rdrClients["July"]),
                                August = Convert.ToBoolean(rdrClients["August"]),
                                September = Convert.ToBoolean(rdrClients["September"]),
                                October = Convert.ToBoolean(rdrClients["October"]),
                                November = Convert.ToBoolean(rdrClients["November"]),
                                December = Convert.ToBoolean(rdrClients["December"]),
                                Alerts = alerts.Where(x => x.ClientId == id).ToList()
                            };
                            result.Add(client);
                        }
                    }
                }
            }
            return result;
        }

        private void SaveInvoiceItems(List<InvoiceItem> items, int invoiceId)
        {
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                const string delete = "Delete from InvoiceItems where InvoiceId=@invoiceId";
                var cmdDelete = new SqlCommand(delete, con);
                cmdDelete.Parameters.AddWithValue("@invoiceId", invoiceId);
                cmdDelete.ExecuteNonQuery();
                if (items != null)
                {
                    const string sql = "Insert into InvoiceItems (InvoiceId, PaymentClientBehalf, Payment, Description) Values (@invoiceId, @behalf, @payment, @description)";
                    foreach (var it in items)
                    {
                        var cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@invoiceId", invoiceId);
                        cmd.Parameters.AddWithValue("@behalf", it.PaymentClientBehalf);
                        cmd.Parameters.AddWithValue("@payment", it.Payment);
                        cmd.Parameters.AddWithValue("@description", it.Description ?? "");
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void SaveDeliveryInvoiceItems(List<DeliveryInvoiceItem> items, int deliveryInvoiceId)
        {
            var con = new SqlConnection(_connectionString);
            using (con)
            {
                con.Open();
                const string delete = "Delete from DeliveryInvoiceItems where DeliveryInvoiceId=@invoiceId";
                var cmdDelete = new SqlCommand(delete, con);
                cmdDelete.Parameters.AddWithValue("@invoiceId", deliveryInvoiceId);
                cmdDelete.ExecuteNonQuery();

                if (items != null)
                {
                    const string sql = "Insert into DeliveryInvoiceItems (DeliveryInvoiceId, UnitPrice, Quantity, Description) Values (@invoiceId, @price, @quantity, @description)";
                    foreach (var it in items)
                    {
                        var cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@invoiceId", deliveryInvoiceId);
                        cmd.Parameters.AddWithValue("@price", it.UnitPrice);
                        cmd.Parameters.AddWithValue("@quantity", it.Quantity);
                        cmd.Parameters.AddWithValue("@description", it.Description ?? "");
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
