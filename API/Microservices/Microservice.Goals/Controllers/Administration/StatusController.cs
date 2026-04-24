using Library.Goals.Models;
using Library.Goals.Models.Status;
using Library.Goals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Administration
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusController : BaseGoalController
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        /// <summary>
        /// Get all status records for the authenticated user's company
        /// </summary>
        /// <returns>List of status records</returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetStatus()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _statusService.GetStatus(basicModel);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving status records.");

            return Ok(result);
        }

        /// <summary>
        /// Save status (create or update)
        /// </summary>
        /// <param name="status">Status data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveStatus([FromBody] StatusModel status)
        {
            if (status == null) return BadRequest("Invalid status data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _statusService.SaveStatus(basicModel, status);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving status.");

            return Ok(result);
        }

        /// <summary>
        /// Delete status (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the status to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteStatus([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return BadRequest("Invalid status selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _statusService.DeleteStatus(basicModel, uuid);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting status.");

            return Ok(result);
        }

        /// <summary>
        /// Get all status ranges for a specific status
        /// </summary>
        /// <param name="statusUUID">UUID of the status</param>
        /// <returns>List of status ranges</returns>
        [HttpGet("Ranges/{statusUUID}"), Authorize]
        public async Task<IActionResult> GetStatusRanges(string statusUUID)
        {
            if (string.IsNullOrEmpty(statusUUID)) return BadRequest("Invalid status UUID.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _statusService.GetStatusRanges(basicModel, statusUUID);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving status ranges.");

            return Ok(result);
        }

        /// <summary>
        /// Save status range (create or update)
        /// </summary>
        /// <param name="statusUUID">UUID of the status</param>
        /// <param name="range">Status range data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Ranges/{statusUUID}/Save"), Authorize]
        public async Task<IActionResult> SaveStatusRange(string statusUUID, [FromBody] StatusRangesModel range)
        {
            if (string.IsNullOrEmpty(statusUUID) || range == null)
                return BadRequest("Invalid status range data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _statusService.SaveStatusRange(basicModel, range, statusUUID);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving status range.");

            return Ok(result);
        }

        /// <summary>
        /// Delete status range (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the status range to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Ranges/Delete"), Authorize]
        public async Task<IActionResult> DeleteStatusRange([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return BadRequest("Invalid status range selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _statusService.DeleteStatusRange(basicModel, uuid);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting status range.");

            return Ok(result);
        }
    }
}
