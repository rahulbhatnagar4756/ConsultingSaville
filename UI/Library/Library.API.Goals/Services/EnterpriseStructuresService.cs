using Library.API.Goals.Models;
using Library.API.Goals.Models.EnterpriseStructures;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Services
{
    public interface IEnterpriseStructuresServices
    {
        Task<List<EnterpriseStructureTypesModel>?> GetEnterpriseStructureTypes();
        Task<ResultsModel?> SaveEnterpriseStructureType(EnterpriseStructureTypesModel enterpriseStructureType);
        Task<ResultsModel?> DeleteEnterpriseStructureType(string uuid);
        Task<List<EnterpriseStructureWeightsModel>?> GetEnterpriseStructureWeights();
        Task<ResultsModel?> SaveEnterpriseStructureWeight(EnterpriseStructureWeightsModel enterpriseStructureWeight);
        Task<ResultsModel?> DeleteEnterpriseStructureWeight(string uuid);
    }

    public class EnterpriseStructuresServices : IEnterpriseStructuresServices
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;

        private readonly string _urlBase;
        private readonly string _endPointEnterpriseStructureTypes = "Goals/EnterpriseStructures/Types";
        private readonly string _endPointEnterpriseStructureTypesSave = "Goals/EnterpriseStructures/Types/Save";
        private readonly string _endPointEnterpriseStructureTypesDelete = "Goals/EnterpriseStructures/Types/Delete";
        private readonly string _endPointEnterpriseStructureWeights = "Goals/EnterpriseStructures/Weights";
        private readonly string _endPointEnterpriseStructureWeightsSave = "Goals/EnterpriseStructures/Weights/Save";
        private readonly string _endPointEnterpriseStructureWeightsDelete = "Goals/EnterpriseStructures/Weights/Delete";

        public EnterpriseStructuresServices(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Get all enterprise structure types for the authenticated user's company
        /// </summary>
        /// <returns>List of enterprise structure types or null if failed</returns>
        public async Task<List<EnterpriseStructureTypesModel>?> GetEnterpriseStructureTypes()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<EnterpriseStructureTypesModel>>(token, $"{_urlBase}{_endPointEnterpriseStructureTypes}");
            return result;
        }

        /// <summary>
        /// Save enterprise structure type (create or update)
        /// </summary>
        /// <param name="enterpriseStructureType">Enterprise structure type data to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveEnterpriseStructureType(EnterpriseStructureTypesModel enterpriseStructureType)
        {
            if (enterpriseStructureType == null)
                return new ResultsModel { isValid = false, Message = "Invalid enterprise structure type data." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, EnterpriseStructureTypesModel>(token, $"{_urlBase}{_endPointEnterpriseStructureTypesSave}", enterpriseStructureType);
            return result ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        /// <summary>
        /// Delete enterprise structure type (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the enterprise structure type to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteEnterpriseStructureType(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { isValid = false, Message = "Invalid enterprise structure type selected." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointEnterpriseStructureTypesDelete}", uuid);
            return result ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }

        /// <summary>
        /// Get all enterprise structure weights for the authenticated user's company
        /// </summary>
        /// <returns>List of enterprise structure weights or null if failed</returns>
        public async Task<List<EnterpriseStructureWeightsModel>?> GetEnterpriseStructureWeights()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<EnterpriseStructureWeightsModel>>(token, $"{_urlBase}{_endPointEnterpriseStructureWeights}");
            return result;
        }

        /// <summary>
        /// Save enterprise structure weight (create or update)
        /// </summary>
        /// <param name="enterpriseStructureWeight">Enterprise structure weight data to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveEnterpriseStructureWeight(EnterpriseStructureWeightsModel enterpriseStructureWeight)
        {
            if (enterpriseStructureWeight == null)
                return new ResultsModel { isValid = false, Message = "Invalid enterprise structure weight data." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, EnterpriseStructureWeightsModel>(token, $"{_urlBase}{_endPointEnterpriseStructureWeightsSave}", enterpriseStructureWeight);
            return  result;
        }

        /// <summary>
        /// Delete enterprise structure weight (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the enterprise structure weight to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteEnterpriseStructureWeight(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { isValid = false, Message = "Invalid enterprise structure weight selected." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel>(token, $"{_urlBase}{_endPointEnterpriseStructureWeightsDelete}", uuid);
            return result;
        }

    }
}