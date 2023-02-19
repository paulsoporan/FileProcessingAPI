using FileProcessingAPI.PdfParser;
using WebApplication1.Models;

namespace FileProcessingAPI.Interfaces
{
    public interface IPdfParserFactory
    {
        InvoiceModel ParseInvoice(int vendorId, string invoiceText);
    }
}