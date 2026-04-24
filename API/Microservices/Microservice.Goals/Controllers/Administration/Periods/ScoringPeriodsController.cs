using Library.Goals.Models.ScoringPeriod;
using Library.Goals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Administration.Periods
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    [Authorize]
    public class ScoringPeriodsController : BaseGoalController
    {
        private readonly IScoringPeriodsService _scoringPeriodsService;

        public ScoringPeriodsController(IScoringPeriodsService scoringPeriodsService)
        {
            _scoringPeriodsService = scoringPeriodsService;
        }

        /// <summary>
        /// Get all scoring periods (Annual, Mid-Year, Custom) for admin configuration
        /// </summary>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetScoringPeriods()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _scoringPeriodsService.GetScoringPeriodsAsync(basicModel);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving scoring periods.");

            return Ok(result);
        }

        /// <summary>
        /// Create or update scoring period (name, description, start/end month-day)
        /// </summary>
        [HttpPost("Create"), Authorize]
        public async Task<IActionResult> CreateScoringPeriod([FromBody] ScoringPeriodDto scoringPeriod)
        {
            if (scoringPeriod == null)
                return BadRequest("Invalid scoring period data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _scoringPeriodsService.CreateScoringPeriodAsync(basicModel, scoringPeriod);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating scoring period.");

            return Ok(result);
        }

        /// <summary>
        /// Delete scoring period (soft delete)
        /// </summary>
        [HttpPost("{uuid}"), Authorize]
        public async Task<IActionResult> DeleteScoringPeriod([FromRoute] string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest("Scoring period UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _scoringPeriodsService.DeleteScoringPeriodAsync(basicModel, uuid);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting scoring period.");

            return Ok(result);
        }
    }
}
