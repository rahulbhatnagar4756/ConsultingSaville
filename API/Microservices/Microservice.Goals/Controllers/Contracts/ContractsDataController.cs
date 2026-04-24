using Library.Goals.Services.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Contracts
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    public class ContractsDataController : BaseGoalController
    {
        #region Fields
        private readonly IContractsDataService _contractsDataService;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor that injects the contract data service.
        /// </summary>
        /// <param name="contractsDataService">An instance of <see cref="IContractsDataService"/>.</param>
        public ContractsDataController(IContractsDataService contractsDataService)
        {
            _contractsDataService = contractsDataService;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Retrieves contract data for a given UUID.
        /// </summary>
        /// <param name="contractsUUID">The unique identifier for the contract.</param>
        /// <returns>An <see cref="IActionResult"/> containing the contract data or an error response.</returns>
        [HttpGet("{contractsUUID}")]
        public async Task<IActionResult> GetContractData(string contractsUUID)
        {
            // Validate user claims and retrieve the basic model and any error result
            var (basicModel, errorResult) = ValidateUserClaims();
            // If user claims are invalid, return the corresponding error result
            if (errorResult != null) return errorResult;
            // Call the service to retrieve contract data
            var contract = await _contractsDataService.GetContractData(contractsUUID, basicModel);
            // If contract not found or an error occurred, return 500 error
            if (contract == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Contract not found");
            // Return the retrieved contract data with 200 OK
            return Ok(contract);
        }

        #endregion
    }
}
