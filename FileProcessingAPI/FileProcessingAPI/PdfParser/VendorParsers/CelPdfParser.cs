using FileProcessingAPI.Converter;
using System.Text.RegularExpressions;
using WebApplication1.Models;

namespace FileProcessingAPI.PdfParser.VendorParsers
{
    public class CelPdfParser : PdfParser
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
                ProductName = match.Groups["ProductName"].Value,
                UOM = match.Groups["UOM"].Value,
                Quantity = FormatHelper.ConvertPrice(match.Groups["Quantity"].Value),
                Price = FormatHelper.ConvertPrice(match.Groups["Price"].Value),
                TVA = FormatHelper.ConvertPrice(match.Groups["TVA"].Value)
            };

            return item;
        }

        private InvoiceHeaderModel ParseHeader(string text)
        {
            var header = new InvoiceHeaderModel();
            var pattern = @"(?<InvoiceNo>\d+)[\r\n]+din data de "
                        + @"(?<InvoiceDate>\d{2}-\d{2}-\d{4})[\r\n]+"
                        + @"(?<Provider>[^\r\n]+)[\S\s]+?reg\.com\. nr\. "
                        + @"(?<OrderNo>[\w\/]+)[\S\s]+preturile de pe factura\. \| "
                        + @"(?<PaymentType>[\w ]+)";
            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                header.InvoiceNo = match.Groups["InvoiceNo"].Value;
                header.InvoiceDate = FormatHelper.ConvertDate(match.Groups["InvoiceDate"].Value);
                header.Provider = match.Groups["Provider"].Value;
                header.OrderNo = match.Groups["OrderNo"].Value;
                header.PaymentType = match.Groups["PaymentType"].Value;
            }

            return header;
        }

        private List<InvoiceDetailsModel> ParseDetails(string text)
        {
            var details = new List<InvoiceDetailsModel>();
            var pattern = @"\d\s+"
                        + @"(?<ProductName>.*) (\- )?(\w+ )?"
                        + @"(?<UOM>\w+)\s+"
                        + @"(?<Quantity>\d+)\s+"
                        + @"(?<Price>[\d.,]+)\s+[\d.,]+\s+"
                        + @"(?<TVA>[\d.,]+)";

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
            var pattern = @"in stare perfecta[\r\n]+.*[\r\n]+"
                        + @"(?<Total>[\d.,]+)\s+"
                        + @"(?<TotalTVA>[\d.,]+)[\S\s]+total ron\s+"
                        + @"(?<TotalPayment>[\d.,]+)";
            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                footer.Total = FormatHelper.ConvertPrice(match.Groups["Total"].Value);
                footer.TotalTVA = FormatHelper.ConvertPrice(match.Groups["TotalTVA"].Value);
                footer.TotalPayment = FormatHelper.ConvertPrice(match.Groups["TotalPayment"].Value);
            }

            return footer;
        }
    }
}
