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
    public class EmployeeJobEducationController : BaseEmployeeController
    {
        private readonly IEmployeeJobEducationService _employeeJobEducationService;

        public EmployeeJobEducationController(IEmployeeJobEducationService employeeJobEducationService)
        {
            _employeeJobEducationService = employeeJobEducationService;
        }

        /// <summary>
        /// Get employee job education records by job UUID
        /// </summary>
        /// <param name="employeeJobsUUID">The UUID of the employee job to get education for</param>
        /// <returns></returns>
        [HttpGet("All/{employeeJobsUUID}"), Authorize]
        public async Task<IActionResult> GetEmployeeJobEducationByJobUUID(string employeeJobsUUID)
        {
            if (string.IsNullOrEmpty(employeeJobsUUID)) 
                return BadRequest("Employee job UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var data = await _employeeJobEducationService.GetEmployeeJobEducationByJobUUID(basicModel!, employeeJobsUUID);
            
            if (data == null) return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

        /// <summary>
        /// Save employee job education record
        /// </summary>
        /// <param name="education">The education record to save</param>
        /// <returns></returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveEmployeeJobEducation([FromBody] EmployeeJobEducationBaseModel education)
        {
            if (education == null) 
                return BadRequest("Invalid education data.");

            if (string.IsNullOrEmpty(education.EmployeeJobsUUID) || string.IsNullOrEmpty(education.Information))
                return BadRequest("Employee job UUID and information are required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _employeeJobEducationService.SaveEmployeeJobEducation(basicModel!, education);

            if (result == null) 
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving education record.");
            
            return Ok(result);
        }

        /// <summary>
        /// Delete employee job education record
        /// </summary>
        /// <param name="deleteModel">The delete model containing the UUID to delete</param>
        /// <returns></returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteEmployeeJobEducation([FromBody] string UUID)
        {
            if (string.IsNullOrEmpty(UUID)) 
                return BadRequest("Invalid education record selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _employeeJobEducationService.DeleteEmployeeJobEducation(basicModel!, UUID);

            if (result == null) 
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting education record.");
            
            return Ok(result);
        }

        /// <summary>
        /// Alternative delete method that accepts just the UUID as a string
        /// </summary>
        /// <param name="uuid">The UUID of the education record to delete</param>
        /// <returns></returns>
        [HttpDelete("{uuid}"), Authorize]
        public async Task<IActionResult> DeleteEmployeeJobEducationByUUID(string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) 
                return BadRequest("Education record UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _employeeJobEducationService.DeleteEmployeeJobEducation(basicModel!, uuid);

            if (result == null) 
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting education record.");
            
            return Ok(result);
        }
    }
}
