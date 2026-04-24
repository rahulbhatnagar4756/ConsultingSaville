using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.KPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Contracts
{
    internal static class KPIDataAccess
    {
        /// <summary>
        /// Save a KPI (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">KPI save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, KPISaveModel saveModel)
        {
            var parameters = new
            {
                CompanyUUID = saveModel.CompanyUUID,
                UsersUUIDLoggedIn = saveModel.UsersUUIDLoggedIn,
                UsersUUID = saveModel.UsersUUID,
                KPIUUID = saveModel.UUID,
                KPAUUID = saveModel.KPAUUID,
                Name = saveModel.Name,
                Description = saveModel.Description,
                StatusUUID = saveModel.StatusUUID ,
                ToleranceSetsUUID = saveModel.ToleranceSetsUUID,
                DateStart = saveModel.DateStart,
                DateEnd = saveModel.DateEnd,
                Target = saveModel.Target,
                Weights = saveModel.Weights,
                TrackingUUID = saveModel.TrackingUUID,
                isScoreProcessing = saveModel.isScoreProcessing
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spKPI_Save]", parameters);
            return result;
        }

        /// <summary>
        /// Delete a KPI (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the KPI to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                KPIUUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spKPI_Delete]", parameters);
            return result;
        }
    }
}
