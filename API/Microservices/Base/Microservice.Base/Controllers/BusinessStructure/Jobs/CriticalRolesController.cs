using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Base.Services.Employees;
using Library.Base.Services.Employees.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers.BusinessStructure.Jobs
{
    [Route("api/Employee/[controller]")]
    [ApiController]
    public class CriticalRolesController : ControllerBase
    {
        private readonly IEmployeeJobsCriticalRolesService _employeeJobsCriticalRolesService;

        public CriticalRolesController(IEmployeeJobsCriticalRolesService employeeJobsCriticalRolesService)
        {
            _employeeJobsCriticalRolesService = employeeJobsCriticalRolesService;
        }

        [HttpGet("All"), Authorize]
        public async Task<IActionResult> GetCriticalRoles()
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
                return StatusCode(StatusCodes.Status409Conflict);

            var data = await _employeeJobsCriticalRolesService.GetEmployeeJobsCriticalRoles(new EmployeeBasicGetModel
            {
                CompanyUUID = CompanyUUID,
                UsersUUIDLoggedIn = UsersUUIDLoggedIn
            });

            if (data == null) return StatusCode(StatusCodes.Status204NoContent);
            return Ok(data);
        }

        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveCriticalRole([FromBody] EmployeeJobsCriticalRoleBaseModel criticalRole)
        {
            if (criticalRole == null) return BadRequest("Invalid critical role data.");

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

            var result = await _employeeJobsCriticalRolesService.SaveEmployeeJobsCriticalRoles(loggedInUser, criticalRole);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving critical role.");

            return Ok(result);
        }

        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteCriticalRole([FromBody] string UUID)
        {
            if (string.IsNullOrEmpty(UUID)) return BadRequest("Invalid critical role selected.");

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

            var result = await _employeeJobsCriticalRolesService.DeleteEmployeeJobsCriticalRoles(loggedInUser, UUID);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting critical role.");

            return Ok(result);
        }
    }
}
