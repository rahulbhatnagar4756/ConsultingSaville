using Library.API.Goals.Models;
using Library.API.Goals.Models.Employee;
using Library.API.Goals.Models.Result;
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
    /// Interface defines operations for managing employees.
    /// </summary>
    public interface IEmployeesService
    {
        /// <summary>
        /// Retrieves the list of all employees.
        /// </summary>
        /// <returns>A list of Employee objects or null.</returns>
        Task<List<Employee>?> GetEmployees();

        /// <summary>
        /// Checks if a specific user is a manager.
        /// </summary>
        /// <param name="usersUUID">The UUID of the user to check.</param>
        /// <returns>True if the user is a manager, false otherwise.</returns>
        Task<bool> IsUserManager(Guid usersUUID);

        Task<ResultsModel?> Save(ResultSaveModel saveModel);
    }

    /// <summary>
    /// Service class handles employee-related operations.
    /// </summary>
    public class EmployeesService : IEmployeesService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _apiConnect;
        private readonly ITokenService _token;

        private readonly string _urlBase;
        private readonly string _endPointGet = "Goals/Employees";
        private const string _endpointIsManager = "Goals/Employees/IsManager";
        private const string _endpointResultSave = "Goals/Employees";


        /// <summary>
        /// Constructor to initialize the service with configuration, API connector, and token service.
        /// </summary>
        public EmployeesService(IConfiguration config, IAPIConnectService apiConnect, ITokenService token)
        {
            _config = config;
            _apiConnect = apiConnect;
            _token = token;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Retrieves the list of all employees from the API.
        /// </summary>
        /// <returns>A list of Employee objects or null if token is missing.</returns>
        public async Task<List<Employee>?> GetEmployees()
        {
            // Get authentication token
            string? token = await _token.GetToken();
            if (token == null) return null;

            // Make a GET request to retrieve employees
            return await _apiConnect.GetAsync<List<Employee>>(token, $"{_urlBase}{_endPointGet}");
        }

        /// <summary>
        /// Calls the API endpoint to check whether the specified user is a manager or not.
        /// </summary>
        /// <param name="usersUUID">The UUID of the user to check.</param>
        /// <returns>True if the user is a manager, false otherwise.</returns>
        public async Task<bool> IsUserManager(Guid usersUUID)
        {
            // Get authentication token
            string? token = await _token.GetToken();
            if (token == null) return false;

            // Build endpoint URL (same base URL, just different endpoint)
            string endpoint = $"{_urlBase}{_endpointIsManager}/{usersUUID}";

            // Call API endpoint
            var response = await _apiConnect.GetAsync<dynamic>(token, endpoint);

            // Handle null or missing data safely
            if (response == null) return false;

            try
            {
                // The backend returns { usersUUID: "...", isManager: true/false }
                bool isManager = (bool)response.isManager;
                return isManager;
            }
            catch
            {
                return false; // Fallback if response parsing fails
            }
        }

        public async Task<ResultsModel?> Save(ResultSaveModel saveModel)
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
                var result = await _apiConnect.PostAsync<ResultsModel, ResultSaveModel>(token, $"{_urlBase}{_endpointResultSave}", saveModel);
                return result;
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error saving KPA: {ex.Message}" };
            }
        }

    }
}
