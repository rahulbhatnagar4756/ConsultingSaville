using Library.Assess.Models.PersonalityPDF;
using Library.Assess.Models.ReportPDF;
using Library.Assess.Services.PersonalityPDF;
using Library.Assess.Services.ReportPDF;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.PersonalityPdf
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class PersonalityPdfController : BasicTokenController
    {
        private readonly IPersonalityPdfService _personalityPdfService;

        public PersonalityPdfController(IPersonalityPdfService personalityPdfService)
        {
           _personalityPdfService = personalityPdfService;
        }

        /// <summary>
        /// Generate Personality Pdf
        /// </summary>
        /// <param name="request">The Personality Pdf Request model containing all report data</param>
        /// <returns>PDF file of the Personality</returns>
        [HttpPost("GeneratePersonalityPdf"), Authorize]
        public async Task<IActionResult> GeneratePersonalityPdf(PersonalityPdfRequest request)
        {
            try
            {
                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _personalityPdfService.GenerateReportAsync(request);

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
        /// Generate Personality Pdf Base64 and return as base64 string
        /// </summary>
        /// <param name="request">The Personality Pdf Request model containing all report data</param>
        /// <returns>Base64 encoded PDF data with metadata</returns>
        [HttpPost("GeneratePersonalityPdfBase64"), Authorize]
        public async Task<IActionResult> GeneratePersonalityPdfBase64(PersonalityPdfRequest request)
        {
            try
            {
                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _personalityPdfService.GenerateReportAsync(request);

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
