using Library.API.Assess.Models;
using Library.API.Assess.Models.Projects;
using Library.API.Assess.Models.Projects.Groups;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Services.Projects
{
    public interface IProjectsService
    {
        Task<List<ProjectModel>?> GetProjects(ProjectSearchModel searchModel);
        Task<List<ProjectGroupedModel>?> GetProjectsGrouped(ProjectSearchModel searchModel);
        Task<List<ProjectModel>?> GetAllProjects();
        Task<ResultsModel> SaveProject(ProjectSaveModel saveModel);
        Task<ResultsModel> DeleteProject(string uuid);
    }

    public class ProjectsService : IProjectsService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointSearch = "Assess/Projects/Search";
        private readonly string _endPointSearchGrouped = "Assess/Projects/SearchGrouped";
        private readonly string _endPointAll = "Assess/Projects/All";
        private readonly string _endPointSave = "Assess/Projects/Save";
        private readonly string _endPointDelete = "Assess/Projects/Delete";

        public ProjectsService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Get projects with search and filter options
        /// </summary>
        /// <param name="searchModel">Search model with filters</param>
        /// <returns>List of projects</returns>
        public async Task<List<ProjectModel>?> GetProjects(ProjectSearchModel searchModel)
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            try
            {
                var response = await _aPIConnect.PostAsync<List<ProjectModel>, ProjectSearchModel>(
                    token, $"{_urlBase}{_endPointSearch}", searchModel);
                return response;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return null;
            }
        }

        /// <summary>
        /// Get projects with grouped model structure for easier data consumption
        /// </summary>
        /// <param name="searchModel">Search model with filters</param>
        /// <returns>List of grouped projects</returns>
        public async Task<List<ProjectGroupedModel>?> GetProjectsGrouped(ProjectSearchModel searchModel)
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            try
            {
                var response = await _aPIConnect.PostAsync<List<ProjectGroupedModel>, ProjectSearchModel>(
                    token, $"{_urlBase}{_endPointSearchGrouped}", searchModel);
                return response;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return null;
            }
        }

        /// <summary>
        /// Get all active projects
        /// </summary>
        /// <returns>List of active projects</returns>
        public async Task<List<ProjectModel>?> GetAllProjects()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            try
            {
                var response = await _aPIConnect.GetAsync<List<ProjectModel>>(
                    token, $"{_urlBase}{_endPointAll}");
                return response;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return null;
            }
        }

        /// <summary>
        /// Save project (create or update)
        /// </summary>
        /// <param name="saveModel">Project data to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel> SaveProject(ProjectSaveModel saveModel)
        {
            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            try
            {
                var response = await _aPIConnect.PostAsync<ResultsModel, ProjectSaveModel>(
                    token, $"{_urlBase}{_endPointSave}", saveModel);
                return response ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
            }
            catch (Exception ex)
            {
                return new ResultsModel { isValid = false, Message = "An error occurred while saving the project." };
            }
        }

        /// <summary>
        /// Delete project (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the project to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel> DeleteProject(string uuid)
        {
            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            try
            {
                var response = await _aPIConnect.PostAsync<ResultsModel, string>(
                    token, $"{_urlBase}{_endPointDelete}", uuid);
                return response ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
            }
            catch (Exception ex)
            {
                return new ResultsModel { isValid = false, Message = "An error occurred while deleting the project." };
            }
        }
    }
}
