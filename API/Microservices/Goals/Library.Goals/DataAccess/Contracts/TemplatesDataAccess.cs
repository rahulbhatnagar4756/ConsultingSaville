using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Contracts
{
    public static class TemplatesDataAccess
    {
        /// <summary>
        /// get the templates and their associated data
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Basic"></param>
        /// <returns>json that then gets deserialized into templetsModel</returns>
        public static async Task<List<TemplatesModel>?> Information(ISqlDataAccess sql, BasicModel Basic) =>
            await sql.LoadDataJsonAsync<TemplatesModel, dynamic>("[Goals].[spTemplates_Information]", Basic);


        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, TemplateContractModel templateContract) =>
            await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spTemplates_Save]", templateContract);

        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string templatesUUID)
        {
            var deleteModel = new
            {
                UUID = templatesUUID,
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn
            };
            return await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spTemplates_Delete]", deleteModel);
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <param name="sql">The SQL data access instance used to execute the stored procedure.</param>
        /// <returns>A list of UsersModel objects, or null if no data is returned.</returns>
        public static async Task<IEnumerable<UsersModel>?> GetAllUsers(ISqlDataAccess sql, UserTemplatesModel userTemplates)
        {
            var userTemplate = new
            {
                TemplatesUUID = userTemplates.TemplatesUUID,
                ContractPeriodsUUID = userTemplates.ContractPeriodsUUID
            };
            return await sql.LoadDataAsync<UsersModel, dynamic>("[Goals].[spUsers_GetAll]", userTemplate);
        }



        /// <summary>
        /// Assigns a template to one or more users by executing the corresponding stored
        /// procedure for each user, and returns all results from the database.
        /// </summary>
        /// <param name="sql">The SQL data access instance used to execute the stored procedure.</param>
        /// <param name="userTemplates">The template assignment data containing users and template details.</param>
        /// <returns>
        /// A collection of <see cref="ResultsModel"/> returned by the stored procedure,
        /// or null if no users are provided.
        /// </returns>
        public static async Task<IEnumerable<ResultsModel>?> UserTemplateSave(ISqlDataAccess sql, UserTemplatesModel userTemplates)
        {
            if (userTemplates.UsersUUID == null || !userTemplates.UsersUUID.Any())
                return null;

            var allResults = new List<ResultsModel>();

            foreach (var user in userTemplates.UsersUUID)
            {
                var userTemplate = new
                {
                    UsersUUID = user, // just use the string directly
                    TemplatesUUID = userTemplates.TemplatesUUID,
                    ContractPeriodsUUID = userTemplates.ContractPeriodsUUID
                };

                var results = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spAssignTemplateWithKPAsKPIs]", userTemplate);

                if (results != null)
                    allResults.AddRange(results);
            }

            return allResults;
        }


        /// <summary>
        /// Retrieves all users linked to a specified template and contract period.
        /// </summary>
        /// <param name="sql">The SQL data access instance used to execute the stored procedure.</param>
        /// <param name="userTemplates">The template details used to filter linked users.</param>
        /// <returns>
        /// A collection of <see cref="UsersModel"/> linked to the template, or null if none are found.
        /// </returns>
        public static async Task<IEnumerable<UsersModel>?> LinkedUserTemplate(ISqlDataAccess sql, UserTemplatesModel userTemplates)
        {
            var userTemplate= new
            {
                TemplatesUUID = userTemplates.TemplatesUUID,
                ContractPeriodsUUID = userTemplates.ContractPeriodsUUID
            };
            return await sql.LoadDataAsync<UsersModel, dynamic>("[Goals].[spLinkedUserTemplate_GetAll]", userTemplate);
        }


        /// <summary>
        /// Removes a template assignment from one or more users by executing the corresponding
        /// stored procedure for each user, and returns all results from the database.
        /// </summary>
        /// <param name="sql">The SQL data access instance used to execute the stored procedure.</param>
        /// <param name="userTemplates">The template assignment data containing users and template details.</param>
        /// <returns>
        /// A collection of <see cref="ResultsModel"/> returned by the stored procedure,
        /// or null if no users are provided.
        /// </returns>
        public static async Task<IEnumerable<ResultsModel>?> UserTemplateDelete(ISqlDataAccess sql, UserTemplatesModel userTemplates)
        {
            if (userTemplates.UsersUUID == null || !userTemplates.UsersUUID.Any())
                return null;

            var allResults = new List<ResultsModel>();

            foreach (var user in userTemplates.UsersUUID)
            {
                var userTemplate = new
                {
                    UsersUUID = user, // just use the string directly
                    TemplatesUUID = userTemplates.TemplatesUUID,
                    ContractPeriodsUUID = userTemplates.ContractPeriodsUUID
                };

                var results = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spDeleteAssignedTemplateWithKPAsKPIs]", userTemplate);

                if (results != null)
                    allResults.AddRange(results);
            }

            return allResults;
        }




    }
}
