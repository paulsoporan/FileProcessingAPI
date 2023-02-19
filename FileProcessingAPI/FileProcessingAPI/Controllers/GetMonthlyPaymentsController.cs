using FileProcessingAPI.Interfaces;
using FileProcessingAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileProcessingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class GetMonthlyPaymentsController : Controller
    {
        private readonly IProcessingBL _processingBL;

        public GetMonthlyPaymentsController(IProcessingBL processingBL)
        {
            _processingBL = processingBL;
        }

        ///<param name="month">The number you select represents the month of the year</param>
        [HttpGet(Name = "GetMonthlyPayments")]
        public MontlyPaymentRaportModel GetMonthlyPayments(Months month, int year)
        {
            var raport = new MontlyPaymentRaportModel();

            raport.Year = year;
            raport.Month = month.ToString();
            raport.TotalPayments = _processingBL.GetPaymentsForTheMonth(month, year);

            if (raport.TotalPayments == 0)
            {
                raport.errorMessage = "No invoices were found for the selected date";
            }

            return raport;
        }
    }
}
