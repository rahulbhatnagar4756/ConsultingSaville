using Library.Base.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers
{
    [Route("api/Users/")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRolesService _rolesService;

        public UserRolesController(IUserRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet("RolesByUsersUUID/{UsersUUID}"), Authorize]
        public async Task<IActionResult> GetUserRolesByUsersUUID([FromBody] string usersUUID)
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            var data = await _rolesService.GetRolesByUsersUUID(CompanyUUID, UsersUUIDLoggedIn, usersUUID);
            if (data == null) return NotFound();
            return Ok(data);
        }

    }
}
