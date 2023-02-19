using System.Text.RegularExpressions;
using WebApplication1.Models;

namespace FileProcessingAPI.PdfParser.VendorParsers
{
    public class PcGaragePdfParser : PdfParser
    {
        public override InvoiceDetailsModel ParseItem(Match match, int index)
        {
            throw new NotImplementedException();
        }

        public override InvoiceModel ParsePdf(string text)
        {
            throw new NotImplementedException();
        }
    }
}
