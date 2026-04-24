using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Services.Employees.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers.BusinessStructure.Jobs;

[Route("api/Employee/[controller]")]
[ApiController]
public class PositionsController : ControllerBase
{

    private IEmployeeJobsService _employeeJobs;

    public PositionsController(IEmployeeJobsService employeejobs)
    {
        _employeeJobs = employeejobs;
    }

    [HttpGet("All"), Authorize]
    public async Task<IActionResult> GetEmployeeJobs()
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
        string CompanyUUID = User.FindFirst("CompanyUUID")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("UsersUUID")?.Value;


        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
            return StatusCode(StatusCodes.Status409Conflict);

        var data = await _employeeJobs.GetEmployeeJobs(new EmployeeBasicGetModel
        {
            CompanyUUID = CompanyUUID,
            UsersUUIDLoggedIn = UsersUUIDLoggedIn
        });
        if (data == null) return StatusCode(StatusCodes.Status204NoContent);

        return Ok(data);
    }

    [HttpPost("Save"), Authorize]
    public async Task<IActionResult> SaveEmployeeJobs([FromBody] Library.Base.Models.Employees.Jobs.EmployeeJobsBaseModel model)
    {
        if (model == null) return BadRequest("Invalid job data.");
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
        var result = await _employeeJobs.SaveEmployeeJobs(LoggedInUser, model);

        if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving job.");
        return Ok(result);
    }

    [HttpPost("Delete"), Authorize]
    public async Task<IActionResult> DeleteEmployeeJobs([FromBody] DeleteModel delete)
    {
        if (delete == null || string.IsNullOrEmpty(delete.UUID)) return BadRequest("Invalid job selected.");
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
        string CompanyUUID = User.FindFirst("Companyid")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;
        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn))
            return StatusCode(StatusCodes.Status409Conflict);
        delete.CompanyUUID = CompanyUUID;
        delete.UsersUUIDLoggedIn = UsersUUIDLoggedIn;
        var result = await _employeeJobs.DeleteEmployeeJobs(new EmployeeBasicGetModel
        {
            CompanyUUID = CompanyUUID,
            UsersUUIDLoggedIn = UsersUUIDLoggedIn
        }, delete.UUID);
        
        if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting job.");
        return Ok(result);
    }

}
