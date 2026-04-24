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
    public interface IKPIService
    {
        Task<ResultsModel?> Save(ContractKPISaveModel saveModel);
        Task<ResultsModel?> Delete(string uuid);
    }

    public class KPIService : IKPIService
    {
        #region Fields
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;

        private readonly string _urlBase;
        private readonly string _endPointKPISave = "Goals/Contracts/KPI/Save";
        private readonly string _endPointKPIDelete = "Goals/Contracts/KPI/Delete";

        #endregion

        #region Constructor

        public KPIService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;

            // Base URL from config
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        #endregion

        /// <summary>
        /// Save a KPI (create or update)
        /// </summary>
        /// <param name="saveModel">KPI save model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> Save(ContractKPISaveModel saveModel)
        {
            if (saveModel == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPI data." };

            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            // If token retrieval failed, return null
            if (token == null) return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

            try
            {
                // Make an HTTP POST call to save KPI data using token and constructed URL
                var result = await _aPIConnect.PostAsync<ResultsModel, ContractKPISaveModel>(token, $"{_urlBase}{_endPointKPISave}", saveModel);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error saving KPI: {ex.Message}" };
            }
        }

        /// <summary>
        /// Delete a KPI
        /// </summary>
        /// <param name="uuid">UUID of the KPI to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> Delete(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPI UUID." };

            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            // If token retrieval failed, return null
            if (token == null) return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

            try
            {
                // Make an HTTP POST call to delete KPI using token and constructed URL
                var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointKPIDelete}", uuid);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error deleting KPI: {ex.Message}" };
            }
        }
    }
}