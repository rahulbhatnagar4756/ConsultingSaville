using Library.Assess.Models.PersonalityWheel;
using Library.Assess.Services.PersonalityWheel;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.PersonalityWheel
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    public class PersonalityWheelController : ControllerBase
    {
        private readonly IPersonalityWheelRepository _repository;
        private readonly ILogger<PersonalityWheelController> _logger;

        public PersonalityWheelController(IPersonalityWheelRepository repository, ILogger<PersonalityWheelController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Generates a personality wheel image and returns it as base64 string
        /// </summary>
        /// <param name="request">The personality wheel configuration</param>
        /// <returns>Base64 encoded PNG image</returns>
        [HttpPost("generate")]
        public async Task<ActionResult<PersonalityWheelResponse>> GenerateWheel([FromBody] PersonalityWheelRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.GenerateWheelImageAsync(request);

                if (!response.Success)
                {
                    _logger.LogError("Failed to generate personality wheel: {Error}", response.ErrorMessage);
                    return StatusCode(500, response);
                }

                _logger.LogInformation("Successfully generated personality wheel image");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error generating personality wheel");
                return StatusCode(500, new PersonalityWheelResponse
                {
                    Success = false,
                    ErrorMessage = "An unexpected error occurred"
                });
            }
        }
    }
}
