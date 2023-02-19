namespace WebApplication1.Models
{
    public class InvoiceModel
    {
        public InvoiceHeaderModel InvoiceHeader { get; set; }
        public List<InvoiceDetailsModel> InvoiceDetails { get; set; }
        public InvoiceFooterModel InvoiceFooter { get; set; }
        public string Error { get; set; }
    }
}
