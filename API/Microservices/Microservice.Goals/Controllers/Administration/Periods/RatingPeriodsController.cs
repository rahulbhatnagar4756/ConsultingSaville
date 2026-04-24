using Library.Goals.Models.RatingPeriod;
using Library.Goals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Administration.Periods
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingPeriodsController : BaseGoalController
    {
        private readonly IRatingPeriodsService _ratingPeriodsService;

        public RatingPeriodsController(IRatingPeriodsService ratingPeriodsService)
        {
            _ratingPeriodsService = ratingPeriodsService;
        }

        [HttpGet("GetRatingPeriods"), Authorize]
        public async Task<IActionResult> GetRatingPeriods()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var periods = await _ratingPeriodsService.GetRatingPeriods(basicModel);
            return Ok(periods);
        }

        /// <summary>
        /// Get all rating period types (Month, Quarter, Year) for administrative setup
        /// GET: /api/Goals/RatingPeriods/Types
        /// </summary>
        [HttpGet("Types"), Authorize]
        public async Task<IActionResult> GetRatingPeriodTypes()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _ratingPeriodsService.GetRatingPeriodTypesAsync(basicModel);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving rating period types.");

            return Ok(result);
        }

        /// <summary>
        /// Create new rating period type (e.g., define "Quarter" with display "Qtr")
        /// POST: /api/Goals/RatingPeriods/Types/Create
        /// </summary>
        [HttpPost("Types/Create"), Authorize]
        public async Task<IActionResult> CreateRatingPeriodType([FromBody] RatingPeriodDto ratingPeriod)
        {
            if (ratingPeriod == null)
                return BadRequest("Invalid rating period type data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _ratingPeriodsService.CreateRatingPeriodTypeAsync(basicModel, ratingPeriod);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating rating period type.");

            return Ok(result);
        }

        /// <summary>
        /// Delete rating period type (soft delete)
        /// DELETE: /api/Goals/RatingPeriods/Types/{uuid}
        /// </summary>
        [HttpPost("Types/{uuid}"), Authorize]
        public async Task<IActionResult> DeleteRatingPeriodType([FromRoute] string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest("Rating period UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _ratingPeriodsService.DeleteRatingPeriodTypeAsync(basicModel, uuid);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting rating period type.");

            return Ok(result);
        }


        /// <summary>
        /// Get all rating period date instances, optionally filtered by period type
        /// GET: /api/Goals/RatingPeriods/Dates?periodTypeUUID={uuid}
        /// </summary>
        [HttpGet("Dates"), Authorize]
        public async Task<IActionResult> GetRatingPeriodDates([FromQuery] string? periodTypeUUID = null)
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _ratingPeriodsService.GetRatingPeriodDatesAsync(basicModel, periodTypeUUID);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving rating period dates.");

            return Ok(result);
        }

        /// <summary>
        /// Create/Update specific rating period date (e.g., "Q1 2025" with date ranges)
        /// POST: /api/Goals/RatingPeriods/Dates/Create
        /// Implements business rules for isActive based on DateClose
        /// </summary>
        [HttpPost("Dates/Create"), Authorize]
        public async Task<IActionResult> CreateRatingPeriodDate([FromBody] RatingPeriodDateDto ratingPeriodDate)
        {
            if (ratingPeriodDate == null)
                return BadRequest("Invalid rating period date data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _ratingPeriodsService.CreateRatingPeriodDateAsync(basicModel, ratingPeriodDate);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating rating period date.");

            return Ok(result);
        }

        /// <summary>
        /// Delete rating period date (soft delete)
        /// DELETE: /api/Goals/RatingPeriods/Dates/{uuid}
        /// </summary>
        [HttpPost("Dates/{uuid}"), Authorize]
        public async Task<IActionResult> DeleteRatingPeriodDate([FromRoute] string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest("Rating period date UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _ratingPeriodsService.DeleteRatingPeriodDateAsync(basicModel, uuid);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting rating period date.");

            return Ok(result);
        }

        /// <summary>
        /// Check if KPI rating is currently accessible - Used by KPI rating system
        /// GET: /api/Goals/RatingPeriods/Access/{ratingPeriodDateUUID}
        /// Core API for rating access control
        /// </summary>
        [HttpGet("Access/{ratingPeriodDateUUID}"), Authorize]
        public async Task<IActionResult> CheckRatingAccess([FromRoute] string ratingPeriodDateUUID)
        {
            if (string.IsNullOrWhiteSpace(ratingPeriodDateUUID))
                return BadRequest("Rating period date UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _ratingPeriodsService.CheckRatingAccessAsync(basicModel, ratingPeriodDateUUID);
            return Ok(result);
        }

        /// <summary>
        /// Activate or deactivate a rating period date
        /// POST: /api/Goals/RatingPeriods/Dates/{uuid}/isActive/{isActive}
        /// </summary>
        [HttpPost("Dates/{uuid}/{isActive}"), Authorize]
        public async Task<IActionResult> UpdateRatingPeriodDateIsActive([FromRoute] string uuid, [FromRoute] bool isActive)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest("Rating period date UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _ratingPeriodsService.UpdateRatingPeriodDateIsActiveAsync(basicModel, uuid, isActive);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating rating period date status.");

            return Ok(result);
        }

    }
}
