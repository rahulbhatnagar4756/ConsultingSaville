using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Tolerances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Tolerances
{
    internal class ToleranceRangesDataAccess
    {
        /// <summary>
        /// Get all tolerance ranges for a specific tolerance set
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="toleranceSetsUUID">UUID of the tolerance set</param>
        /// <returns>Collection of tolerance ranges</returns>
        public static async Task<IEnumerable<ToleranceRangesModel>?> GetAllByToleranceSet(ISqlDataAccess sql, BasicModel basic, string toleranceSetsUUID)
        {
            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                ToleranceSetsUUID = toleranceSetsUUID
            };

            var result = await sql.LoadDataAsync<ToleranceRangesModel, dynamic>("[Goals].[spToleranceRanges]", parameters);
            return result;
        }

        /// <summary>
        /// Save tolerance range (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">Tolerance range save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, ToleranceRangesSaveModel saveModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spToleranceRanges_Save]", saveModel);
            return result;
        }

        /// <summary>
        /// Delete tolerance range (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the tolerance range to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spToleranceRanges_Delete]", parameters);
            return result;
        }
    }
}
