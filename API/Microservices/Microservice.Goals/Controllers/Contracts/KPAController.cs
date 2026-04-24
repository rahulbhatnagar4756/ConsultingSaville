using Library.Goals.Models;
using Library.Goals.Models.Contracts;
using Library.Goals.Models.KPA;
using Library.Goals.Services;
using Library.Goals.Services.Contracts;
using Microservice.Base.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Contracts
{
    [Route("api/Goals/Contracts/[controller]")]
    [ApiController]
    public class KPAController : BaseGoalController
    {
        private readonly ISecureService _secureService;
        private readonly IKPAService _kpaService;

        public KPAController(ISecureService secureService, IKPAService kpaService)
        {
            _secureService = secureService;
            _kpaService = kpaService;
        }

        /// <summary>
        /// Save a Contract KPA (create or update)
        /// </summary>
        /// <param name="saveModel">KPA save model</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> Save([FromBody] ContractKPASaveModel saveModel)
        {
            try
            {
                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                // Set the user context from claims
                saveModel.CompanyUUID = basicModel.CompanyUUID;
                saveModel.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;

                var result = await _kpaService.Save(saveModel);

                if (result?.isValid == true)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResultsModel { UUID = null, isValid = false, Message = $"Internal server error: {ex.Message}" });
            }
        }

        /// <summary>
        /// Delete a Contract KPA
        /// </summary>
        /// <param name="uuid">UUID of the KPA to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> Delete([FromBody] string uuid)
        {
            try
            {
                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var basic = new BasicModel
                {
                    CompanyUUID = basicModel.CompanyUUID,
                    UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn
                };

                var result = await _kpaService.DeleteKPA(basic, uuid);

                if (result?.isValid == true)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResultsModel { UUID = null, isValid = false, Message = $"Internal server error: {ex.Message}" });
            }
        }

    }
}
