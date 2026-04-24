using Library.Goals.Models;
using Library.Goals.Models.Contracts;
using Library.Goals.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers.Contracts
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    public class KPALinksController : BaseGoalController
    {
        private readonly IKPALinksService _kpaLinksService;

        public KPALinksController(IKPALinksService kpaLinksService)
        {
            _kpaLinksService = kpaLinksService;
        }

        /// <summary>
        /// Save KPA Link (create or update)
        /// </summary>
        /// <param name="saveModel">KPA Link save model</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> Save([FromBody] ContractKPALinksSaveModel saveModel)
        {
            if (saveModel == null)
                return BadRequest("Invalid KPA Link data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            // Set the company and user information from claims
            saveModel.CompanyUUID = basicModel.CompanyUUID;
            saveModel.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;

            var result = await _kpaLinksService.Save(saveModel);
            
            if (result == null)
                return StatusCode(500, "An error occurred while saving the KPA Link.");

            if (result.isValid == true)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("BulkSave"), Authorize]
        public async Task<IActionResult> BulkSave([FromBody] List<ContractKPALinksSaveModel> saveModels)
        {
            if (saveModels == null || !saveModels.Any())
                return BadRequest("KPA Link save models are required.");
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;
            // Set the company and user information from claims for each model
            foreach (var model in saveModels)
            {
                model.CompanyUUID = basicModel.CompanyUUID;
                model.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;
            }
            var results = await _kpaLinksService.BulkSave(saveModels);
            
            if (results == null)
                return StatusCode(500, "An error occurred while saving the KPA Links.");
            return Ok(results);
        }

        /// <summary>
        /// Delete KPA Link (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the KPA Link to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> Delete([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return BadRequest("KPA Link UUID is required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _kpaLinksService.Delete(basicModel, uuid);
            
            if (result == null)
                return StatusCode(500, "An error occurred while deleting the KPA Link.");

            if (result.isValid == true)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Change weight of an existing KPA Link
        /// </summary>
        /// <param name="request">Weight change request containing UUID and new weight</param>
        /// <returns>Result of the weight change operation</returns>
        [HttpPost("ChangeWeight"), Authorize]
        public async Task<IActionResult> ChangeWeight([FromBody] KPALinkWeightUpdateModel request)
        {
            if (request == null)
                return BadRequest("Weight change request is required.");

            if (string.IsNullOrEmpty(request.UUID))
                return BadRequest("KPA Link UUID is required.");

            if (request.Weight <= 0)
                return BadRequest("Weight must be greater than 0.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _kpaLinksService.ChangeWeight(basicModel, request);
            
            if (result == null)
                return StatusCode(500, "An error occurred while changing the KPA Link weight.");

            if (result.isValid == true)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Bulk update weights for multiple KPA Links
        /// </summary>
        /// <param name="weightUpdates">List of weight updates containing UUID and new weight</param>
        /// <returns>Results of the bulk update operation</returns>
        [HttpPost("BulkUpdateWeights"), Authorize]
        public async Task<IActionResult> BulkUpdateWeights([FromBody] List<KPALinkWeightUpdateModel> weightUpdates)
        {
            if (weightUpdates == null || !weightUpdates.Any())
                return BadRequest("Weight updates are required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var results = await _kpaLinksService.BulkUpdateWeights(basicModel, weightUpdates);
            
            if (results == null)
                return StatusCode(500, "An error occurred while updating KPA Link weights.");

            return Ok(results);
        }
    }

    
}
