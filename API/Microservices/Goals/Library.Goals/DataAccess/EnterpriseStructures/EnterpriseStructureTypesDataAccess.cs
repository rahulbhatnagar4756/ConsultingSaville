using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.EnterpriseStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.EnterpriseStructures
{
    internal static class EnterpriseStructureTypesDataAccess
    {
        /// <summary>
        /// Get all enterprise structure types for a company
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <returns>Collection of enterprise structure types</returns>
        public static async Task<IEnumerable<EnterpriseStructureTypesModel>?> GetAll(ISqlDataAccess sql, BasicModel basic)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn
            };

            var result = await sql.LoadDataAsync<EnterpriseStructureTypesModel, dynamic>("[Goals].[spEnterpriseStructureTypes]", parameters);
            return result;
        }

        /// <summary>
        /// Save enterprise structure type (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">Enterprise structure type save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, EnterpriseStructureTypesSaveModel saveModel)
        {
            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spEnterpriseStructureTypes_Save]", saveModel);
            return result;
        }

        /// <summary>
        /// Delete enterprise structure type (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the enterprise structure type to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spEnterpriseStructureTypes_Delete]", parameters);
            return result;
        }
    }
}
