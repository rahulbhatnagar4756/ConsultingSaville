using Library.Goals.Models;
using Library.Goals.Models.KPI;
using Library.Goals.Services.Contracts;
using Microservice.Base.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Contracts
{
    [Route("api/Goals/Contracts/[controller]")]
    [ApiController]
    public class KPIController : BaseGoalController
    {
        private readonly ISecureService _secureService;
        private readonly IKPIService _kpiService;

        public KPIController(ISecureService secureService, IKPIService kpiService)
        {
            _secureService = secureService;
            _kpiService = kpiService;
        }

        /// <summary>
        /// Save a KPI (create or update)
        /// </summary>
        /// <param name="saveModel">KPI save model</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> Save([FromBody] KPISaveModel saveModel)
        {
            try
            {
                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                // Set the user context from claims
                saveModel.CompanyUUID = basicModel.CompanyUUID;
                saveModel.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;

                var result = await _kpiService.Save(saveModel);

                if (result?.isValid == true)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented,
                    new ResultsModel { UUID = null, isValid = false, Message = $"Internal server error: {ex.Message}" });
            }
        }

        /// <summary>
        /// Delete a KPI
        /// </summary>
        /// <param name="uuid">UUID of the KPI to delete</param>
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

                var result = await _kpiService.Delete(basic, uuid);

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