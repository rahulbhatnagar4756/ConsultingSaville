using Library.Assess.Models;
using Library.Assess.Services.Projects;
using Microservice.Assess.Projects.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.Projects
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class CandidateController : BasicTokenController
    {

        private readonly IAssessmentUsersService _assessmentUsersService;
        private readonly ILogger<CandidateRegistrationController> _logger;

        public CandidateController(IAssessmentUsersService AssessmentUsersService,
                                   ILogger<CandidateRegistrationController> logger)
        {
            _assessmentUsersService = AssessmentUsersService;
            _logger = logger;
        }


        /// <summary>
        /// Get employee job education records by job UUID
        /// </summary>
        /// <param name="employeeJobsUUID">The UUID of the employee job to get education for</param>
        /// <returns></returns>
        [HttpPost("AssessmentList"), Authorize]
        public async Task<IActionResult> GetEmployeeJobEducationByJobUUID(BasicGetWithProjectModel model)
        {
            if (string.IsNullOrEmpty(model.CompanyUUID) || string.IsNullOrEmpty(model.UsersUUID))
                return BadRequest("Not enough information to get candidate assessments.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var data = await _assessmentUsersService.GetCandidateTestsAsync(model);

            if (data == null) return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

    }
}
