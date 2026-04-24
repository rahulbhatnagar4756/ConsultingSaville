using Library.Base.Services;
using Library.Database.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private ISqlDataAccess _sql;
    private Library.Base.Services.ICompanyService _companyService;

    public CompanyController(ISqlDataAccess sql, ICompanyService companyService)
    {
        _sql = sql;
        _companyService = companyService;
    }

    [HttpGet("CompanyByUUID/{CompanyUUID}"), Authorize]
    public async Task<IActionResult> GetCompanyInformation(string CompanyUUID)
    {
        var userClaims = User.Claims;
        if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

        string CompanyUUID2 = User.FindFirst("Companyid")?.Value;
        string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

        if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);

        var data = await _companyService.GetCompanyByUUID(CompanyUUID, UsersUUIDLoggedIn);
        if (data == null) return StatusCode(StatusCodes.Status204NoContent);
        return Ok(data);
    }

}
