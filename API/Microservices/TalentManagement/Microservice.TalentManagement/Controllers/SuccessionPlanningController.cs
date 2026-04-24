using Library.TalentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.TalentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuccessionPlanningController : ControllerBase
    {
        private readonly IBox9Service _box9Service;

        public SuccessionPlanningController(IBox9Service box9Service)
        {
            _box9Service = box9Service;
        }

        [HttpPost("Search"), Authorize]
        public async Task<IActionResult> Search([FromBody] Library.TalentManagement.Models.SearchModel search)
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
            var result = await _box9Service.GetSuccessionPlannings(CompanyUUID, UsersUUIDLoggedIn, search);

            //if no data return no data error
            if(result == null || result.Count() == 0  ) return StatusCode(StatusCodes.Status204NoContent);

            return Ok(result);
        }

        //can not transmit a DataTable case the it cant be serialized
        
        //[HttpPost("Search/Return/DataTable"), Authorize]
        //public async Task<IActionResult> SearchReturnDataTable([FromBody] Library.TalentManagement.Models.SearchModel search)
        //{
        //    var userClaims = User.Claims;
        //    if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
        //    string CompanyUUID = User.FindFirst("Companyid")?.Value;
        //    string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

        //    if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
        //    var result = await _box9Service.GetSuccessionPlanningsToDataTable(CompanyUUID, UsersUUIDLoggedIn, search);

        //    //if no data return no data error
        //    if (result == null || result.Rows.Count == 0) return StatusCode(StatusCodes.Status204NoContent);

        //    return Ok(result);
        //}

        [HttpPost("Search/Return/Raw"), Authorize]
        public async Task<IActionResult> SearchReturnRaw([FromBody] Library.TalentManagement.Models.SearchModel search)
        {
            var userClaims = User.Claims;
            if (userClaims == null) return StatusCode(StatusCodes.Status401Unauthorized);
            string CompanyUUID = User.FindFirst("Companyid")?.Value;
            string UsersUUIDLoggedIn = User.FindFirst("Usersid")?.Value;

            if (string.IsNullOrEmpty(CompanyUUID) || string.IsNullOrEmpty(UsersUUIDLoggedIn)) return StatusCode(StatusCodes.Status409Conflict);
            var result = await _box9Service.GetSuccessionPlanningsToRaw(CompanyUUID, UsersUUIDLoggedIn, search);

            //if no data return no data error
            if (result == null || result.Count() == 0) return StatusCode(StatusCodes.Status204NoContent);

            return Ok(result);
        }

    }
}
