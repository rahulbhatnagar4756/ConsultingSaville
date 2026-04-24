using Library.Goals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers;
 
/// <summary>
/// Base controller for employee-related API endpoints that require user authentication
/// </summary>
public abstract class BaseGoalController : ControllerBase
{
    /// <summary>
    /// Validates user claims and extracts company and user UUIDs
    /// </summary>
    /// <returns>EmployeeBasicGetModel if valid, or IActionResult with error status if invalid</returns>
    protected (BasicModel? basicModel, IActionResult? errorResult) ValidateUserClaims()
    {
        var userClaims = User.Claims;
        if (userClaims == null)
            return (null, StatusCode(StatusCodes.Status401Unauthorized));

        string? companyUUID = User.FindFirst("Companyid")?.Value;
        if(companyUUID == null) companyUUID = User.FindFirst("CompanyUUID")?.Value;
        string? usersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;
        if(usersUUIDLoggedIn == null) usersUUIDLoggedIn = User.FindFirst("UsersUUID")?.Value;

        if (string.IsNullOrEmpty(companyUUID) || string.IsNullOrEmpty(usersUUIDLoggedIn))
            return (null, StatusCode(StatusCodes.Status409Conflict));

        var basicModel = new BasicModel
        {
            CompanyUUID = companyUUID,
            UsersUUIDLoggedIn = usersUUIDLoggedIn
        };

        return (basicModel, null);
    }
}
