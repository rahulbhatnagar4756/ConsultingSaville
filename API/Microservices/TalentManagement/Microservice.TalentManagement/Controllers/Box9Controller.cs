using Library.TalentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.TalentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Box9Controller : ControllerBase
    {
        private readonly IBox9Service _box9Service;

        public Box9Controller(IBox9Service box9Service)
        {
            _box9Service = box9Service;
        }


        [HttpPost("Statistics"), Authorize]
        public async Task<IActionResult> Statistics([FromBody] Library.Base.Models.Employees.EmployeeSearchModel search)
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
            var result = await _box9Service.GetStatisticsFilter(CompanyUUID, UsersUUIDLoggedIn, search);
            return Ok(result);
        }


        [HttpGet("Statistics/User/{UsersUUID}"), Authorize]
        public async Task<IActionResult> StatisticsUser(string UsersUUID)
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;
            
            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
            var result = await _box9Service.GetStatisticsForUser(CompanyUUID, UsersUUIDLoggedIn, UsersUUID);
            return Ok(result);
        }

        [HttpGet("Statistics/UserInformation/{UsersUUID}"), Authorize]
        public async Task<IActionResult> TalentManagementUserInformation(string UsersUUID)
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);

            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
            var result = await _box9Service.GetTalentManagementUserInformation(CompanyUUID, UsersUUIDLoggedIn, UsersUUID);
            return Ok(result);
        }

    }
}
