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
    internal static class KPALinksDataAccess
    {
        /// <summary>
        /// Save KPA Link (create or update)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="saveModel">KPA Links save model</param>
        /// <returns>Result of the save operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, ContractKPALinksSaveModel saveModel)
        {
            var parameters = new
            {
                saveModel.CompanyUUID,
                saveModel.UsersUUIDLoggedIn,
                KPALinksUUID = saveModel.UUID,
                saveModel.ContractPeriodsUUID,
                KPALinkTypesid = (int)saveModel.KPALinkTypesid,
                saveModel.LinkedUUID,
                saveModel.KPAUUID,
                saveModel.Weight
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spKPALinks_Save]", parameters);
            return result;
        }

        /// <summary>
        /// Delete KPA Link (soft delete)
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the KPA Link to delete</param>
        /// <returns>Result of the delete operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn,
                KPALinksUUID = uuid
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spKPALinks_Delete]", parameters);
            return result;
        }

        /// <summary>
        /// Change weight of an existing KPA Link
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the KPA Link</param>
        /// <param name="newWeight">New weight value</param>
        /// <returns>Result of the weight change operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> ChangeWeight(ISqlDataAccess sql, BasicModel basic, KPALinkWeightUpdateModel kPALink)
        {
            var parameters = new
            {
                basic.CompanyUUID,
                basic.UsersUUIDLoggedIn,
                KPALinksUUID = kPALink.UUID,
                NewWeight = kPALink.Weight
            };

            var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spKPALinks_ChangeWeight]", parameters);
            return result;
        }

        /// <summary>
        /// Bulk update weights for multiple KPA Links
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="weightUpdates">List of weight updates containing UUID and new weight</param>
        /// <returns>Result of the bulk update operation</returns>
        public static async Task<IEnumerable<ResultsModel>?> BulkUpdateWeights(ISqlDataAccess sql, BasicModel basic, List<KPALinkWeightUpdateModel> weightUpdates)
        {
            if (weightUpdates == null || !weightUpdates.Any())
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "No weight updates provided." } };

            var results = new List<ResultsModel>();

            foreach (var update in weightUpdates)
            {
                var result = await ChangeWeight(sql, basic, update);
                if (result != null)
                    results.AddRange(result);
            }

            return results;
        }
    }
}