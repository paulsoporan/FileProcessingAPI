namespace FileProcessingAPI.Interfaces
{
    public interface IProcessingBL
    {
        void ProcessFilesByVendorId(int vendorId);
        decimal GetPaymentsForTheMonth(Months month, int year);
    }
}