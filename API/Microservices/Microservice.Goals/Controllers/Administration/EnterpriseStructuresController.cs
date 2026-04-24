using Library.Goals.Models;
using Library.Goals.Models.EnterpriseStructures;
using Library.Goals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Administration
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    [Authorize]
    public class EnterpriseStructuresController : BaseGoalController
    {
        private readonly IEnterpriseStructuresService _enterpriseStructuresService;

        public EnterpriseStructuresController(IEnterpriseStructuresService enterpriseStructuresService)
        {
            _enterpriseStructuresService = enterpriseStructuresService;
        }

        /// <summary>
        /// Get all enterprise structure types for the authenticated user's company
        /// </summary>
        /// <returns>List of enterprise structure types</returns>
        [HttpGet("Types"), Authorize]
        public async Task<IActionResult> GetEnterpriseStructureTypes()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _enterpriseStructuresService.GetEnterpriseStructureTypes(basicModel);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving enterprise structure types.");

            return Ok(result);
        }

        /// <summary>
        /// Save enterprise structure type (create or update)
        /// </summary>
        /// <param name="enterpriseStructureType">Enterprise structure type data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Types/Save"), Authorize]
        public async Task<IActionResult> SaveEnterpriseStructureType([FromBody] EnterpriseStructureTypesModel enterpriseStructureType)
        {
            if (enterpriseStructureType == null) return BadRequest("Invalid enterprise structure type data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _enterpriseStructuresService.SaveEnterpriseStructureType(basicModel, enterpriseStructureType);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving enterprise structure type.");

            return Ok(result);
        }

        /// <summary>
        /// Delete enterprise structure type (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the enterprise structure type to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Types/Delete"), Authorize]
        public async Task<IActionResult> DeleteEnterpriseStructureType([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return BadRequest("Invalid enterprise structure type selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _enterpriseStructuresService.DeleteEnterpriseStructureType(basicModel, uuid);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting enterprise structure type.");

            return Ok(result);
        }


        /// <summary>
        /// Get all enterprise structure weights for the authenticated user's company
        /// </summary>
        /// <returns>List of enterprise structure weights</returns>
        [HttpGet("Weights"), Authorize]
        public async Task<IActionResult> GetEnterpriseStructureWeights()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _enterpriseStructuresService.GetEnterpriseStructureWeights(basicModel);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving enterprise structure weights.");

            return Ok(result);
        }

        /// <summary>
        /// Save enterprise structure weight (create or update)
        /// </summary>
        /// <param name="enterpriseStructureWeight">Enterprise structure weight data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Weights/Save"), Authorize]
        public async Task<IActionResult> SaveEnterpriseStructureWeight([FromBody] EnterpriseStructureWeightsModel enterpriseStructureWeight)
        {
            if (enterpriseStructureWeight == null) return BadRequest("Invalid enterprise structure weight data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _enterpriseStructuresService.SaveEnterpriseStructureWeight(basicModel, enterpriseStructureWeight);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error saving enterprise structure weight.");

            return Ok(result);
        }

        /// <summary>
        /// Delete enterprise structure weight (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the enterprise structure weight to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Weights/Delete"), Authorize]
        public async Task<IActionResult> DeleteEnterpriseStructureWeight([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return BadRequest("Invalid enterprise structure weight selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _enterpriseStructuresService.DeleteEnterpriseStructureWeight(basicModel, uuid);
            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting enterprise structure weight.");

            return Ok(result);
        }
    }
}