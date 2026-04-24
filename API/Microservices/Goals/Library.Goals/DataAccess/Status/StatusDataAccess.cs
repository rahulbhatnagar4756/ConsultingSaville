using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Status
{
    internal class StatusDataAccess
    {

        /// <summary>
        /// Get all status records for a company
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <returns>Collection of status records</returns>
        public static async Task<IEnumerable<StatusModel>?> GetAll(ISqlDataAccess sql, BasicModel basic)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn
            };

            var result = await sql.LoadDataAsync<StatusModel, dynamic>("[Goals].[spStatus]", parameters);
            return result;
        }

        /// <summary>
        /// Save status (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">Status save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, StatusSaveModel saveModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spStatus_Save]", saveModel);
            return result;
        }

        /// <summary>
        /// Delete status (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the status to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spStatus_Delete]", parameters);
            return result;
        }
    }
}
