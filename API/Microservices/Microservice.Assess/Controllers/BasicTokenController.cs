using Library.Assess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers
{
   
    public class BasicTokenController : ControllerBase
    {
        /// <summary>
        /// Validates user claims and extracts company and user UUIDs
        /// </summary>
        /// <returns>EmployeeBasicGetModel if valid, or IActionResult with error status if invalid</returns>
        protected (BasicGetModel? basicModel, IActionResult? errorResult) ValidateUserClaims()
        {
            var userClaims = User.Claims;
            if (userClaims == null)
                return (null, StatusCode(StatusCodes.Status401Unauthorized));

            string? companyUUID = User.FindFirst("Companyid")?.Value ?? User.FindFirst("CompanyUUID")?.Value;
            string? usersUUIDLoggedIn = User.FindFirst("Usersid")?.Value ?? User.FindFirst("UsersUUID")?.Value;

            if (string.IsNullOrEmpty(companyUUID) || string.IsNullOrEmpty(usersUUIDLoggedIn))
                return (null, StatusCode(StatusCodes.Status409Conflict));

            var basicModel = new BasicGetModel
            {
                CompanyUUID = companyUUID,
                UsersUUIDLoggedIn = usersUUIDLoggedIn
            };

            return (basicModel, null);
        }
    }
     

}
