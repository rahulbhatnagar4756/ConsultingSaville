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
    public interface IPositionExperienceService
    {
        Task<List<EmployeeJobExperienceBaseModel>?> GetPositionExperienceByJobUUID(string employeeJobsUUID);
        Task<ResultsModel> SavePositionExperience(EmployeeJobExperienceBaseModel experience);
        Task<ResultsModel> DeletePositionExperience(string uuid);
    }

    /// <summary>
    /// Position Experience Service - manages experience requirements for positions
    /// Uses the EmployeeJobExperience API endpoints
    /// </summary>
    public class PositionExperienceService : IPositionExperienceService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointGetExperience = "Employee/EmployeeJobExperience/All";
        private readonly string _endPointSaveExperience = "Employee/EmployeeJobExperience/Save";
        private readonly string _endPointDeleteExperience = "Employee/EmployeeJobExperience/Delete";

        public PositionExperienceService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Get experience requirements for a specific position/job
        /// </summary>
        /// <param name="employeeJobsUUID">The UUID of the position/job to get experience for</param>
        /// <returns>List of experience requirements or null if none found</returns>
        public async Task<List<EmployeeJobExperienceBaseModel>?> GetPositionExperienceByJobUUID(string employeeJobsUUID)
        {
            if (string.IsNullOrEmpty(employeeJobsUUID))
                return null;

            try
            {
                string? token = await _token.GetToken();
                if (token == null) return null;

                var response = await _aPIConnect.GetAsync<List<EmployeeJobExperienceBaseModel>>(
                    token, 
                    $"{_urlBase}{_endPointGetExperience}/{employeeJobsUUID}"
                );

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting position experience: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Save (create or update) an experience requirement for a position
        /// </summary>
        /// <param name="experience">The experience requirement to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel> SavePositionExperience(EmployeeJobExperienceBaseModel experience)
        {
            if (experience == null)
                return new ResultsModel { isValid = false, Message = "Invalid experience data." };

            if (string.IsNullOrEmpty(experience.EmployeeJobsUUID) || string.IsNullOrEmpty(experience.Information))
                return new ResultsModel { isValid = false, Message = "Position UUID and experience information are required." };

            try
            {
                string? token = await _token.GetToken();
                if (token == null) 
                    return new ResultsModel { isValid = false, Message = "Authentication failed." };

                var response = await _aPIConnect.PostAsync<ResultsModel, EmployeeJobExperienceBaseModel>(
                    token, 
                    $"{_urlBase}{_endPointSaveExperience}", 
                    experience
                );

                return response ?? new ResultsModel { isValid = false, Message = "Failed to save experience requirement." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving position experience: {ex.Message}");
                return new ResultsModel { isValid = false, Message = $"Error: {ex.Message}" };
            }
        }

        /// <summary>
        /// Delete an experience requirement
        /// </summary>
        /// <param name="uuid">The UUID of the experience requirement to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel> DeletePositionExperience(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { isValid = false, Message = "Invalid experience record UUID." };

            try
            {
                string? token = await _token.GetToken();
                if (token == null) 
                    return new ResultsModel { isValid = false, Message = "Authentication failed." };

                var response = await _aPIConnect.PostAsync<ResultsModel, string>(
                    token, 
                    $"{_urlBase}{_endPointDeleteExperience}", 
                    uuid
                );

                return response ?? new ResultsModel { isValid = false, Message = "Failed to delete experience requirement." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting position experience: {ex.Message}");
                return new ResultsModel { isValid = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
