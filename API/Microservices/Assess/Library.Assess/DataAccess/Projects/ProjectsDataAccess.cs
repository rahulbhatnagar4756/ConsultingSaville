using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.DataAccess.Projects
{
    internal class ProjectsDataAccess
    {
        /// <summary>
        /// Get projects for a company with search and filter options
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="model">Project search model with filters</param>
        /// <returns>Collection of project records</returns>
        public static async Task<IEnumerable<ProjectModel>?> GetProjects(ISqlDataAccess sql, ProjectSearchModel model)
        {
            var result = await sql.LoadDataAsync<ProjectModel, dynamic>("[Project].[spProjects]", model);
            return result;
        }

        /// <summary>
        /// Save project (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">Project save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> SaveProject(ISqlDataAccess sql, ProjectSaveModel saveModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Project].[spProjects_Save]", saveModel);
            return result;
        }

        /// <summary>
        /// Update project status flags
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="statusModel">Project status update model</param>
        /// <returns>Result of the status update operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> UpdateProjectStatus(ISqlDataAccess sql, ProjectStatusUpdateModel statusModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Project].[spProjects_Update_Status]", statusModel);
            return result;
        }

        /// <summary>
        /// Delete project (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="deleteModel">Delete model with project UUID</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> DeleteProject(ISqlDataAccess sql, DeleteModel deleteModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Project].[spProjects_Delete]", deleteModel);
            return result;
        }
    }
}
