using FileProcessingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileProcessingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProcessVendorFilesController : Controller
    {
        private readonly IProcessingBL _processingBL;

        public ProcessVendorFilesController(IProcessingBL processingBL)
        {
            _processingBL = processingBL;
        }

        [HttpPost(Name = "ProcessFiles")]
        public ActionResult ProcessFiles(int vendorId)
        {
            try
            {
                _processingBL.ProcessFilesByVendorId(vendorId);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    message = "Could not process files",
                    error = e.Message
                });
            }

            return Ok();
        }
    }
}
