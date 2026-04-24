using Library.Goals.Models;
using Library.Goals.Models.Tolerances;
using Library.Goals.Services;
using Library.Goals.Mappers.Tolerances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Administration
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    [Authorize]
    public class TolerancesController : BaseGoalController
    {
        private readonly ITolerancesService _tolerancesService;

        public TolerancesController(ITolerancesService tolerancesService)
        {
            _tolerancesService = tolerancesService;
        }

        /// <summary>
        /// Get all tolerance sets for the authenticated user's company
        /// </summary>
        /// <returns>List of tolerance sets</returns>
        [HttpGet("Sets"), Authorize]
        public async Task<IActionResult> GetToleranceSets()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult; 

            var result = await _tolerancesService.GetToleranceSets(basicModel);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving tolerance sets.");

            return Ok(result);
        }

        /// <summary>
        /// Save tolerance set (create or update)
        /// </summary>
        /// <param name="tolerance">Tolerance set data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Sets/Save"), Authorize]
        public async Task<IActionResult> SaveToleranceSet([FromBody] ToleranceSetsModel tolerance)
        {
            if (tolerance == null) return BadRequest("Invalid tolerance set data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _tolerancesService.SaveToleranceSet(basicModel, tolerance);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving tolerance set.");

            return Ok(result);
        }

        /// <summary>
        /// Delete tolerance set (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the tolerance set to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Sets/Delete"), Authorize]
        public async Task<IActionResult> DeleteToleranceSet([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return BadRequest("Invalid tolerance set selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _tolerancesService.DeleteToleranceSet(basicModel, uuid);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting tolerance set.");

            return Ok(result);
        }

        /// <summary>
        /// Get all tolerance ranges for a specific tolerance set
        /// </summary>
        /// <param name="toleranceSetsUUID">UUID of the tolerance set</param>
        /// <returns>List of tolerance ranges</returns>
        [HttpGet("Ranges/{toleranceSetsUUID}"), Authorize]
        public async Task<IActionResult> GetToleranceRanges(string toleranceSetsUUID)
        {
            if (string.IsNullOrEmpty(toleranceSetsUUID)) return BadRequest("Invalid tolerance set UUID.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _tolerancesService.GetToleranceRanges(basicModel, toleranceSetsUUID);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving tolerance ranges.");

            return Ok(result);
        }

        /// <summary>
        /// Save tolerance range (create or update)
        /// </summary>
        /// <param name="toleranceSetsUUID">UUID of the tolerance set</param>
        /// <param name="range">Tolerance range data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Ranges/Save"), Authorize]
        public async Task<IActionResult> SaveToleranceRange([FromBody] ToleranceRangesModel range)
        {
            if (range == null) 
                return BadRequest("Invalid tolerance range data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _tolerancesService.SaveToleranceRange(basicModel, range);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving tolerance range.");

            return Ok(result);
        }

        /// <summary>
        /// Delete tolerance range (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the tolerance range to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Ranges/Delete"), Authorize]
        public async Task<IActionResult> DeleteToleranceRange([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return BadRequest("Invalid tolerance range selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _tolerancesService.DeleteToleranceRange(basicModel, uuid);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting tolerance range.");

            return Ok(result);
        }
    }
}
