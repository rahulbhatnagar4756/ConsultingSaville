using Library.API.Goals.Models.ContractPeriod;
using Library.API.Goals.Models.Contracts;
using Library.API.Goals.Models.RatingPeriod;
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
    /// Interface defines operations for managing contract results.
    /// </summary>
    public interface IContractResultsService
    {
        /// <summary>
        /// Retrieves contract results with all nested structures.
        /// </summary>
        /// <param name="contractsUUID">The UUID of the contract to retrieve</param>
        /// <returns>A ContractResultsResponse object or null.</returns>
        Task<ContractResultsResponse?> GetContractResults(string contractsUUID ,string _selectedUsersUUID=null);

        Task<List<RatingPeriodsDto?>> GetRatingPeriods();
    }

    /// <summary>
    /// Service class handles contract results-related operations.
    /// </summary>
    public class ContractResultsService : IContractResultsService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _apiConnect;
        private readonly ITokenService _token;
        private readonly string _urlBase;
        private readonly string _endPointGet = "Goals/ContractResults";
        private readonly string _endPointGetRatingPeriods = "Goals/ContractResults/GetRatingPeriods";


        /// <summary>
        /// Constructor to initialize the service with configuration, API connector, and token service.
        /// </summary>
        public ContractResultsService(IConfiguration config, IAPIConnectService apiConnect, ITokenService token)
        {
            _config = config;
            _apiConnect = apiConnect;
            _token = token;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Retrieves contract results with all nested structures from the API.
        /// </summary>
        /// <param name="contractsUUID">The UUID of the contract to retrieve</param>
        /// <param name="_selectedUsersUUID">The UUID of the contract to retrieve</param>
        /// <returns>A ContractResultsResponse object or null if token is missing.</returns>
        public async Task<ContractResultsResponse?> GetContractResults(string contractsUUID, string? _selectedUsersUUID=null)
        {
            // Get authentication token
            string? token = await _token.GetToken();

            // If token is null, return null (unauthenticated)
            if (token == null) return null;

            // Make a GET request to retrieve contract results for the specified contract UUID
            return await _apiConnect.GetAsync<ContractResultsResponse>(token, $"{_urlBase}{_endPointGet}/{contractsUUID}/{_selectedUsersUUID}");
        }

        public async Task<List<RatingPeriodsDto?>> GetRatingPeriods()
        {
            // Get authentication token
            string? token = await _token.GetToken();

            // If token is null, return null (unauthenticated)
            if (token == null) return null;

            return await _apiConnect.GetAsync<List<RatingPeriodsDto?>>(token, $"{_urlBase}{_endPointGetRatingPeriods}");
        }
    }
}
