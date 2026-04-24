using Library.Base.Models.Employees;
using Library.Base.Services.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers.BusinessStructure
{
    [Route("api/Employee/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private IEmployeeDepartmentsService _departments;

        public DepartmentsController(IEmployeeDepartmentsService departments)
        {
            _departments = departments;
        }

        [HttpGet("All"), Authorize]
        public async Task<IActionResult> GetEmployeeDepartments()
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
            string CompanyUUID = User.FindFirst("CompanyUUID")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("UsersUUID")?.Value;


            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
                return StatusCode(StatusCodes.Status409Conflict);

            var data = await _departments.GetEmployeeDepartments (new Library.Base.Models.Employees.EmployeeBasicGetModel
            {
                CompanyUUID = CompanyUUID,
                UsersUUIDLoggedIn = UsersUUIDLoggedIn
            });
            if (data == null) return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveEmployeeDepartments([FromBody] Library.Base.Models.Employees.BusinessStructure.EmployeeDepartmentBaseModel businessUnit)
        {
            if (businessUnit == null) return BadRequest("Invalid business unit data.");

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

            var result = await _departments.SaveEmployeeDepartments(LoggedInUser, businessUnit);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving department.");
            return Ok(result);
        }

        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteEmployeeDepartments([FromBody] string UUID)
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
            var result = await _departments.DeleteEmployeeDepartments(LoggedInUser, UUID);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting department.");
            return Ok(result);
        }
    }
}
