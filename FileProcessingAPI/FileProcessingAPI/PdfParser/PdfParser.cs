using System.Text.RegularExpressions;
using FileProcessingAPI.Interfaces;
using WebApplication1.Models;

namespace FileProcessingAPI.PdfParser
{
    public abstract class PdfParser
    {
        public InvoiceModel _invoice = new InvoiceModel();
        public abstract InvoiceModel ParsePdf(string text);
        public abstract InvoiceDetailsModel ParseItem(Match match, int index);
    }
}
