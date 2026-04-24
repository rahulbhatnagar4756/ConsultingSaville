using Library.Goals.Models.ContractPeriods;
using Library.Goals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Administration.Periods
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractPeriodsController : BaseGoalController
    {
        private readonly IContractPeriodsService _contractPeriodsService;

        /// <summary>
        /// Constructor injecting contract periods service.
        /// </summary>
        /// <param name="contractPeriodsService">Service to manage contract periods</param>
        public ContractPeriodsController(IContractPeriodsService contractPeriodsService)
        {
            _contractPeriodsService = contractPeriodsService;
        }

        /// <summary>
        /// Retrieves all contract periods or a specific one if UUID is provided.
        /// GET: /api/Goals/ContractPeriods?contractPeriodUUID={uuid}
        /// </summary>
        /// <param name="contractPeriodUUID">Optional UUID to fetch a specific contract period</param>
        /// <returns>List of contract periods or a specific one</returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetContractPeriods()
        {
            // Validate user claims from JWT and extract company/user UUIDs
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;
            // Call service to retrieve contract periods
            var result = await _contractPeriodsService.GetContractPeriodsAsync(basicModel);
            // Return 500 if service failed
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving contract periods.");

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a single contract period by its UUID.
        /// GET: /api/Goals/ContractPeriods/{uuid}
        /// </summary>
        /// <param name="uuid">UUID of the contract period to fetch</param>
        /// <returns>The contract period if found; otherwise, 404</returns>
        [HttpGet("{uuid}"), Authorize]
        public async Task<IActionResult> GetById([FromRoute] string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest("Invalid contract period UUID.");

            // Validate user claims
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null)
                return errorResult;

            // Retrieve the specific contract period by UUID using the dedicated service method
            var contractPeriod = await _contractPeriodsService.GetContractPeriodByIdAsync(basicModel, uuid);

            if (contractPeriod == null)
                return NotFound($"Contract period with UUID '{uuid}' not found.");

            return Ok(contractPeriod);
        }

        /// <summary>
        /// Saves a contract period (create or update).
        /// POST: /api/Goals/ContractPeriods/Save
        /// </summary>
        /// <param name="contractPeriod">The contract period data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveContractPeriod([FromBody] ContractPeriodDto contractPeriod)
        {
            if (contractPeriod == null)
                return BadRequest("Invalid contract period data.");
            // Validate user claims
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;
            // Call service to save the contract period
            var result = await _contractPeriodsService.SaveContractPeriodAsync(basicModel, contractPeriod);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving contract period.");

            return Ok(result);
        }

        /// <summary>
        /// Deletes (soft deletes) a contract period by UUID.
        /// POST: /api/Goals/ContractPeriods/Delete
        /// </summary>
        /// <param name="uuid">UUID of the contract period to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteContractPeriod([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return BadRequest("Invalid contract period selected.");
            // Validate user claims
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;
            // Call service to delete the contract period
            var result = await _contractPeriodsService.DeleteContractPeriodAsync(basicModel, uuid);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting contract period.");

            return Ok(result);
        }
    }
}
