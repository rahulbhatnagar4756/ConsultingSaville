using Library.API.Goals.Models;
using Library.API.Goals.Models.Contracts;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Services.Contracts
{
    public interface IKPALinksService
    {
        Task<ResultsModel?> Save(ContractLinkedSaveModel saveModel);
        Task<ResultsModel?> Delete(string uuid);
        Task<ResultsModel?> ChangeWeight(string uuid, decimal newWeight);
        Task<List<ResultsModel>?> BulkUpdateWeights(List<KPALinkWeightUpdateModel> weightUpdates);
        Task<ResultsModel?> BulkSave(List<ContractLinkedSaveModel> saveModel);
    }

    public class KPALinksService : IKPALinksService
    {
        #region Fields
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;

        private readonly string _urlBase;
        private readonly string _endPointKPALinksSave = "Goals/KPALinks/Save";
        private readonly string _endPointKPALinksBulkSave = "Goals/KPALinks/BulkSave";
        private readonly string _endPointKPALinksDelete = "Goals/KPALinks/Delete";
        private readonly string _endPointKPALinksChangeWeight = "Goals/KPALinks/ChangeWeight";
        private readonly string _endPointKPALinksBulkUpdateWeights = "Goals/KPALinks/BulkUpdateWeights";

        #endregion

        #region Constructor

        public KPALinksService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;

            // Base URL from config
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        #endregion

        /// <summary>
        /// Save a KPA Link (create or update)
        /// </summary>
        /// <param name="saveModel">KPA Link save model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> Save(ContractLinkedSaveModel saveModel)
        {
            if (saveModel == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPA Link data." };

            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

            try
            {
                // Convert ContractLinkedSaveModel to the expected API model
                var apiModel = new ContractKPALinksSaveModel
                {
                    ContractPeriodsUUID = saveModel.ContractPeriodsUUID,
                    KPALinkTypesid = saveModel.KPALinkTypesid,
                    LinkedUUID = saveModel.LinkedUUID,
                    KPAUUID = saveModel.KPAUUID,
                    Weight = saveModel.Weight
                };

                // Make an HTTP POST call to save KPA Link data using token and constructed URL
                var result = await _aPIConnect.PostAsync<ResultsModel, ContractKPALinksSaveModel>(token, $"{_urlBase}{_endPointKPALinksSave}", apiModel);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error saving KPA Link: {ex.Message}" };
            }
        }

        public async Task<ResultsModel?> BulkSave(List<ContractLinkedSaveModel> saveModel)
        {
            if (saveModel == null || !saveModel.Any())
                return new ResultsModel { UUID = null, isValid = false, Message = "KPA Link save models are required." };
            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };
            try
            {
                // Convert List<ContractLinkedSaveModel> to List<ContractKPALinksSaveModel>
                var apiModels = saveModel.Select(s => new ContractKPALinksSaveModel
                {
                    ContractPeriodsUUID = s.ContractPeriodsUUID,
                    KPALinkTypesid = s.KPALinkTypesid,
                    LinkedUUID = s.LinkedUUID,
                    KPAUUID = s.KPAUUID,
                    Weight = s.Weight
                }).ToList();
                // Make an HTTP POST call to bulk save KPA Link data using token and constructed URL
                var result = await _aPIConnect.PostAsync<List<ResultsModel>, List<ContractKPALinksSaveModel>>(token, $"{_urlBase}{_endPointKPALinksBulkSave}", apiModels);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error saving KPA Links: {ex.Message}" };
            }
        }



        /// <summary>
        /// Delete a KPA Link
        /// </summary>
        /// <param name="uuid">UUID of the KPA Link to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> Delete(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPA Link UUID." };

            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

            try
            {
                // Make an HTTP POST call to delete KPA Link using token and constructed URL
                var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointKPALinksDelete}", uuid);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error deleting KPA Link: {ex.Message}" };
            }
        }

        /// <summary>
        /// Change weight of an existing KPA Link
        /// </summary>
        /// <param name="uuid">UUID of the KPA Link</param>
        /// <param name="newWeight">New weight value</param>
        /// <returns>Result of the weight change operation</returns>
        public async Task<ResultsModel?> ChangeWeight(string uuid, decimal newWeight)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPA Link UUID." };

            if (newWeight <= 0)
                return new ResultsModel { UUID = null, isValid = false, Message = "Weight must be greater than 0." };

            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

            try
            {
                // Create the weight change request model
                var request = new KPALinkWeightChangeRequest
                {
                    UUID = uuid,
                    Weight = newWeight
                };

                // Make an HTTP POST call to change KPA Link weight using token and constructed URL
                var result = await _aPIConnect.PostAsync<ResultsModel, KPALinkWeightChangeRequest>(token, $"{_urlBase}{_endPointKPALinksChangeWeight}", request);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error changing KPA Link weight: {ex.Message}" };
            }
        }

        /// <summary>
        /// Bulk update weights for multiple KPA Links
        /// </summary>
        /// <param name="weightUpdates">List of weight updates containing UUID and new weight</param>
        /// <returns>Results of the bulk update operation</returns>
        public async Task<List<ResultsModel>?> BulkUpdateWeights(List<KPALinkWeightUpdateModel> weightUpdates)
        {
            if (weightUpdates == null || !weightUpdates.Any())
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "Weight updates are required." } };

            // Validate all weight updates
            var invalidUpdates = weightUpdates.Where(w => string.IsNullOrEmpty(w.UUID) || w.Weight <= 0).ToList();
            if (invalidUpdates.Any())
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "All weight updates must have valid UUID and weight greater than 0." } };

            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            if (token == null) 
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = "Authentication failed." } };

            try
            {
                // Make an HTTP POST call to bulk update KPA Link weights using token and constructed URL
                var result = await _aPIConnect.PostAsync<List<ResultsModel>, List<KPALinkWeightUpdateModel>>(token, $"{_urlBase}{_endPointKPALinksBulkUpdateWeights}", weightUpdates);
                return result;
            }
            catch (Exception ex)
            {
                return new List<ResultsModel> { new() { UUID = null, isValid = false, Message = $"Error updating KPA Link weights: {ex.Message}" } };
            }
        }
    }

    /// <summary>
    /// Request model for changing KPA Link weight
    /// </summary>
    public class KPALinkWeightChangeRequest
    {
        public string UUID { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }

    /// <summary>
    /// Model for bulk weight updates
    /// </summary>
    public class KPALinkWeightUpdateModel
    {
        public string UUID { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }

    /// <summary>
    /// Extended save model that matches the API contract
    /// </summary>
    public class ContractKPALinksSaveModel
    {
        /// <summary>
        /// UUID of the KPA Link to update, null for new link
        /// </summary>
        public string? UUID { get; set; }
        
        /// <summary>
        /// UUID of the Contract Period this link belongs to
        /// </summary>
        public string? ContractPeriodsUUID { get; set; }
        
        /// <summary>
        /// Type of link (KPA or KPI)
        /// </summary>
        public Library.API.Goals.Enumerables.Contracts.KPALinkTypes KPALinkTypesid { get; set; }
        
        /// <summary>
        /// UUID of the linked entity (KPA or KPI UUID)
        /// </summary>
        public string? LinkedUUID { get; set; }
        
        /// <summary>
        /// UUID of the KPA this link belongs to
        /// </summary>
        public string? KPAUUID { get; set; }
        
        /// <summary>
        /// Weight of the link
        /// </summary>
        public decimal Weight { get; set; } = 100.0m;
    }
}
