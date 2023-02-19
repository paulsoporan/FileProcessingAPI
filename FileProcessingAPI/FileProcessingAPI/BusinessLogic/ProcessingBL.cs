using FileProcessingAPI.Interfaces;

namespace FileProcessingAPI.BusinessLogic
{
    public class ProcessingBL : IProcessingBL
    {
        private readonly IFileRepository _fileRepository;
        private readonly IPdfParserFactory _parserFactory;
        private readonly IDbRepository _dbRepository;
        private readonly IITextExtractor _textExtractor;

        public ProcessingBL(IFileRepository fileRepository,
                            IPdfParserFactory parserFactory,
                            IDbRepository dbRepository,
                            IITextExtractor textExtractor)
        {
            _fileRepository = fileRepository;
            _parserFactory = parserFactory;
            _dbRepository = dbRepository;
            _textExtractor = textExtractor;
        }

        public void ProcessFilesByVendorId(int vendorId)
        {
            var filesLocation = _dbRepository.GetVendorFilePath(vendorId);
            var filesToProcess = _fileRepository.GetInvoicesForProcessing(filesLocation);

            foreach(var file in filesToProcess)
            {
                var filebytes = _fileRepository.GetFileBytes(file);
                var invoiceText = _textExtractor.GetText(filebytes);

                var invoice = _parserFactory.ParseInvoice(vendorId, invoiceText);

                if (invoice.Error != null)
                { 
                    //_dbRepository.LogError(invoice.InvoiceHeader.InvoiceNo, invoice.Error);
                }
                else
                {
                    _dbRepository.SaveInvoice(invoice);
                }
            }
        }

        public decimal GetPaymentsForTheMonth(Months month, int year)
        {
            return _dbRepository.GetMontlhyTotalPayments((int)month, year);
        }
    }
}
