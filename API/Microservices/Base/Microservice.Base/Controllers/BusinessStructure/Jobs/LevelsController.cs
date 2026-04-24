using Library.Base.Models.Employees;
using Library.Base.Services.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers.BusinessStructure.Jobs
{
    [Route("api/Employee/[controller]")]
    [ApiController]
    public class LevelsController : BaseEmployeeController
    {
       
        private IEmployeeLevelsService _employeeLevels;

        public LevelsController(IEmployeeLevelsService employeeLevels)
        {
            _employeeLevels = employeeLevels;
        }

        [HttpGet("All"), Authorize]
        public async Task<IActionResult> GetEmployeeBusinessUnit()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var data = await _employeeLevels.GetEmployeeLevels(basicModel);
            if (data == null) return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveEmployeeBusinessUnit([FromBody] EmployeeLevelBaseModel levels)
        {
            if (levels == null) return BadRequest("Invalid level data.");

            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
                return StatusCode(StatusCodes.Status409Conflict);
            EmployeeBasicGetModel LoggedInUser = new EmployeeBasicGetModel
            {
                CompanyUUID = CompanyUUID,
                UsersUUIDLoggedIn = UsersUUIDLoggedIn
            };

            var result = await _employeeLevels.SaveEmployeeLevels(LoggedInUser, levels);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving level.");
            return Ok(result);
        }

        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteEmployeeBusinessUnit([FromBody] string UUID)
        {
            if (string.IsNullOrEmpty(UUID)) return BadRequest("Invalid business unit selected.");
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;
            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
                return StatusCode(StatusCodes.Status409Conflict);
            EmployeeBasicGetModel LoggedInUser = new EmployeeBasicGetModel
            {
                CompanyUUID = CompanyUUID,
                UsersUUIDLoggedIn = UsersUUIDLoggedIn
            };
            var result = await _employeeLevels.DeleteEmployeeLevels(LoggedInUser, UUID);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting level.");
            return Ok(result);
        }
    }
}
