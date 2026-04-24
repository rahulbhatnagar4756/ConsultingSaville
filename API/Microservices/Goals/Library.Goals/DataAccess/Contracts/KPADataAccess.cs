using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Contracts
{
    internal static class KPADataAccess
    {
        /// <summary>
        /// Save a Contract KPA (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">KPA save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, ContractKPASaveModel saveModel)
        {
            var parameters = new
            {
                CompanyUUID = saveModel.CompanyUUID,
                UsersUUIDLoggedIn = saveModel.UsersUUIDLoggedIn,
                KPAUUID = saveModel.UUID,
                ContractPillarsUUID = saveModel.ContractPillarsUUID,
                StatusUUID = saveModel.StatusUUID,
                RatingPeriodsUUID = saveModel.RatingPeriodsUUID,
                Name = saveModel.Name,
                Description = saveModel.Description,
                Weight = saveModel.Weight,
                isActive = saveModel.isActive
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spKPA_Save]", parameters);
            return result;
        }

        /// <summary>
        /// Delete a Contract KPA (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the KPA to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spKPA_Delete]", parameters);
            return result;
        }
    }
}
