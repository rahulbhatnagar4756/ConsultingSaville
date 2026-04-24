using Library.Assess.Models.TalentMatchSelectionReportPDF;
using Library.Assess.Services.TalentMatchSelectionReportPDF;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.TalentMatchSelectionReportPDF
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class TalentMatchSelectionReportPDFController : BasicTokenController
    {
        private readonly ITalentMatchSelectionReportPDFService _talentMatchSelectionReportPDFService;

        public TalentMatchSelectionReportPDFController(ITalentMatchSelectionReportPDFService talentMatchSelectionReportPDFService)
        {
            _talentMatchSelectionReportPDFService = talentMatchSelectionReportPDFService;
        }

        /// <summary>
        /// Generate talent match selection report PDF for a candidate
        /// </summary>
        /// <param name="request">The talent selection report request model containing all report data</param>
        /// <returns>PDF file of the talent match selection report</returns>
        [HttpPost("GenerateTalentMatchSelectionReport"), Authorize]
        public async Task<IActionResult> GenerateTalentMatchSelectionReport(TalentMatchSelectionReportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentMatchSelectionReportPDFService.GenerateReportAsync(request);

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
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred while generating selection report.");
            }
        }

        /// <summary>
        /// Generate talent match selection report PDF and return as base64 string
        /// </summary>
        /// <param name="request">The talent selection report request model containing all report data</param>
        /// <returns>Base64 encoded PDF data with metadata</returns>
        [HttpPost("GenerateTalentMatchSelectionReportBase64"), Authorize]
        public async Task<IActionResult> GenerateTalentMatchSelectionReportBase64(TalentMatchSelectionReportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentMatchSelectionReportPDFService.GenerateReportAsync(request);

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
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred while generating selection report.");
            }
        }
    }
}
