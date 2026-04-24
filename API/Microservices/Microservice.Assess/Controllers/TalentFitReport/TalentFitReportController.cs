using Library.Assess.Models.TalentFitReport;
using Library.Assess.Services.TalentFitReport;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.TalentFitReport
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class TalentFitReportController : BasicTokenController
    {
        private readonly ITalentFitReport_DevelopmentService _talentFitReport_DevelopmentService;
        private readonly ITalentFitReport_SelectionService _talentFitReport_SelectionService;
        private readonly ITalentFitReport_InterviewService _talentFitReport_InterviewService;

        public TalentFitReportController(ITalentFitReport_DevelopmentService talentFitReport_DevelopmentService, ITalentFitReport_SelectionService talentFitReport_SelectionService, ITalentFitReport_InterviewService talentFitReport_InterviewService)
        {
            _talentFitReport_DevelopmentService = talentFitReport_DevelopmentService;
            _talentFitReport_SelectionService = talentFitReport_SelectionService;
            _talentFitReport_InterviewService = talentFitReport_InterviewService;
        }

        /// <summary>
        /// Generate talent match report PDF for a candidate
        /// </summary>
        /// <param name="request">The talent report request model containing all report data</param>
        /// <returns>PDF file of the talent match report</returns>
        [HttpPost("GenerateTalentFitReportSelection"), Authorize]
        public async Task<IActionResult> GenerateTalentFitReport(TalentFitReport_SelectionRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent fit report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentFitReport_SelectionService.GenerateReportAsync(request);

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
        [HttpPost("GenerateTalentFitReportSelectionBase64"), Authorize]
        public async Task<IActionResult> GenerateTalentFitReportBase64(TalentFitReport_SelectionRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentFitReport_SelectionService.GenerateReportAsync(request);

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

        /// <summary>
        /// Generate talent match report PDF for a candidate
        /// </summary>
        /// <param name="request">The talent report request model containing all report data</param>
        /// <returns>PDF file of the talent match report</returns>
        [HttpPost("GenerateTalentFitReportDevelopment"), Authorize]
        public async Task<IActionResult> GenerateTalentFitReportSelection(TalentFitReport_DevelopmentRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent fit report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentFitReport_DevelopmentService.GenerateReportAsync(request);

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
        [HttpPost("GenerateTalentFitReportDevelopmentBase64"), Authorize]
        public async Task<IActionResult> GenerateTalentFitReportSelectionBase64(TalentFitReport_DevelopmentRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentFitReport_DevelopmentService.GenerateReportAsync(request);

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

        /// <summary>
        /// Generate talent match report PDF for a candidate
        /// </summary>
        /// <param name="request">The talent report request model containing all report data</param>
        /// <returns>PDF file of the talent match report</returns>
        [HttpPost("GenerateTalentFitReportInterview"), Authorize]
        public async Task<IActionResult> GenerateTalentFitReportInterview(TalentFitReport_InterviewRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent fit report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentFitReport_InterviewService.GenerateReportAsync(request);

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
        [HttpPost("GenerateTalentFitReportInterviewBase64"), Authorize]
        public async Task<IActionResult> GenerateTalentFitReportInterviewBase64(TalentFitReport_InterviewRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.EmployeeName))
                    return BadRequest("Not enough information to generate talent report.");

                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var response = await _talentFitReport_InterviewService.GenerateReportAsync(request);

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
