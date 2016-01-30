using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using EReceipt.Models;
using EReceipts.DAL;
using ICSharpCode.SharpZipLib.Zip;
using Pechkin;

namespace EReceipt.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Receipts")]
    public class ReceiptsController : ApiController
    {
        private IDataAccess _dal;

        public ReceiptsController()
        {
            _dal = new DataAccess(ConfigurationManager.ConnectionStrings["AuthContext"].ConnectionString);
            //_dal = new DataAccessFake();
        }

        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        public List<Client> GetClients()
        {
            try
            {
                //TODO. Add generic result class and use
                var result = _dal.GetClients();
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        public List<Receipt> GetClientReceipts(int clientId)
        {
            try
            {
                //TODO. Add generic result class and use
                var result = _dal.GetClientReceipts(clientId);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        public List<Invoice> GetClientInvoices(int clientId)
        {
            try
            {
                //TODO. Add generic result class and use
                var result = _dal.GetClientInvoices(clientId);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        public List<DeliveryInvoice> GetClientDeliveryInvoices(int clientId)
        {
            try
            {
                //TODO. Add generic result class and use
                var result = _dal.GetClientDeliveryInvoices(clientId);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        public List<ClientReceipt> GetMultipleReceipts(int month, int year)
        {
            var result = _dal.GetMultipleReceipts(month, year);
            return result;
        }
        
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public void SaveClient(Client client)
        {
            _dal.SaveClient(client);
        }

        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public void InsertClient(Client client)
        {
            _dal.InsertClient(client);
        }

        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public void DeleteClient(int clientId)
        {
            _dal.DeleteClient(clientId);
        }

        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public void SaveReceipt(Receipt receipt)
        {
            _dal.SaveReceipt(receipt, receipt.ReceiptType);
        }

        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public void InsertReceipt(Receipt receipt)
        {
            _dal.CreateReceipt(receipt, receipt.ReceiptType);
        }

        [HttpGet]
        public bool SaveMultipleReceipts(int month, int year)
        {
            try
            {
                var receipts = _dal.SaveMultipleReceipts(month, year);

                var path = HttpContext.Current.Server.MapPath("~/receiptTemplate.html");
                var template = File.ReadAllText(path);

                var zipPath = string.Format("{0}\\receipts_{1}_{2}.zip", HttpContext.Current.Server.MapPath("~/exports"), month, year);
                var sourceZip = ZipFile.Create(zipPath);
                sourceZip.BeginUpdate();

                var gc = new GlobalConfig();
                gc.SetPaperSize(kind: PaperKind.A4);

                var margins = new Margins(30, 30, 30, 30);
                gc.SetMargins(margins);
                var pechkin = Factory.Create(gc);
                var objConfig = new ObjectConfig();
                objConfig.SetLoadImages(true);
                objConfig.SetPrintBackground(true);
                objConfig.SetScreenMediaType(true);
                objConfig.SetCreateExternalLinks(true);
                var fileList = new List<string>();
                foreach (var r in receipts)
                {
                    var receiptTemplate = template;
                    receiptTemplate = receiptTemplate.Replace("{indexNumber}", r.Receipt.IndexNumber.ToString());
                    receiptTemplate = receiptTemplate.Replace("{total}", r.Receipt.TotalAmount.ToString());
                    receiptTemplate = receiptTemplate.Replace("{vat}", r.Receipt.VatAmount.ToString());
                    receiptTemplate = receiptTemplate.Replace("{vatpercent}", ConfigurationManager.AppSettings["DefaultVatPercent"]);
                    receiptTemplate = receiptTemplate.Replace("{net}", r.Receipt.NetAmount.ToString());
                    receiptTemplate = receiptTemplate.Replace("{firstName}", r.Client.FirstName);
                    receiptTemplate = receiptTemplate.Replace("{lastName}", r.Client.LastName);
                    receiptTemplate = receiptTemplate.Replace("{address}", r.Client.Address);
                    receiptTemplate = receiptTemplate.Replace("{jobTitle}", r.Client.Title);
                    receiptTemplate = receiptTemplate.Replace("{afm}", r.Client.AFM);
                    receiptTemplate = receiptTemplate.Replace("{doy}", r.Client.DOY);
                    receiptTemplate = receiptTemplate.Replace("{desciption}", r.Receipt.ReceiptDescription);
                    receiptTemplate = receiptTemplate.Replace("{date}", r.Receipt.Date.ToString("dd-MM-yyyy"));

                    gc.SetDocumentTitle(r.Client.Address.Replace(".", "_"));
                    var pdf = pechkin.Convert(objConfig, receiptTemplate);
                    r.Client.Address = r.Client.Address.Replace(@"/", "-").Replace(@"\", "-");
                    r.Client.AdministrationOffice = r.Client.AdministrationOffice.Replace(@"/", "-").Replace(@"\", "-");
                    var filename = string.Format("{0}\\{1}_{2}{3}.pdf", HttpContext.Current.Server.MapPath("~/exports"),
                        r.Client.IndexNumber, r.Client.Address, (!String.IsNullOrEmpty(r.Client.AdministrationOffice) ? "_" + r.Client.AdministrationOffice : ""));
                    File.WriteAllBytes(filename, pdf);
                    fileList.Add(filename);
                    sourceZip.Add(new StaticDiskDataSource(filename), Path.GetFileName(filename), CompressionMethod.Deflated, true);
                }

                sourceZip.CommitUpdate();
                sourceZip.Close();

                foreach (var file in fileList)
                {
                    File.Delete(file);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet]
        public List<Client> GetExpiredAlerts()
        {
            try
            {
                var clientAlerts = _dal.GetExpiredAlerts();
                return clientAlerts;
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage PrintInvoice(string invoiceId)
        {
            var path = HttpContext.Current.Server.MapPath("~/invoiceTemplate.html");
            var template = File.ReadAllText(path);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var invoice = _dal.GetInvoice(Convert.ToInt32(invoiceId));
            var client = _dal.GetClient(invoice.ClientId);
            var receiptTemplate = template;
            receiptTemplate = receiptTemplate.Replace("{indexNumber}", invoice.IndexNumber.ToString());
            receiptTemplate = receiptTemplate.Replace("{total}", invoice.TotalAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{vat}", invoice.VatAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{vatpercent}", invoice.VatPercent.ToString());
            receiptTemplate = receiptTemplate.Replace("{net}", invoice.NetAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{firstName}", client.FirstName);
            receiptTemplate = receiptTemplate.Replace("{lastName}", client.LastName);
            receiptTemplate = receiptTemplate.Replace("{address}", client.Address);
            receiptTemplate = receiptTemplate.Replace("{jobTitle}", client.Title);
            receiptTemplate = receiptTemplate.Replace("{afm}", client.AFM);
            receiptTemplate = receiptTemplate.Replace("{doy}", client.DOY);
            receiptTemplate = receiptTemplate.Replace("{desciption}", invoice.ReceiptDescription);
            receiptTemplate = receiptTemplate.Replace("{date}", invoice.Date.ToString("dd-MM-yyyy"));
            var html = "";
            foreach (var item in invoice.InvoiceItems)
            {
                html += "<tr>";
                html += "<td>" + item.Description + "</td>";
                html += "<td>" + item.Payment + "</td>";
                html += "<td>" + item.PaymentClientBehalf + "</td>";
                html += "<td>" + (1*item.Payment + 1*item.PaymentClientBehalf) + "</td>";
                html += "</tr>";
            }
            receiptTemplate = receiptTemplate.Replace("{items}", html);

            var gc = new GlobalConfig();
            gc.SetPaperSize(kind: PaperKind.A4);
            gc.SetDocumentTitle(client.Address.Replace(".", "_"));
            var margins = new Margins(30, 30, 30, 30);
            gc.SetMargins(margins);

            var pechkin = Factory.Create(gc);
            var objConfig = new ObjectConfig();
            objConfig.SetLoadImages(true);
            objConfig.SetPrintBackground(true);
            objConfig.SetScreenMediaType(true);
            objConfig.SetCreateExternalLinks(true);
            objConfig.Footer.SetLeftText("www.tsotsis.gr").SetFontSize(6).SetFontName("Verdana");
            var pdf = pechkin.Convert(objConfig, receiptTemplate);

            var stream = new MemoryStream(pdf);
            result.Headers.AcceptRanges.Add("bytes");
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = string.Format("receipt_{0}_{1}.pdf", DateTime.Now.ToShortDateString(), invoice.IndexNumber);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage PrintDeliveryInvoice(string deliveryInvoiceId)
        {
            var path = HttpContext.Current.Server.MapPath("~/deliveryInvoiceTemplate.html");
            var template = File.ReadAllText(path);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var invoice = _dal.GetDeliveryInvoice(Convert.ToInt32(deliveryInvoiceId));
            var client = _dal.GetClient(invoice.ClientId);
            var receiptTemplate = template;
            receiptTemplate = receiptTemplate.Replace("{indexNumber}", invoice.IndexNumber.ToString());
            receiptTemplate = receiptTemplate.Replace("{total}", invoice.TotalAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{vat}", invoice.VatAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{vatpercent}", invoice.VatPercent.ToString());
            receiptTemplate = receiptTemplate.Replace("{net}", invoice.NetAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{firstName}", client.FirstName);
            receiptTemplate = receiptTemplate.Replace("{lastName}", client.LastName);
            receiptTemplate = receiptTemplate.Replace("{address}", client.Address);
            receiptTemplate = receiptTemplate.Replace("{jobTitle}", client.Title);
            receiptTemplate = receiptTemplate.Replace("{afm}", client.AFM);
            receiptTemplate = receiptTemplate.Replace("{doy}", client.DOY);
            receiptTemplate = receiptTemplate.Replace("{desciption}", invoice.ReceiptDescription);
            receiptTemplate = receiptTemplate.Replace("{date}", invoice.Date.ToString("dd-MM-yyyy"));
            var html = "";
            foreach (var item in invoice.DeliveryInvoiceItems)
            {
                html += "<tr>";
                html += "<td>" + item.Description + "</td>";
                html += "<td>" + item.UnitPrice + "</td>";
                html += "<td>" + item.Quantity + "</td>";
                html += "<td>" + item.UnitPrice * item.Quantity + "</td>";
                html += "</tr>";
            }
            receiptTemplate = receiptTemplate.Replace("{items}", html);

            var gc = new GlobalConfig();
            gc.SetPaperSize(kind: PaperKind.A4);
            gc.SetDocumentTitle(client.Address.Replace(".", "_"));
            var margins = new Margins(30, 30, 30, 30);
            gc.SetMargins(margins);

            var pechkin = Factory.Create(gc);
            var objConfig = new ObjectConfig();
            objConfig.SetLoadImages(true);
            objConfig.SetPrintBackground(true);
            objConfig.SetScreenMediaType(true);
            objConfig.SetCreateExternalLinks(true);
            objConfig.Footer.SetLeftText("www.tsotsis.gr").SetFontSize(6).SetFontName("Verdana");
            var pdf = pechkin.Convert(objConfig, receiptTemplate);

            var stream = new MemoryStream(pdf);
            result.Headers.AcceptRanges.Add("bytes");
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = string.Format("receipt_{0}_{1}.pdf", DateTime.Now.ToShortDateString(), invoice.IndexNumber);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage PrintReceipt(string receiptId)
        {
            var path = HttpContext.Current.Server.MapPath("~/receiptTemplate.html");
            var template = File.ReadAllText(path);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var receipt = _dal.GetReceipt(Convert.ToInt32(receiptId));
            var client = _dal.GetClient(receipt.ClientId);
            var receiptTemplate = template;
            receiptTemplate = receiptTemplate.Replace("{indexNumber}", receipt.IndexNumber.ToString());
            receiptTemplate = receiptTemplate.Replace("{total}", receipt.TotalAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{vat}", receipt.VatAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{vatpercent}", receipt.VatPercent.ToString());
            receiptTemplate = receiptTemplate.Replace("{net}", receipt.NetAmount.ToString());
            receiptTemplate = receiptTemplate.Replace("{firstName}", client.FirstName);
            receiptTemplate = receiptTemplate.Replace("{lastName}", client.LastName);
            receiptTemplate = receiptTemplate.Replace("{address}", client.Address);
            receiptTemplate = receiptTemplate.Replace("{jobTitle}", client.Title);
            receiptTemplate = receiptTemplate.Replace("{afm}", client.AFM);
            receiptTemplate = receiptTemplate.Replace("{doy}", client.DOY);
            receiptTemplate = receiptTemplate.Replace("{desciption}", receipt.ReceiptDescription);
            receiptTemplate = receiptTemplate.Replace("{date}", receipt.Date.ToString("dd-MM-yyyy"));

            var gc = new GlobalConfig();
            gc.SetPaperSize(kind: PaperKind.A4);
            gc.SetDocumentTitle(client.Address.Replace(".", "_"));
            var margins = new Margins(30, 30, 30, 30);
            gc.SetMargins(margins);

            var pechkin = Factory.Create(gc);
            var objConfig = new ObjectConfig();
            objConfig.SetLoadImages(true);
            objConfig.SetPrintBackground(true);
            objConfig.SetScreenMediaType(true);
            objConfig.SetCreateExternalLinks(true);
            //objConfig.Footer.SetRightText(footerlbl.Text).SetFontSize(6).SetFontName("Verdana");
            var pdf = pechkin.Convert(objConfig, receiptTemplate);

            var stream = new MemoryStream(pdf);
            result.Headers.AcceptRanges.Add("bytes");
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = string.Format("receipt_{0}_{1}.pdf", DateTime.Now.ToShortDateString(), receipt.IndexNumber);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int month, int year)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var zipPath = string.Format("{0}\\receipts_{1}_{2}.zip", HttpContext.Current.Server.MapPath("~/exports"), month, year);
            
            var stream = new FileStream(zipPath, FileMode.Open);
            result.Headers.AcceptRanges.Add("bytes");
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = string.Format("receipts_{0}.zip", DateTime.Now.ToShortDateString());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
}
