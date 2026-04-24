using Library.API.Base.Models;
using Library.API.Base.Models.BusinessHierarchy.Jobs;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Services.BusinessHierarchy.Jobs
{
    public interface IPositionEducationService
    {
        Task<List<EmployeeJobEducationBaseModel>?> GetPositionEducationByJobUUID(string employeeJobsUUID);
        Task<ResultsModel> SavePositionEducation(EmployeeJobEducationBaseModel education);
        Task<ResultsModel> DeletePositionEducation(string uuid);
    }

    /// <summary>
    /// Position Education Service - manages education requirements for positions
    /// Uses the EmployeeJobEducation API endpoints
    /// </summary>
    public class PositionEducationService : IPositionEducationService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointGetEducation = "Employee/EmployeeJobEducation/All";
        private readonly string _endPointSaveEducation = "Employee/EmployeeJobEducation/Save";
        private readonly string _endPointDeleteEducation = "Employee/EmployeeJobEducation/Delete";

        public PositionEducationService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Get education requirements for a specific position/job
        /// </summary>
        /// <param name="employeeJobsUUID">The UUID of the position/job to get education for</param>
        /// <returns>List of education requirements or null if none found</returns>
        public async Task<List<EmployeeJobEducationBaseModel>?> GetPositionEducationByJobUUID(string employeeJobsUUID)
        {
            if (string.IsNullOrEmpty(employeeJobsUUID))
                return null;

            try
            {
                string? token = await _token.GetToken();
                if (token == null) return null;

                var response = await _aPIConnect.GetAsync<List<EmployeeJobEducationBaseModel>>(
                    token, 
                    $"{_urlBase}{_endPointGetEducation}/{employeeJobsUUID}"
                );

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting position education: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Save (create or update) an education requirement for a position
        /// </summary>
        /// <param name="education">The education requirement to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel> SavePositionEducation(EmployeeJobEducationBaseModel education)
        {
            if (education == null)
                return new ResultsModel { isValid = false, Message = "Invalid education data." };

            if (string.IsNullOrEmpty(education.EmployeeJobsUUID) || string.IsNullOrEmpty(education.Information))
                return new ResultsModel { isValid = false, Message = "Position UUID and education information are required." };

            try
            {
                string? token = await _token.GetToken();
                if (token == null) 
                    return new ResultsModel { isValid = false, Message = "Authentication failed." };

                var response = await _aPIConnect.PostAsync<ResultsModel, EmployeeJobEducationBaseModel>(
                    token, 
                    $"{_urlBase}{_endPointSaveEducation}", 
                    education
                );

                return response ?? new ResultsModel { isValid = false, Message = "Failed to save education requirement." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving position education: {ex.Message}");
                return new ResultsModel { isValid = false, Message = $"Error: {ex.Message}" };
            }
        }

        /// <summary>
        /// Delete an education requirement
        /// </summary>
        /// <param name="uuid">The UUID of the education requirement to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel> DeletePositionEducation(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { isValid = false, Message = "Invalid education record UUID." };

            try
            {
                string? token = await _token.GetToken();
                if (token == null) 
                    return new ResultsModel { isValid = false, Message = "Authentication failed." };

                 
                var response = await _aPIConnect.PostAsync<ResultsModel, string>(
                    token, 
                    $"{_urlBase}{_endPointDeleteEducation}",
                    uuid
                );

                return response ?? new ResultsModel { isValid = false, Message = "Failed to delete education requirement." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting position education: {ex.Message}");
                return new ResultsModel { isValid = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
