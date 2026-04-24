using Library.Base.Models.Users;
using Library.Base.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Base.Controllers.Users
{
    [Route("api/Users/Login")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserLoginService _userLoginService;
        private readonly ILogger<UserLoginController> _logger;

        public UserLoginController(IUserLoginService userLoginService, ILogger<UserLoginController> logger)
        {
            _userLoginService = userLoginService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user with username and password
        /// </summary>
        /// <param name="loginRequest">Login credentials</param>
        /// <returns>Authentication result with user and company information</returns>
        [HttpPost("LoginToken")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserLoginResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserLoginResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserLoginResponseModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginToken([FromBody] UserLoginRequestModel loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(new UserLoginResponseModel
                    {
                        IsSuccessful = false,
                        Message = string.Join("; ", errors)
                    });
                }

                _logger.LogInformation("Login attempt for username: {Username}", loginRequest.Username);

                var result = await _userLoginService.LoginReturnTokenAsync(
                    loginRequest.CompanyUUID,
                    loginRequest.Username,
                    loginRequest.Password);

                if (!result.IsSuccessful)
                {
                    return Unauthorized(result);
                }

                //return a token
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for username: {Username}", 
                    loginRequest?.Username);
                
                return StatusCode(StatusCodes.Status500InternalServerError, new UserLoginResponseModel
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred during authentication"
                });
            }
        }

        
    }
}