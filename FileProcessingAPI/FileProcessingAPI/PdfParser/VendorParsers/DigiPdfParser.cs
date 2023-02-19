using FileProcessingAPI.Converter;
using System.Text.RegularExpressions;
using WebApplication1.Models;

namespace FileProcessingAPI.PdfParser.VendorParsers
{
    public class DigiPdfParser : PdfParser
    {
        public override InvoiceModel ParsePdf(string text)
        {
            _invoice.InvoiceHeader = ParseHeader(text);
            _invoice.InvoiceDetails = ParseDetails(text);
            _invoice.InvoiceFooter = ParseFooter(text);

            return _invoice;
        }

        public override InvoiceDetailsModel ParseItem(Match match, int index)
        {
            var item = new InvoiceDetailsModel
            {
                ProductNo = index,
                ProductName = match.Groups["productName"].Value,
                UOM = "Subscription",
                Quantity = FormatHelper.ConvertPrice(match.Groups["qty"].Value),
                Price = FormatHelper.ConvertPrice(match.Groups["price"].Value),
                TVA = FormatHelper.ConvertPrice(match.Groups["tva"].Value)
            };

            return item;
        }

        private InvoiceHeaderModel ParseHeader(string text)
        {
            var header = new InvoiceHeaderModel();

            var pattern = @"(?<Provider>.*)\s+Cod client\:[\r\n]+(?<ClientCode>\d+)";
            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            var clientCode = match.Success ? match.Groups["ClientCode"].Value : string.Empty;
            var provider = match.Success ? match.Groups["Provider"].Value : string.Empty;

            pattern = @"Factură seria și nr.*[\r\n]+\w+\s+\/\s+(?<InvoiceNo>\d+)[\r\n]+Judet.*[\r\n]+Data\:\s+(?<InvoiceDate>\d{2}\.\d{2}\.\d{4})";
            match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            var invoiceNo = match.Success ? match.Groups["InvoiceNo"].Value : string.Empty;
            var invoiceDate = match.Success ? FormatHelper.ConvertDate(match.Groups["InvoiceDate"].Value) : string.Empty;

            pattern = @"Nr\.\s*ord\.\s*ORC\:\s*(?<OrderNo>.*)";
            match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            var orderNo = match.Success ? match.Groups["OrderNo"].Value : string.Empty;

            header.InvoiceNo = invoiceNo;
            header.InvoiceDate = invoiceDate;
            header.Provider = provider;
            header.OrderNo = orderNo;
            header.CliendCode = clientCode;

            return header;
        }

        private List<InvoiceDetailsModel> ParseDetails(string text)
        {
            var details = new List<InvoiceDetailsModel>();
            var pattern = @"\d\.\d\s+(?<productName>.*)\s+(?<qty>\d+)\s+?(?<price>[\d.,]+)\s+\d+\s+(?<tva>[\d.,]+)\s+[\d.,]+";

            var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);

            for (int i = 0; i < matches.Count; i++)
            {
                details.Add(ParseItem(matches[i], i + 1));
            }

            return details;
        }

        private InvoiceFooterModel ParseFooter(string text)
        {
            var footer = new InvoiceFooterModel();
            var pattern = @"(?<total>[\d.,]+)\s+(?<totalTva>[\d.,]+)\s+(?<totalPament>[\d.,]+)[\r\n]+Factura curenta";
            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                footer.Total = FormatHelper.ConvertPrice(match.Groups["total"].Value);
                footer.TotalTVA = FormatHelper.ConvertPrice(match.Groups["totalTva"].Value);
                footer.TotalPayment = FormatHelper.ConvertPrice(match.Groups["totalPament"].Value);
            }

            return footer;
        }
    }
}
