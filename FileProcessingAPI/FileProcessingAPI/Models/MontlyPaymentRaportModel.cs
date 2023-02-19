namespace FileProcessingAPI.Models
{
    public class MontlyPaymentRaportModel
    {
        public decimal TotalPayments { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string GeneratedDateTime
        {
            get { return DateTime.Now.ToString("g"); }
            set { }
        }
        public string errorMessage { get; set; }
    }
}
