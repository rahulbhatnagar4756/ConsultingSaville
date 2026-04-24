using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess
{
    internal static class StatusRangesDataAccess
    {
        /// <summary>
        /// Get all status ranges for a specific status
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="statusUUID">UUID of the status</param>
        /// <returns>Collection of status ranges</returns>
        public static async Task<IEnumerable<StatusRangesModel>?> GetAllByStatus(ISqlDataAccess sql, BasicModel basic, string statusUUID)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn,
                StatusUUID = statusUUID
            };

            var result = await sql.LoadDataAsync<StatusRangesModel, dynamic>("[Goals].[spStatusRanges]", parameters);
            return result;
        }

        /// <summary>
        /// Save status range (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">Status range save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, StatusRangesSaveModel saveModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spStatusRanges_Save]", saveModel);
            return result;
        }

        /// <summary>
        /// Delete status range (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the status range to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spStatusRanges_Delete]", parameters);
            return result;
        }
    }
}
