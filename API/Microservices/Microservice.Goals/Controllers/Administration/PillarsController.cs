using Library.Goals.Models;
using Library.Goals.Models.Pillars;
using Library.Goals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Administration
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    [Authorize]
    public class PillarsController : BaseGoalController
    {
        private readonly IPillarsService _pillarsService;

        public PillarsController(IPillarsService pillarsService)
        {
            _pillarsService = pillarsService;
        }

        /// <summary>
        /// Get all pillars for the authenticated user's company
        /// </summary>
        /// <returns>List of pillars</returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetPillars()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _pillarsService.GetPillars(basicModel);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving pillars.");

            return Ok(result);
        }

        /// <summary>
        /// Save pillar (create or update)
        /// </summary>
        /// <param name="pillar">Pillar data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SavePillar([FromBody] PillarsModel pillar)
        {
            if (pillar == null) return BadRequest("Invalid pillar data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _pillarsService.SavePillar(basicModel, pillar);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving pillar.");

            return Ok(result);
        }

        /// <summary>
        /// Delete pillar (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the pillar to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeletePillar([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return BadRequest("Invalid pillar selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _pillarsService.DeletePillar(basicModel, uuid);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting pillar.");

            return Ok(result);
        }
    }
}
