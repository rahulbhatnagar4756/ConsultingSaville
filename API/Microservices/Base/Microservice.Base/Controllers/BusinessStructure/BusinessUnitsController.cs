using Library.Base.Models.Employees;
using Library.Base.Services.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers.BusinessStructure
{
    [Route("api/Employee/[controller]")]
    [ApiController]
    public class BusinessUnitsController : ControllerBase
    {
        private IEmployeeBusinessUnitsService _employeeBusinessUnitTypes;

        public BusinessUnitsController(IEmployeeBusinessUnitsService employeeBusinessUnitTypes)
        {
            _employeeBusinessUnitTypes = employeeBusinessUnitTypes;
        }

        [HttpGet("All"), Authorize]
        public async Task<IActionResult> GetEmployeeBusinessUnit()
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
            string CompanyUUID = User.FindFirst("CompanyUUID")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("UsersUUID")?.Value;


            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
                return StatusCode(StatusCodes.Status409Conflict);

            var data = await _employeeBusinessUnitTypes.GetEmployeeBusinessUnits(new Library.Base.Models.Employees.EmployeeBasicGetModel {
                                                                                                                CompanyUUID = CompanyUUID,
                                                                                                                UsersUUIDLoggedIn = UsersUUIDLoggedIn});
            if (data == null) return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveEmployeeBusinessUnit([FromBody] Library.Base.Models.Employees.BusinessStructure.EmployeeBaseModel businessUnit)
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

            var result = await _employeeBusinessUnitTypes.SaveEmployeeBusinessUnit(LoggedInUser, businessUnit);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving business unit.");
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
            var result = await _employeeBusinessUnitTypes.DeleteEmployeeBusinessUnit(LoggedInUser, UUID);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting business unit.");
            return Ok(result);
        }

    }
}
