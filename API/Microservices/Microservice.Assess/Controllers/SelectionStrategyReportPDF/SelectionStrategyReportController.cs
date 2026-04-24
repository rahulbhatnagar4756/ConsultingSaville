using Library.Assess.Models.ReportPDF;
using Library.Assess.Models.SelectionStrategyReport;
using Library.Assess.Services.ReportPDF;
using Library.Assess.Services.SelectionStrategyReport;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.SelectionStrategyReportPDF
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class SelectionStrategyReportController : BasicTokenController
    {
        private readonly ISelectionStrategyReportService _selectionStrategyReportService;

        public SelectionStrategyReportController(ISelectionStrategyReportService selectionStrategyReportService)
        {
           _selectionStrategyReportService = selectionStrategyReportService;
        }

        /// <summary>
        /// Generate talent match report PDF for a candidate
        /// </summary>
        /// <param name="request">The talent report request model containing all report data</param>
        /// <returns>PDF file of the talent match report</returns>
        [HttpPost("GenerateSelectionStrategyReport"), Authorize]
        public async Task<IActionResult> GenerateSelectionStrategyReport(SelectionStrategyReportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Candidate))
                    return BadRequest("Not enough information to generate selection strategy report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _selectionStrategyReportService.GenerateReportAsync(request);

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
        [HttpPost("GenerateSelectionStrategyReportBase64"), Authorize]
        public async Task<IActionResult> GenerateTalentReportBase64(SelectionStrategyReportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Candidate))
                    return BadRequest("Not enough information to generate selection strategy report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _selectionStrategyReportService.GenerateReportAsync(request);

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
