using FileProcessingAPI.Interfaces;
using FileProcessingAPI.PdfParser.VendorParsers;
using WebApplication1.Models;

namespace FileProcessingAPI.PdfParser
{
    public class PdfParserFactory : IPdfParserFactory
    {
        public InvoiceModel ParseInvoice(int vendorId, string invoiceText)
        {
            switch (vendorId)
            {
                case (int)Vendor.Altex:
                    return new AltexPdfParser().ParsePdf(invoiceText);
                case (int)Vendor.Cel:
                    return new CelPdfParser().ParsePdf(invoiceText);
                case (int)Vendor.PcGarage:
                    return new PcGaragePdfParser().ParsePdf(invoiceText);
                case (int)Vendor.Digi:
                    return new DigiPdfParser().ParsePdf(invoiceText);
                default:
                    return new InvoiceModel
                    {
                        Error = "Vendor parser is not implemented"
                    };
            }
        }
    }
}
