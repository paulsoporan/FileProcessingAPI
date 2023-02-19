using WebApplication1.Models;

namespace FileProcessingAPI.Interfaces
{
    public interface IDbRepository
    {
        void SaveInvoice(InvoiceModel invoice);
        public string GetVendorFilePath(int vendorId);
        decimal GetMontlhyTotalPayments(int month, int year);
        public void LogError(string invoiceId, string errorMessage);
    }
}