using Dapper;
using FileProcessingAPI.Interfaces;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace FileProcessingAPI.Repositories
{
    public class DbRepository : IDbRepository
    {
        private readonly IConfiguration _configuration;

        public DbRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SaveInvoice(InvoiceModel invoice)
        {
            var conn = Connection();
            using (conn)
            {
                var procedure = "SP_InsertInvoice";
                var invoiceParameters = new
                {
                    provider = invoice.InvoiceHeader.Provider,
                    invoiceNo = invoice.InvoiceHeader.InvoiceNo,
                    invoiceDate = invoice.InvoiceHeader.InvoiceDate,
                    orderNo = invoice.InvoiceHeader.OrderNo,
                    clientCode = invoice.InvoiceHeader.CliendCode,
                    paymenyType = invoice.InvoiceHeader.PaymentType,
                };

                conn.Execute(procedure, invoiceParameters, commandType: CommandType.StoredProcedure);

                procedure = "SP_InsertInvoiceDetails";
                foreach (var item in invoice.InvoiceDetails)
                {
                    var invoiceDetailsParameters = new
                    {
                        invoiceNo = invoice.InvoiceHeader.InvoiceNo,
                        productNo = item.ProductNo,
                        productName = item.ProductName,
                        uom = item.UOM,
                        quantity = item.Quantity,
                        price = item.Price,
                        tVA = item.TVA
                    };

                    conn.Execute(procedure, invoiceDetailsParameters, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public string GetVendorFilePath(int vendorId)
        {
            var conn = Connection();
            var path = string.Empty;

            using (conn)
            {
                var procedure = "dbo.GetVendorFilePath";
                var parameters = new
                {
                    id = vendorId
                };

                path = conn.QueryFirstOrDefault<string>(procedure, parameters, commandType: CommandType.StoredProcedure);
            }

            return path;
        }

        public decimal GetMontlhyTotalPayments(int month, int year)
        {
            try
            {
                var conn = Connection();
                var result = 0m;

                using (conn)
                {
                    var procedure = "dbo.GetMontlhyPayments";
                    var parameters = new
                    {
                        month = month,
                        year = year
                    };

                    result = conn.QueryFirstOrDefault<decimal>(procedure, parameters, commandType: CommandType.StoredProcedure);
                }

                return result;

            }
            catch(Exception e)
            {
                return 0m;
            }
        }

        public void LogError(string invoiceId, string errorMessage)
        {
            var conn = Connection();

            using (conn)
            {
                var procedure = "dbo.LogInvoiceParsingError";
                var parameters = new
                {
                    message = errorMessage,
                };

                conn.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        private SqlConnection Connection()
        {
            var conn = new SqlConnection(_configuration.GetConnectionString("sqlConnection"));

            return conn;
        }
    }
}
