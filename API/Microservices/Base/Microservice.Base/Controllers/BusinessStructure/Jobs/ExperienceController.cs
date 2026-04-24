using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Base.Services.Employees.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers.BusinessStructure.Jobs
{
    [Route("api/Employee/[controller]")]
    [ApiController]
    public class EmployeeJobExperienceController : BaseEmployeeController
    {
        private readonly IEmployeeJobExperienceService _employeeJobExperienceService;

        public EmployeeJobExperienceController(IEmployeeJobExperienceService employeeJobExperienceService)
        {
            _employeeJobExperienceService = employeeJobExperienceService;
        }

        /// <summary>
        /// Get employee job experience records by job UUID
        /// </summary>
        /// <param name="employeeJobsUUID">The UUID of the employee job to get experience for</param>
        /// <returns></returns>
        [HttpGet("All/{employeeJobsUUID}"), Authorize]
        public async Task<IActionResult> GetEmployeeJobExperienceByJobUUID(string employeeJobsUUID)
        {
            if (string.IsNullOrEmpty(employeeJobsUUID))
                return BadRequest("Employee job UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var data = await _employeeJobExperienceService.GetEmployeeJobExperienceByJobUUID(basicModel!, employeeJobsUUID);

            if (data == null) return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

        /// <summary>
        /// Save employee job experience record
        /// </summary>
        /// <param name="experience">The experience record to save</param>
        /// <returns></returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveEmployeeJobExperience([FromBody] EmployeeJobExperienceBaseModel experience)
        {
            if (experience == null)
                return BadRequest("Invalid experience data.");

            if (string.IsNullOrEmpty(experience.EmployeeJobsUUID) || string.IsNullOrEmpty(experience.Information))
                return BadRequest("Employee job UUID and information are required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _employeeJobExperienceService.SaveEmployeeJobExperience(basicModel!, experience);

            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving experience record.");

            return Ok(result);
        }

        /// <summary>
        /// Delete employee job experience record
        /// </summary>
        /// <param name="UUID">The UUID of the experience record to delete</param>
        /// <returns></returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteEmployeeJobExperience([FromBody] string UUID)
        {
            if (string.IsNullOrEmpty(UUID))
                return BadRequest("Invalid experience record selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _employeeJobExperienceService.DeleteEmployeeJobExperience(basicModel!, UUID);

            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting experience record.");

            return Ok(result);
        }
    }
}