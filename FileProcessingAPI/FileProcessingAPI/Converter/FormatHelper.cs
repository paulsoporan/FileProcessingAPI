using System.Globalization;

namespace FileProcessingAPI.Converter
{
    public class FormatHelper
    {
        public static decimal ConvertPrice(string price)
        {
            price = price.Replace(",", ".");

            return Convert.ToDecimal(price);
        }

        public static string ConvertDate(string date)
        {
            string[] formats = { "dd-MM-yyyy", "dd/MM/yyyy", "dd.MM.yyyy" };
            var convertedDate = DateTime.ParseExact(date, formats, CultureInfo.InvariantCulture);

            return convertedDate.ToString("yyyy-MM-dd");
        }
    }
}
