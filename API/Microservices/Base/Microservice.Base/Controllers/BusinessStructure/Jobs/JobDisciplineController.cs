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
    public class JobDisciplineController : ControllerBase
    {
        private readonly IEmployeeJobDisciplinesService _employeeJobDisciplinesService;

        public JobDisciplineController(IEmployeeJobDisciplinesService employeeJobDisciplinesService)
        {
            _employeeJobDisciplinesService = employeeJobDisciplinesService;
        }

        /// <summary>
        /// Get all job disciplines for the logged-in user's company
        /// </summary>
        /// <returns></returns>
        [HttpGet("All"), Authorize]
        public async Task<IActionResult> GetJobDisciplines()
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
                return StatusCode(StatusCodes.Status409Conflict);

            var data = await _employeeJobDisciplinesService.GetEmployeeJobDisciplines(new EmployeeBasicGetModel
            {
                CompanyUUID = CompanyUUID,
                UsersUUIDLoggedIn = UsersUUIDLoggedIn
            });

            if (data == null) return StatusCode(StatusCodes.Status204NoContent);
            return Ok(data);
        }

        /// <summary>
        /// Save (create or update) a job discipline
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveJobDiscipline([FromBody] EmployeeJobDisciplineBaseModel discipline)
        {
            if (discipline == null) return BadRequest("Invalid discipline data.");

            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
                return StatusCode(StatusCodes.Status409Conflict);

            EmployeeBasicGetModel loggedInUser = new EmployeeBasicGetModel
            {
                CompanyUUID = CompanyUUID,
                UsersUUIDLoggedIn = UsersUUIDLoggedIn
            };

            var result = await _employeeJobDisciplinesService.SaveEmployeeJobDisciplines(loggedInUser, discipline);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving discipline.");

            return Ok(result);
        }

        /// <summary>
        /// Delete a job discipline by UUID
        /// </summary>
        /// <param name="UUID"></param>
        /// <returns></returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteJobDiscipline([FromBody] string UUID)
        {
            if (string.IsNullOrEmpty(UUID)) return BadRequest("Invalid discipline selected.");

            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
                return StatusCode(StatusCodes.Status409Conflict);

            EmployeeBasicGetModel loggedInUser = new EmployeeBasicGetModel
            {
                CompanyUUID = CompanyUUID,
                UsersUUIDLoggedIn = UsersUUIDLoggedIn
            };

            var result = await _employeeJobDisciplinesService.DeleteEmployeeJobDisciplines(loggedInUser, UUID);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting discipline.");

            return Ok(result);
        }
    }
}
