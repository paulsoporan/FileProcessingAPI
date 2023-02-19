namespace WebApplication1.Models
{
    public class InvoiceDetailsModel
    {
        public int ProductNo { get; set; }
        public string ProductName { get; set; }
        public string UOM { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TVA { get; set; }
    }
}
