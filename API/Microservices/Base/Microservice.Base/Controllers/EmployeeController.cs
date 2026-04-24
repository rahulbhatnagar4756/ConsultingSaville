using Library.Base.Models.Employees;
using Library.Base.Services;
using Library.Database.DAL;
using Library.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microservice.Base.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private  ISqlDataAccess _sql; 
    private Library.Base.Services.IEmployeeService _employeeService;
    private IEmployeeHierarchyService _employeeHierarchyService;

    public EmployeeController(ISqlDataAccess sql, IEmployeeService employeeService,IEmployeeHierarchyService employeeHierarchyService)
    {
        _sql = sql; 
        _employeeService = employeeService;
        _employeeHierarchyService = employeeHierarchyService;
    }

   
    [HttpGet("EmployeeCriteria"), Authorize]
    public async Task<IActionResult> AllEmployeeCriteria()
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

        string CompanyUUID = User.FindFirst("CompanyUUID")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("UsersUUID")?.Value;

        if(string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);

        var data = await _employeeService.GetAllSearchCriteria(new Library.Base.Models.Employees.EmployeeBasicGetModel { CompanyUUID = CompanyUUID, UsersUUIDLoggedIn = UsersUUIDLoggedIn });
        if(data == null) return StatusCode(StatusCodes.Status204NoContent);
        return Ok(data);
    }

    [HttpPost("Search"), Authorize]
    public async Task<IActionResult> EmployeeSearch([FromBody] EmployeeSearchModel search)
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

        string CompanyUUID = User.FindFirst("Companyid")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;
        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);

        var data = await _employeeService.SearchEmployees(CompanyUUID, UsersUUIDLoggedIn, search);
        if (data == null) return StatusCode(StatusCodes.Status204NoContent);
        return Ok(data);
    }


    [HttpGet("BasicInformation/{UserUUID}"), Authorize]
    public async Task<IActionResult> EmployeeBasicInformation(string userUUID)
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
        string CompanyUUID = User.FindFirst("CompanyUUID")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("UsersUUID")?.Value;
        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
        var data = await _employeeService.GetEmployeeBasicInformation(CompanyUUID, UsersUUIDLoggedIn, userUUID);
        if (data == null) return StatusCode(StatusCodes.Status204NoContent);
        return Ok(data);
    }

    [HttpPost("SaveBasicInformation"), Authorize]
    public async Task<IActionResult> SaveEmployeeBasicInformation(EmployeeBasicInformationModel employeeInformation)
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
        string CompanyUUID = User.FindFirst("Companyid")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
        var data = await _employeeService.SetEmployeeBasicInformation(employeeInformation);
        if (data == null) return StatusCode(StatusCodes.Status204NoContent);
        return Ok(data);
    }

    [HttpPost("Hierarchy"), Authorize]
    public async Task<IActionResult> SaveEmployeeHierarchy(EmployeeHierarchyBasicModel employeeHierarchy)
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
        string CompanyUUID = User.FindFirst("CompanyUUID")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("UsersUUID")?.Value;

        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
        var data = await _employeeHierarchyService.SetEmployeeHierarchy(employeeHierarchy);
        if (data == null) return StatusCode(StatusCodes.Status204NoContent);
        return Ok(data);
    }

    [HttpDelete("DeleteHierarchy/{EmployeeHierarchyUUID}"), Authorize]
    public async Task<IActionResult> DeleteEmployeeHierarchy(string EmployeeHierarchyUUID)
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
        string CompanyUUID = User.FindFirst("Companyid")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;
        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
        var data = await _employeeHierarchyService.DeleteEmployeeHierarchy(CompanyUUID, UsersUUIDLoggedIn, EmployeeHierarchyUUID);
        if (data == null) return StatusCode(StatusCodes.Status204NoContent);
        return Ok(data);
    }

}


