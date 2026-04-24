using Library.API.Goals.Models;
using Library.API.Goals.Models.ContractPeriod;
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
    /// <summary>
    /// Interface defines operations for managing contract periods.
    /// </summary>
    public interface IContractPeriodsService
    {
        /// <summary>
        /// Retrieves the list of all contract periods.
        /// </summary>
        /// <returns>A list of ContractPeriod objects or null.</returns>
        Task<List<ContractPeriod>?> GetContractPeriods();

        /// <summary>
        /// Saves a new or existing contract period.
        /// </summary>
        /// <param name="contractPeriod">The contract period to save.</param>
        /// <returns>Result of the operation.</returns>
        Task<ResultsModel?> SaveContractPeriod(ContractPeriod contractPeriod);

        /// <summary>
        /// Deletes a contract period by its UUID.
        /// </summary>
        /// <param name="uuid">The unique identifier of the contract period to delete.</param>
        /// <returns>Result of the operation.</returns>
        Task<ResultsModel?> DeleteContractPeriod(string uuid);
    }

    /// <summary>
    /// Service class handles contract period-related operations.
    /// </summary>
    public class ContractPeriodsService : IContractPeriodsService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _apiConnect;
        private readonly ITokenService _token;

        private readonly string _urlBase;
        private readonly string _endPointGet = "Goals/ContractPeriods";
        private readonly string _endPointDelete = "Goals/ContractPeriods/Delete";
        private readonly string _endPointSave = "Goals/ContractPeriods/Save";

        /// <summary>
        /// Constructor to initialize the service with configuration, API connector, and token service.
        /// </summary>
        public ContractPeriodsService(IConfiguration config, IAPIConnectService apiConnect, ITokenService token)
        {
            _config = config;
            _apiConnect = apiConnect;
            _token = token;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }


        /// <summary>
        /// Retrieves the list of all contract periods from the API.
        /// </summary>
        /// <returns>A list of ContractPeriod objects or null if token is missing.</returns>
        public async Task<List<ContractPeriod>?> GetContractPeriods()
        {
            // Get authentication token
            string? token = await _token.GetToken();
            // If token is null, return null (unauthenticated)
            if (token == null) return null;
            // Make a GET request to retrieve the list of contract periods
            return await _apiConnect.GetAsync<List<ContractPeriod>>(token, $"{_urlBase}{_endPointGet}");
        }

        /// <summary>
        /// Saves a contract period by sending it to the API.
        /// </summary>
        /// <param name="contractPeriod">The contract period object to save.</param>
        /// <returns>A ResultsModel indicating success or failure.</returns>
        public async Task<ResultsModel?> SaveContractPeriod(ContractPeriod contractPeriod)
        {
            // Check if contract period is null (invalid data)
            if (contractPeriod == null)
                return new ResultsModel { isValid = false, Message = "Invalid pillar data." };
            // Retrieve authentication token
            var token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };
            // Make a POST request to save the contract period
            var result = await _apiConnect.PostAsync<ResultsModel, ContractPeriod>(token, $"{_urlBase}{_endPointSave}", contractPeriod);
            // Return result or default error response
            return result ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        /// <summary>
        /// Deletes a contract period based on its UUID.
        /// </summary>
        /// <param name="uuid">The unique identifier of the contract period.</param>
        /// <returns>A ResultsModel indicating success or failure.</returns>
        public async Task<ResultsModel?> DeleteContractPeriod(string uuid)
        {
            // Validate UUID input
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { isValid = false, Message = "Invalid contract period selected." };
            // Get authentication token
            var token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };
            // Send POST request to delete the contract period using its UUID
            var result = await _apiConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointDelete}", uuid);
            // Return the result or error response
            return result ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }
    }
}
