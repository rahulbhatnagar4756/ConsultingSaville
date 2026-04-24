using Library.Database.DAL;
using Library.Goals.DataAccess.Contracts;
using Library.Goals.Models;
using Library.Goals.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services.Contracts
{
    public interface IKPALinksService
    {
        Task<ResultsModel?> Save(ContractKPALinksSaveModel saveModel);
        Task<ResultsModel?> Delete(BasicModel basic, string uuid);
        Task<ResultsModel?> ChangeWeight(BasicModel basic, KPALinkWeightUpdateModel kpalink);
        Task<List<ResultsModel>?> BulkUpdateWeights(BasicModel basic, List<KPALinkWeightUpdateModel> weightUpdates);
        Task<ResultsModel?> BulkSave(List<ContractKPALinksSaveModel> saveModels);
    }

    public class KPALinksService : IKPALinksService
    {
        private readonly ISqlDataAccess _sql;

        public KPALinksService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Save KPA Link (create or update)
        /// </summary>
        /// <param name="saveModel">KPA Links save model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> Save(ContractKPALinksSaveModel saveModel)
        {
            if (saveModel == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPA Link data." };

            // Validate required fields
            if (string.IsNullOrEmpty(saveModel.CompanyUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Company UUID is required." };

            if (string.IsNullOrEmpty(saveModel.UsersUUIDLoggedIn))
                return new ResultsModel { UUID = null, isValid = false, Message = "User UUID is required." };

            if (string.IsNullOrEmpty(saveModel.KPAUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "KPA UUID is required." };

            if (string.IsNullOrEmpty(saveModel.LinkedUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Linked UUID is required." };

            try
            {
                var result = await KPALinksDataAccess.Save(_sql, saveModel);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error saving KPA Link: {ex.Message}" };
            }
        }

        public async Task<ResultsModel?> BulkSave(List<ContractKPALinksSaveModel> saveModels)
        {
            if (saveModels == null || !saveModels.Any())
                return new ResultsModel { UUID = null, isValid = false, Message = "No KPA Link data provided." };
            try
            {
                var results = new List<ResultsModel>();
                foreach (var model in saveModels)
                {
                    var result = await Save(model);
                    if (result != null)
                        results.Add(result);
                }
                // Return a summary result
                if (results.All(r => r.isValid == true))
                {
                    return new ResultsModel { UUID = null, isValid = true, Message = "All KPA Links saved successfully." };
                }
                else
                {
                    var errorMessages = string.Join("; ", results.Where(r => r.isValid == false).Select(r => r.Message));
                    return new ResultsModel { UUID = null, isValid = false, Message = $"Some KPA Links failed to save: {errorMessages}" };
                }
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error during bulk save of KPA Links: {ex.Message}" };
            }
        }

        /// <summary>
        /// Delete KPA Link (soft delete)
        /// </summary>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the KPA Link to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> Delete(BasicModel basic, string uuid)
        {
            if (basic == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Basic model is required." };

            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "KPA Link UUID is required." };

            if (string.IsNullOrEmpty(basic.CompanyUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Company UUID is required." };

            if (string.IsNullOrEmpty(basic.UsersUUIDLoggedIn))
                return new ResultsModel { UUID = null, isValid = false, Message = "User UUID is required." };

            try
            {
                var result = await KPALinksDataAccess.Delete(_sql, basic, uuid);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error deleting KPA Link: {ex.Message}" };
            }
        }

        /// <summary>
        /// Change weight of an existing KPA Link
        /// </summary>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the KPA Link</param>
        /// <param name="newWeight">New weight value</param>
        /// <returns>Result of the weight change operation</returns>
        public async Task<ResultsModel?> ChangeWeight(BasicModel basic, KPALinkWeightUpdateModel kpalink)
        {
            if (basic == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Basic model is required." };

            if (string.IsNullOrEmpty(kpalink.UUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "KPA Link UUID is required." };

            if (string.IsNullOrEmpty(basic.CompanyUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Company UUID is required." };

            if (string.IsNullOrEmpty(basic.UsersUUIDLoggedIn))
                return new ResultsModel { UUID = null, isValid = false, Message = "User UUID is required." };

            if (kpalink.Weight <= 0)
                return new ResultsModel { UUID = null, isValid = false, Message = "Weight must be greater than 0." };

            try
            {
                var result = await KPALinksDataAccess.ChangeWeight(_sql, basic, kpalink);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error changing KPA Link weight: {ex.Message}" };
            }
        }

        /// <summary>
        /// Bulk update weights for multiple KPA Links
        /// </summary>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="weightUpdates">List of weight updates containing UUID and new weight</param>
        /// <returns>Result of the bulk update operation</returns>
        public async Task<List<ResultsModel>?> BulkUpdateWeights(BasicModel basic, List<KPALinkWeightUpdateModel> weightUpdates)
        {
            if (basic == null)
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "Basic model is required." } };

            if (weightUpdates == null || !weightUpdates.Any())
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "Weight updates are required." } };

            if (string.IsNullOrEmpty(basic.CompanyUUID))
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "Company UUID is required." } };

            if (string.IsNullOrEmpty(basic.UsersUUIDLoggedIn))
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "User UUID is required." } };

            // Validate all weight updates
            var invalidUpdates = weightUpdates.Where(w => string.IsNullOrEmpty(w.UUID) || w.Weight <= 0).ToList();
            if (invalidUpdates.Any())
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "All weight updates must have valid UUID and weight greater than 0." } };

            try
            {
                var result = await KPALinksDataAccess.BulkUpdateWeights(_sql, basic, weightUpdates);
                return result?.ToList();
            }
            catch (Exception ex)
            {
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = $"Error updating KPA Link weights: {ex.Message}" } };
            }
        }
    }
}
