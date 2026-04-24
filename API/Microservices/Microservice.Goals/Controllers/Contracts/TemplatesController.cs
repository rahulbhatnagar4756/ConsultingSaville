using Library.Goals.Models;
using Library.Goals.Models.Templates;
using Library.Goals.Services.Contracts;
using Microservice.Base.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Microservice.Goals.Controllers.Contracts;

[Route("api/Goals/[controller]")]
[ApiController]
public class TemplatesController : BaseGoalController
{
    private readonly ITemplatesService _templatesService;
    private readonly ISecureService _secureService;

    public TemplatesController(ITemplatesService templatesService, ISecureService secureService)
    {
        _templatesService = templatesService;
        _secureService = secureService;
    }

    [HttpGet("AllTemplates"), Authorize]
    public async Task<IActionResult> GetTemplates()
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var results = await _templatesService.GetTemplates(new BasicModel() { CompanyUUID = basicModel.CompanyUUID
                                                                            , UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn });
        if (results == null) return BadRequest();
        return Ok(results);
    }

    [HttpPost("Save"), Authorize]
    public async Task<IActionResult> Save([FromBody] TemplateContractModel templateContract)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        templateContract.CompanyUUID = basicModel.CompanyUUID;
        templateContract.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;    

        var results = await _templatesService.Save(templateContract);
        if (results == null) return BadRequest();
        return Ok(results);
    }

    [HttpPost("Delete"), Authorize]
    public async Task<IActionResult> Delete([FromBody] string uuid)
    {
        if (string.IsNullOrEmpty(uuid)) return BadRequest("Invalid pillar selected.");

        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var result = await _templatesService.Delete(basicModel, uuid);
        if (result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting pillar.");

        return Ok(result);
    }


    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <remarks>
    /// Requires authorization. Calls the <see cref="_templatesService.GetAllUsers"/> method
    /// and returns a 200 OK response with the list of users, or 400 Bad Request if no users are found.
    /// </remarks>
    /// <returns>An <see cref="IActionResult"/> containing the list of users or a BadRequest response.</returns>
    [HttpPost("GetAllUsers"), Authorize]
    public async Task<IActionResult> GetAllUsers(UserTemplatesModel userTemplates)
    {
        var results = await _templatesService.GetAllUsers(userTemplates);
        if (results == null) return BadRequest();
        return Ok(results);
    }

    /// <summary>
    /// Saves or updates a user template.
    /// </summary>
    /// <remarks>
    /// Requires authorization. Validates user claims before calling 
    /// <see cref="_templatesService.UserTemplateSave"/>. 
    /// Returns a 200 OK response with the results of the operation, or 400 Bad Request if the operation fails.
    /// </remarks>
    /// <param name="userTemplates">The user template data sent in the request body.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the save operation or a BadRequest response.</returns>
    [HttpPost("UserTemplateSave"), Authorize]
    public async Task<IActionResult> UserTemplateSave([FromBody] UserTemplatesModel userTemplates)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var results = await _templatesService.UserTemplateSave(userTemplates);
        if (results == null) return BadRequest();
        return Ok(results);
    }


    /// <summary>
    /// Deletes a user template based on the provided template data.
    /// </summary>
    /// <param name="userTemplates">The user template to delete, passed in the request body.</param>
    /// <returns>
    /// Returns an <see cref="IActionResult"/>:
    /// <list type="bullet">
    /// <item><description>200 OK with the deletion results if successful.</description></item>
    /// <item><description>400 Bad Request if the deletion failed or no data was returned.</description></item>
    /// <item><description>Appropriate error response if user validation fails.</description></item>
    /// </list>
    /// </returns>
    [HttpPost("UserTemplateDelete"), Authorize]
    public async Task<IActionResult> UserTemplateDelete([FromBody] UserTemplatesModel userTemplates)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var results = await _templatesService.UserTemplateDelete(userTemplates);
        if (results == null) return BadRequest();
        return Ok(results);
    }




    /// <summary>
    /// Retrieves a list of users linked to a specific user template.
    /// </summary>
    /// <param name="userTemplates">The user template to check for linked users, passed in the request body.</param>
    /// <returns>
    /// Returns an <see cref="IActionResult"/>:
    /// <list type="bullet">
    /// <item><description>200 OK with the list of linked users if successful.</description></item>
    /// <item><description>400 Bad Request if no linked users are found.</description></item>
    /// <item><description>Appropriate error response if user validation fails.</description></item>
    /// </list>
    /// </returns>
    [HttpPost("LinkedUserTemplate"), Authorize]
    public async Task<IActionResult> LinkedUserTemplate([FromBody] UserTemplatesModel userTemplates)
    {
        var (basicModel, errorResult) = ValidateUserClaims();
        if (errorResult != null) return errorResult;

        var results = await _templatesService.LinkedUserTemplate(userTemplates);
        if (results == null) return BadRequest();
        return Ok(results);
    }


}
