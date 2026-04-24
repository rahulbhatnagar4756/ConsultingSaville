using Library.Assess.Models.ReportPDF;
using Library.Assess.Services.ReportPDF;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.ReportPDF
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class ReportPdfController : BasicTokenController
    {
        private readonly IPdfReportService _pdfReportService;

        public ReportPdfController(IPdfReportService pdfReportService)
        {
            _pdfReportService = pdfReportService;
        }

        /// <summary>
        /// Generate talent match report PDF for a candidate
        /// </summary>
        /// <param name="request">The talent report request model containing all report data</param>
        /// <returns>PDF file of the talent match report</returns>
        [HttpPost("GenerateTalentReport"), Authorize]
        public async Task<IActionResult> GenerateTalentReport(TalentReportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _pdfReportService.GenerateReportAsync(request);

                if (!response.Success)
                {
                    return BadRequest(new { message = response.Message });
                }

                if (response.PdfData == null || response.PdfData.Length == 0)
                    return StatusCode(StatusCodes.Status204NoContent, "No PDF data generated.");

                return File(response.PdfData, "application/pdf", response.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred while generating report.");
            }
        }

        /// <summary>
        /// Generate talent match report PDF and return as base64 string
        /// </summary>
        /// <param name="request">The talent report request model containing all report data</param>
        /// <returns>Base64 encoded PDF data with metadata</returns>
        [HttpPost("GenerateTalentReportBase64"), Authorize]
        public async Task<IActionResult> GenerateTalentReportBase64(TalentReportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _pdfReportService.GenerateReportAsync(request);

                if (!response.Success)
                {
                    return BadRequest(new { message = response.Message });
                }

                if (response.PdfData == null || response.PdfData.Length == 0)
                    return StatusCode(StatusCodes.Status204NoContent, "No PDF data generated.");

                var base64Pdf = Convert.ToBase64String(response.PdfData);

                return Ok(new
                {
                    success = true,
                    fileName = response.FileName,
                    pdfBase64 = base64Pdf,
                    generatedAt = response.GeneratedAt,
                    message = response.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred while generating report.");
            }
        }
    }
}
