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
    public interface IKPAService
    {
        Task<ResultsModel?> Save(ContractKPASaveModel saveModel);
        Task<ResultsModel?> Delete(string uuid);
    }

    public class KPAService : IKPAService
    {
        #region Fields
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;

        private readonly string _urlBase;
        private readonly string _endPointKPASave = "Goals/Contracts/KPA/Save";
        private readonly string _endPointKPADelete = "Goals/Contracts/KPA/Delete";

        #endregion

        #region Constructor

        public KPAService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;

            // Base URL from config
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        #endregion

        /// <summary>
        /// Save a KPA (create or update)
        /// </summary>
        /// <param name="saveModel">KPA save model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> Save(ContractKPASaveModel saveModel)
        {
            if (saveModel == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPA data." };

            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            // If token retrieval failed, return null
            if (token == null) return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

            try
            {
                // Make an HTTP POST call to save KPA data using token and constructed URL
                var result = await _aPIConnect.PostAsync<ResultsModel, ContractKPASaveModel>(token, $"{_urlBase}{_endPointKPASave}", saveModel);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error saving KPA: {ex.Message}" };
            }
        }

        /// <summary>
        /// Delete a KPA
        /// </summary>
        /// <param name="uuid">UUID of the KPA to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> Delete(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPA UUID." };

            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            // If token retrieval failed, return null
            if (token == null) return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

            try
            {
                // Make an HTTP POST call to delete KPA using token and constructed URL
                var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointKPADelete}", uuid);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error deleting KPA: {ex.Message}" };
            }
        }
    }
}
