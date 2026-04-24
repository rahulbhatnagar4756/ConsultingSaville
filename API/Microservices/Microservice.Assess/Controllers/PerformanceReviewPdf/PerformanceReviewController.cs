using Library.Assess.Models.PerformanceReview;
using Library.Assess.Services.PerformanceReviewReports;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.PerformanceReviewPdf
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class PerformanceReviewController : BasicTokenController
    {
        private readonly IPerformanceReviewService _performanceReview;

        public PerformanceReviewController(IPerformanceReviewService performanceReview)
        {
            _performanceReview = performanceReview;
        }

        /// <summary>
        /// Generates a performance review PDF for an employee.
        /// </summary>
        /// <returns>PDF file containing the performance review report</returns>
        [HttpPost("GeneratePerformanceReviewReport")]
        [Authorize]
        public async Task<IActionResult> GeneratePerformanceReviewReport(PerformanceReviewRequest request)
        {   
            try
            {
                var (_, errorResult) = ValidateUserClaims();
                if (errorResult != null)
                    return errorResult;

                var response = await _performanceReview.GenerateReportAsync(request);

                if (!response.Success)
                    return BadRequest(new { message = response.Message });

                if (response.PdfData == null || response.PdfData.Length == 0)
                    return StatusCode(StatusCodes.Status204NoContent, "No PDF data generated.");

                //return File(response.PdfData, "application/pdf", response.FileName);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Internal server error occurred while generating the performance review report."
                );
            }
        }

        /// <summary>
        /// Generates a performance review PDF and returns it as a Base64-encoded string.
        /// </summary>
        /// <returns>Base64-encoded PDF data with metadata</returns>
        [HttpPost("GeneratePerformanceReviewReportBase64")]
        [Authorize]
        public async Task<IActionResult> GeneratePerformanceReviewReportBase64(PerformanceReviewRequest request)
        {
            try
            {
                var (_, errorResult) = ValidateUserClaims();
                if (errorResult != null)
                    return errorResult;

                var response = await _performanceReview.GenerateReportAsync(request);

                if (!response.Success)
                    return BadRequest(new { message = response.Message });

                if (response.PdfData == null || response.PdfData.Length == 0)
                    return StatusCode(StatusCodes.Status204NoContent, "No PDF data generated.");

                return Ok(new
                {
                    success = true,
                    fileName = response.FileName,
                    pdfBase64 = Convert.ToBase64String(response.PdfData),
                    generatedAt = response.GeneratedAt,
                    message = response.Message
                });
            }
            catch
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Internal server error occurred while generating the performance review report."
                );
            }
        }
    }
}
