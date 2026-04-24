using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Assess.Services.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Assess.Projects.Controllers
{
    [Route("api/Assess/Projects/[controller]")]
    [ApiController]
    public class CandidateRegistrationController : ControllerBase
    {
        private readonly IProjectsUsersService _projectsUsersService;
        private readonly ILogger<CandidateRegistrationController> _logger;

        public CandidateRegistrationController(
            IProjectsUsersService projectsUsersService,
            ILogger<CandidateRegistrationController> logger)
        {
            _projectsUsersService = projectsUsersService;
            _logger = logger;
        }

        /// <summary>
        /// Register a candidate for a project using quick link or JWT token
        /// </summary>
        /// <param name="model">Candidate registration information</param>
        /// <returns>Registration result with success status and message</returns>
        [HttpPost("Register")]
        [AllowAnonymous] // Allow anonymous access for candidate self-registration
        public async Task<IActionResult> RegisterCandidate([FromBody] CandidateRegistrationModel model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogWarning("Candidate registration attempted with null model");
                    return BadRequest(new ResultsModel
                    {
                        isValid = false,
                        Message = "Invalid registration data provided."
                    });
                }

                // Validate required fields
                var validationResult = ValidateRegistrationModel(model);
                if (!validationResult.isValid)
                {
                    _logger.LogWarning("Candidate registration validation failed: {Message}", validationResult.Message);
                    return BadRequest(validationResult);
                }

                _logger.LogInformation("Processing candidate registration for email: {Email}, project: {ProjectUUID}",
                                        model.Email, 
                                        model.ProjectsUUID);

                var result = await _projectsUsersService.RegisterCandidateAsync(model);

                if (result == null)
                {
                    _logger.LogError("Registration service returned null result for email: {Email}", model.Email);
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new ResultsModel
                        {
                            isValid = false,
                            Message = "Registration failed due to an unexpected error."
                        });
                }

                if (!result.isValid)
                {
                    _logger.LogWarning("Registration failed for email: {Email} - {Message}", model.Email, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Candidate registration successful for email: {Email}", model.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during candidate registration for email: {Email}", model?.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResultsModel
                    {
                        isValid = false,
                        Message = "An unexpected error occurred during registration. Please try again."
                    });
            }
        }

        /// <summary>
        /// Validate Quick Link Code
        /// </summary>
        /// <param name="quickLinkCode">Quick link code to validate</param>
        /// <returns>Validation result with project information if valid</returns>
        [HttpGet("ValidateQuickLink/{quickLinkCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateQuickLink(string quickLinkCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(quickLinkCode))
                {
                    return BadRequest(new ResultsModel
                    {
                        isValid = false,
                        Message = "Quick link code is required."
                    });
                }

                var result = await _projectsUsersService.ValidateQuickLinkAsync(quickLinkCode);

                if (result == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new ResultsModel
                        {
                            isValid = false,
                            Message = "Validation failed due to an unexpected error."
                        });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating quick link: {QuickLinkCode}", quickLinkCode);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResultsModel
                    {
                        isValid = false,
                        Message = "An unexpected error occurred during validation."
                    });
            }
        }

        [HttpGet("CandidateAutoLoginWithToken/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> CandidateAutoLogin(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogWarning("Candidate auto-login attempted with empty token");
                    return BadRequest(new CandidateAutoLoginResult
                    {
                        IsValid = false,
                        Message = "Token is required for auto-login."
                    });
                }

                _logger.LogInformation("Processing candidate auto-login with token");
                var result = await _projectsUsersService.ValidateAutoLoginToken(token);

                if (!result.IsValid)
                {
                    _logger.LogWarning("Candidate auto-login failed: {Message}", result.Message);
                    return Unauthorized(result);
                }

                _logger.LogInformation("Candidate auto-login successful for user: {UsersUUID}", result.UsersUUID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during candidate auto-login");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CandidateAutoLoginResult
                    {
                        IsValid = false,
                        Message = "An unexpected error occurred during auto-login validation."
                    });
            }
        }

        private static ResultsModel ValidateRegistrationModel(CandidateRegistrationModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CompanyUUID))
                return new ResultsModel { isValid = false, Message = "Company information is required." };

            if (string.IsNullOrWhiteSpace(model.ProjectsUUID))
                return new ResultsModel { isValid = false, Message = "Project information is required." };

            if (string.IsNullOrWhiteSpace(model.FirstName))
                return new ResultsModel { isValid = false, Message = "First name is required." };

            if (string.IsNullOrWhiteSpace(model.LastName))
                return new ResultsModel { isValid = false, Message = "Last name is required." };

            if (string.IsNullOrWhiteSpace(model.Email))
                return new ResultsModel { isValid = false, Message = "Email address is required." };

            if (!IsValidEmail(model.Email))
                return new ResultsModel { isValid = false, Message = "Please provide a valid email address." };

            return new ResultsModel { isValid = true };
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var emailValidation = new EmailAddressAttribute();
                return emailValidation.IsValid(email);
            }
            catch
            {
                return false;
            }
        }
    }
}