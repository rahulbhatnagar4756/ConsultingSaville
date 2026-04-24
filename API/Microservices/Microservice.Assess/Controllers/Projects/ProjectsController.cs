using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Assess.Services.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Assess.Controllers.Projects
{
    [Route("api/Assess/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : BasicTokenController
    {
        private readonly IProjectsService _projectsService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectsService projectsService, ILogger<ProjectsController> logger)
        {
            _projectsService = projectsService;
            _logger = logger;
        }

        /// <summary>
        /// Get projects with search and filter options
        /// </summary>
        /// <param name="searchModel">Search model with filters</param>
        /// <returns>List of projects</returns>
        [HttpPost("Search"), Authorize]
        public async Task<IActionResult> GetProjects([FromBody] ProjectSearchModel searchModel)
        {
            if (searchModel == null)
                return BadRequest("Search parameters are required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            // Override with validated claims
            searchModel.CompanyUUID = basicModel!.CompanyUUID;
            searchModel.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;

            var data = await _projectsService.GetProjects(searchModel);

            if (data == null || !data.Any()) 
                return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

        /// <summary>
        /// Get projects with grouped model structure for easier data consumption
        /// </summary>
        /// <param name="searchModel">Search model with filters</param>
        /// <returns>List of grouped projects</returns>
        [HttpPost("SearchGrouped"), Authorize]
        public async Task<IActionResult> GetProjectsGrouped([FromBody] ProjectSearchModel searchModel)
        {
            if (searchModel == null)
                return BadRequest("Search parameters are required.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            // Override with validated claims
            searchModel.CompanyUUID = basicModel!.CompanyUUID;
            searchModel.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;

            var data = await _projectsService.GetProjectsGrouped(searchModel);

            if (data == null || !data.Any()) 
                return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

        /// <summary>
        /// Get projects with default search parameters (all active projects)
        /// </summary>
        /// <returns>List of active projects</returns>
        [HttpGet("All"), Authorize]
        public async Task<IActionResult> GetAllProjects()
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var searchModel = new ProjectSearchModel
            {
                CompanyUUID = basicModel!.CompanyUUID,
                UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn,
                isWaiting = true,
                isReleased = true,
                isCompleted = false,
                isArchive = false,
                isDeleted = false
            };

            var data = await _projectsService.GetProjects(searchModel);

            if (data == null || !data.Any()) 
                return StatusCode(StatusCodes.Status204NoContent);

            return Ok(data);
        }

        /// <summary>
        /// Save project (create or update)
        /// </summary>
        /// <param name="saveModel">Project data to save</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("Save"), Authorize]
        public async Task<IActionResult> SaveProject([FromBody] ProjectSaveModel saveModel)
        {
            if (saveModel == null)
                return BadRequest("Invalid project data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            // Override with validated claims
            saveModel.CompanyUUID = basicModel!.CompanyUUID;
            saveModel.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;

            var result = await _projectsService.SaveProject(saveModel);

            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving project.");

            if (result.isValid == false)
                return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Update project status flags (isWaiting, isReleased, isCompleted, isArchive)
        /// </summary>
        /// <param name="statusModel">Status update data</param>
        /// <returns>Result of the status update operation</returns>
        [HttpPost("UpdateStatus"), Authorize]
        public async Task<IActionResult> UpdateProjectStatus([FromBody] ProjectStatusUpdateModel statusModel)
        {
            if (statusModel == null)
                return BadRequest("Invalid status update data.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            // Override with validated claims
            statusModel.CompanyUUID = basicModel!.CompanyUUID;
            statusModel.UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn;

            var result = await _projectsService.UpdateProjectStatus(statusModel);

            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating project status.");

            if (result.isValid == false)
                return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Delete project (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the project to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteProject([FromBody] string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return BadRequest("Invalid project selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _projectsService.DeleteProject(basicModel!, uuid);

            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting project.");

            if (result.isValid == false)
                return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Delete project by UUID in route parameter
        /// </summary>
        /// <param name="uuid">UUID of the project to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpDelete("{uuid}"), Authorize]
        public async Task<IActionResult> DeleteProjectByRoute(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return BadRequest("Invalid project selected.");

            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var result = await _projectsService.DeleteProject(basicModel!, uuid);

            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting project.");

            if (result.isValid == false)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
