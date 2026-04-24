using Library.Goals.Models.Comments;
using Library.Goals.Services.Contracts;

using Microservice.Base.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Contracts
{
    /// <summary>
    /// API controller for contract results operations.
    /// </summary>
    [Route("api/Goals/[controller]")]
    [ApiController]
    public class ContractResultsController : BaseGoalController
    {
        private readonly IContractResultsService _contractResultsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractResultsController"/> class.
        /// </summary>
        /// <param name="secureService">Secure service for authentication/authorization</param>
        /// <param name="contractResultsService">Contract results service for data operations</param>
        public ContractResultsController(IContractResultsService contractResultsService)
        {
            _contractResultsService = contractResultsService;
        }

        /// <summary>
        /// Gets contract results with all nested structures (enterprise structures, pillars, KPAs, KPIs).
        /// </summary>
        /// <param name="contractsUUID">The UUID of the contract to retrieve</param>
        /// <param name="selectedUsersUUID?">The UUID of the Users to retrieve</param>
        /// <returns>Contract results response DTO with hierarchical data</returns>
        /// <response code="200">Returns the contract results</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="404">If the contract is not found</response>
        [HttpGet("{contractsUUID}/{selectedUsersUUID?}")]
        [Authorize]
        public async Task<IActionResult> GetContractResults(string contractsUUID, string? selectedUsersUUID=null)
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var results = await _contractResultsService.GetContractResults(basicModel, contractsUUID, selectedUsersUUID);

            if (results == null)
                return NotFound(new { message = "Contract results not found" });

            return Ok(results);
        }


        [HttpGet("GetRatingPeriods")]
        [Authorize]
        public async Task<IActionResult> GetRatingPeriods()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var results = await _contractResultsService.GetRatingPeriods(basicModel);

            if (results == null)
                return NotFound(new { message = "Rating Periods results not found" });

            return Ok(results);
        }


    }
}
