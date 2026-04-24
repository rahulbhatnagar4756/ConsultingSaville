using Library.Assess.Models.TalentMatchReportPDF;
using Library.Assess.Services.TalentMatchReportPDF;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.TalentMatchReportPDF
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class TalentMatchReportPDFController : BasicTokenController
    {
        private readonly ITalentMatchReportPDFService _talentMatchReportPDFService;

        public TalentMatchReportPDFController(ITalentMatchReportPDFService talentMatchReportPDFService)
        {
               _talentMatchReportPDFService = talentMatchReportPDFService; 
        }

        /// <summary>
        /// Generate talent match report PDF for a candidate
        /// </summary>
        /// <param name="request">The talent report request model containing all report data</param>
        /// <returns>PDF file of the talent match report</returns>
        [HttpPost("GenerateTalentMatchReport"), Authorize]
        public async Task<IActionResult> GenerateTalentMatchReport(TalentMatchReportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentMatchReportPDFService.GenerateReportAsync(request);

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
        [HttpPost("GenerateTalentMatchReportBase64"), Authorize]
        public async Task<IActionResult> GenerateTalentMatchReportBase64(TalentMatchReportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentMatchReportPDFService.GenerateReportAsync(request);

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
