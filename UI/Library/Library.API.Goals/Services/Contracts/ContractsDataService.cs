using Library.API.Goals.Models.Contracts;
using Library.API.Service;
using Library.Security.Services;

using Microsoft.Extensions.Configuration;

namespace Library.API.Goals.Services.Contracts
{
    public interface IContractsDataService
    {
        /// <summary>
        /// Retrieves a single contract based on its unique identifier.
        /// </summary>
        /// <param name="contractsUUID">The unique identifier for the contract.</param>
        /// <returns>
        /// A task representing the asynchronous operation. 
        /// The result contains a <see cref="ContractsDataModel"/> if found; otherwise, null.
        /// </returns>
        Task<ContractsDataModel?> GetContract(string contractsUUID);

    }

    public class ContractsDataServices : IContractsDataService
    {
        #region Fields
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;

        private readonly string _urlBase;
        private readonly string _endPointContract = "Goals/ContractsData";

        #endregion

        #region Constructor

        public ContractsDataServices(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;

            // Base URL from config
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        #endregion

        /// <summary>
        /// Retrieves contract data by its UUID from the external API.
        /// </summary>
        /// <param name="contractsUUID">The unique contract identifier.</param>
        /// <returns>
        /// A task that returns a <see cref="ContractsDataModel"/> if found; otherwise, null.
        /// </returns>
        public async Task<ContractsDataModel?> GetContract(string contractsUUID)
        {
            // Retrieve the authentication token for API call
            string? token = await _token.GetToken();
            // If token retrieval failed, return null
            if (token == null) return null;
            // Make an HTTP GET call to fetch contract data using token and constructed URL
            return await _aPIConnect.GetAsync<ContractsDataModel>(token, $"{_urlBase}{_endPointContract}/{contractsUUID}");           
        }
    }
}
