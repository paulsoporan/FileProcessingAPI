using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.Text;
using FileProcessingAPI.Interfaces;

namespace FileProcessingAPI.TextExtractor
{
    public class ITextExtractor : IITextExtractor
    {
        public string GetText(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            using (PdfReader reader = new PdfReader(bytes))
            {
                for (int pageNo = 1; pageNo <= reader.NumberOfPages; pageNo++)
                {
                    string text = PdfTextExtractor.GetTextFromPage(reader, pageNo);
                    text = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
                    sb.Append(text);
                }
            }

            return sb.ToString();
        }
    }
}
