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
    internal static class ToleranceSetsDataAccess
    {
        /// <summary>
        /// Get all tolerance sets for a company
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <returns>Collection of tolerance sets</returns>
        public static async Task<IEnumerable<ToleranceSetsModel>?> GetAll(ISqlDataAccess sql, BasicModel basic)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn
            };

            var result = await sql.LoadDataAsync<ToleranceSetsModel, dynamic>("[Goals].[spToleranceSets]", parameters);
            return result;
        }

        /// <summary>
        /// Save tolerance set (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">Tolerance set save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, ToleranceSetsSaveModel saveModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spToleranceSets_Save]", saveModel);
            return result;
        }

        /// <summary>
        /// Delete tolerance set (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the tolerance set to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spToleranceSets_Delete]", parameters);
            return result;
        }
    }
}