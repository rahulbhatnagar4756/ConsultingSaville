using Library.TalentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.TalentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuccessionReadinessController : ControllerBase
    {
        private readonly IBox9Service _box9Service;
        public SuccessionReadinessController(IBox9Service box9Service)
        {
            _box9Service = box9Service;
        }

        [HttpPost("RoleInformation"), Authorize]
        public async Task<IActionResult> Search([FromBody] Library.TalentManagement.Models.UUIDSearchModel UUID)
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);

            var result = await _box9Service.GetSuccessionReadinessRoleInformation(CompanyUUID, UsersUUIDLoggedIn, UUID?.UUID??"", UUID?.Search??null);
            //if no data return no data error
            if (result == null) return StatusCode(StatusCodes.Status204NoContent);
            return Ok(result);
        }
    }
}
