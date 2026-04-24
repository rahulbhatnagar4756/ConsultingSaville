using Library.Base.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers.Users
{
    [Route("api/Users/Data")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataService _usersDataService;
        private string CompanyUUID = "";
        private string UsersUUIDLoggedIn = "";
        private bool isClaimsSuccess = false;

        public UserDataController(IUserDataService usersDataService)
        {
            _usersDataService = usersDataService;
        }

        private void GetCompanyUserFromClaims()
        {
            isClaimsSuccess = false;
            if (User == null) return;
            var userClaims = User.Claims;
            if (userClaims == null) return;

            isClaimsSuccess = true;
            CompanyUUID = User.FindFirst("Companyid")?.Value ?? "";
            UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value ?? "";
        }

        [HttpGet("EmployeeInformation/{UsersUUID}"), Authorize]
        public async Task<IActionResult> GetUserByUUID(string UsersUUID)
        {
            GetCompanyUserFromClaims();
            if (!isClaimsSuccess) return StatusCode(StatusCodes.Status401Unauthorized);
            var data = await _usersDataService.GetUserEmployeeInformationByUsersUUID(CompanyUUID, UsersUUIDLoggedIn, UsersUUID);
            if (data == null) return NotFound();
            return Ok(data);
        }

      

    }
}
