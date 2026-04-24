using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Pillars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess
{
    internal static class PillarsDataAccess
    {
        /// <summary>
        /// Get all pillars for a company
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <returns>Collection of pillars</returns>
        public static async Task<IEnumerable<PillarsModel>?> GetAll(ISqlDataAccess sql, BasicModel basic)
        {
            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn
            };

            var result = await sql.LoadDataAsync<PillarsModel, dynamic>("[Goals].[spPillars]", parameters);
            return result;
        }

        /// <summary>
        /// Save pillar (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">Pillar save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, PillarsSaveModel saveModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spPillars_Save]", saveModel);
            return result;
        }

        /// <summary>
        /// Delete pillar (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the pillar to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spPillars_Delete]", parameters);
            return result;
        }
    }
}
