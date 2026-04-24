using Library.Goals.Models.EmployeeResult;
using Library.Goals.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers
{
    [Route("api/Goals/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : BaseGoalController
    {
        // field for accessing employee-related service logic
        private readonly IEmployeesService _employeesService;

        /// <summary>
        /// Constructor injecting employees service.
        /// </summary>
        /// <param name="employeesService">Service to manage employees</param>
        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        /// <summary>
        /// Retrieves a list of all employees for the authenticated user's company context.
        /// </summary>
        /// <remarks>
        /// Endpoint: GET /api/Goals/Employees
        /// </remarks>
        /// <returns>
        /// HTTP 200 OK with list of employees if successful; 
        /// HTTP 500 Internal Server Error if the data retrieval fails.
        /// </returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetEmployees()
        {
            // Extract basic model and any error using base controller's method (e.g., validates JWT claims)
            var (basicModel, errorResult) = ValidateUserClaims();
            // If there was an error validating the user, return the error response
            if (errorResult != null) return errorResult;
            // Retrieve employee list from the service layer
            var result = await _employeesService.GetEmployeesAsync(basicModel);
            // If the result is null (error occurred in service or data access), return 500 Internal Server Error
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving employees.");
            // On success, return HTTP 200 OK with the employee list
            return Ok(result);
        }

        /// <summary>
        /// Checks whether a given user (by UsersUUID) is a manager or not.
        /// </summary>
        /// <remarks>
        /// Endpoint: GET /api/Goals/Employees/IsManager/{usersUUID}
        /// </remarks>
        /// <param name="usersUUID">The UUID of the user to check.</param>
        /// <returns>
        /// HTTP 200 OK with a boolean (true if manager, false if not);
        /// HTTP 400 Bad Request if UUID is invalid;
        /// HTTP 500 Internal Server Error if the data retrieval fails.
        /// </returns>
        [HttpGet("IsManager/{usersUUID}"), Authorize]
        public async Task<IActionResult> GetIsUserManager(Guid usersUUID)
        {
            //  Validate user claims
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null)
                return errorResult;

            try
            {
                //  Call service layer
                var isManager = await _employeesService.IsUserManagerAsync(usersUUID, basicModel);

                //  Handle result
                return Ok(new { usersUUID, isManager });
            }
            catch (Exception ex)
            {
                // Log error (optional) and return 500
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error checking manager status.", error = ex.Message });
            }
        }

        /// <summary>
        /// Creates or updates a Result record using the authenticated user's context.
        /// </summary>
        /// <remarks>
        /// Endpoint: POST /api/Goals/Results
        /// </remarks>
        /// <param name="req">The request model containing Result fields for insert/update.</param>
        /// <returns>
        /// Returns HTTP 200 OK with SaveResultResponse containing:
        /// - Id (created/updated record)
        /// - isValid (success/failure)
        /// - Message (operation details)
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveResultRequest req)
        {
            var (basic, error) = ValidateUserClaims();
            if (error != null) return error;

            var result = await _employeesService.SaveResult(req, basic);
            return Ok(result);
        }
    }
}
