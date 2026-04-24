using Library.Assess.DataAccess.Projects;
using Library.Assess.Mapper.Projects;
using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Assess.Models.Projects.Groups;
using Library.Database.DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Services.Projects
{
    public interface IProjectsService
    {
        Task<List<ProjectModel>?> GetProjects(ProjectSearchModel searchModel);
        Task<List<ProjectGroupedModel>?> GetProjectsGrouped(ProjectSearchModel searchModel);
        Task<ResultsModel?> SaveProject(ProjectSaveModel saveModel);
        Task<ResultsModel?> UpdateProjectStatus(ProjectStatusUpdateModel statusModel);
        Task<ResultsModel?> DeleteProject(BasicGetModel basic, string uuid);
    }

    public class ProjectsService : IProjectsService
    {
        private readonly ISqlDataAccess _sql;
        private readonly IConfiguration _configuration;

        public ProjectsService(ISqlDataAccess sql, IConfiguration configuration)
        {
            _sql = sql;
            _configuration = configuration;
        }

        /// <summary>
        /// Get projects with search and filter options
        /// </summary>
        /// <param name="searchModel">Search model with filters</param>
        /// <returns>List of project models</returns>
        public async Task<List<ProjectModel>?> GetProjects(ProjectSearchModel searchModel)
        {
            try
            {
                var data = await ProjectsDataAccess.GetProjects(_sql, searchModel);
                if (data == null || !data.Any()) return null;
                return data.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return null;
            }
        }

        /// <summary>
        /// Get projects with grouped model structure for easier data consumption
        /// </summary>
        /// <param name="searchModel">Search model with filters</param>
        /// <returns>List of grouped project models</returns>
        public async Task<List<ProjectGroupedModel>?> GetProjectsGrouped(ProjectSearchModel searchModel)
        {
            try
            {
                var data = await ProjectsDataAccess.GetProjects(_sql, searchModel);
                if (data == null || !data.Any()) return null;
                
                return data.ToGroupedModels().ToList();
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return null;
            }
        }

        /// <summary>
        /// Save project (create or update)
        /// </summary>
        /// <param name="saveModel">Project save model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveProject(ProjectSaveModel saveModel)
        {
            if (saveModel == null) 
                return new ResultsModel { isValid = false, Message = "Invalid project data." };

            if (string.IsNullOrEmpty(saveModel.Name)) 
                return new ResultsModel { isValid = false, Message = "Project name is required." };

            try
            {
                var result = await ProjectsDataAccess.SaveProject(_sql, saveModel);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { isValid = false, Message = "Error saving project." };
            }
        }

        /// <summary>
        /// Update project status flags
        /// </summary>
        /// <param name="statusModel">Project status update model</param>
        /// <returns>Result of the status update operation</returns>
        public async Task<ResultsModel?> UpdateProjectStatus(ProjectStatusUpdateModel statusModel)
        {
            if (statusModel == null) 
                return new ResultsModel { isValid = false, Message = "Invalid status update data." };

            if (string.IsNullOrEmpty(statusModel.UUID)) 
                return new ResultsModel { isValid = false, Message = "Project UUID is required." };

            // Validate that at least one status flag is provided
            if (statusModel.isWaiting == null && statusModel.isReleased == null && 
                statusModel.isCompleted == null && statusModel.isArchive == null)
            {
                return new ResultsModel { isValid = false, Message = "At least one status flag must be provided." };
            }

            try
            {
                var result = await ProjectsDataAccess.UpdateProjectStatus(_sql, statusModel);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { isValid = false, Message = "Error updating project status." };
            }
        }

        /// <summary>
        /// Delete project (soft delete)
        /// </summary>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the project to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteProject(BasicGetModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) 
                return new ResultsModel { isValid = false, Message = "Invalid project selected." };

            var deleteModel = new DeleteModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            try
            {
                var result = await ProjectsDataAccess.DeleteProject(_sql, deleteModel);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { isValid = false, Message = "Error deleting project." };
            }
        }
    }
}
