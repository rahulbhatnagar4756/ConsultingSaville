using Library.API.Goals.Models;
using Library.API.Goals.Models.Status;
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
    public interface IStatusService
    {
        Task<List<StatusModel>?> GetStatus();
        Task<ResultsModel?> SaveStatus(StatusModel status);
        Task<ResultsModel?> DeleteStatus(string uuid);
        
        // StatusRanges methods
        Task<List<StatusRangesModel>?> GetStatusRanges(string statusUUID);
        Task<ResultsModel?> SaveStatusRange(string statusUUID, StatusRangesModel range);
        Task<ResultsModel?> DeleteStatusRange(string uuid);
    }

    public class StatusService : IStatusService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;

        private readonly string _urlBase;
        private readonly string _endPointStatus = "Goals/Status";
        private readonly string _endPointStatusSave = "Goals/Status/Save";
        private readonly string _endPointStatusDelete = "Goals/Status/Delete";
        private readonly string _endPointStatusRanges = "Goals/Status/Ranges";
        private readonly string _endPointStatusRangesSave = "Goals/Status/Ranges/{0}/Save";
        private readonly string _endPointStatusRangesDelete = "Goals/Status/Ranges/Delete";

        public StatusService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Get all status records for the authenticated user's company
        /// </summary>
        /// <returns>List of status records or null if failed</returns>
        public async Task<List<StatusModel>?> GetStatus()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<StatusModel>>(token, $"{_urlBase}{_endPointStatus}");
            return result;
        }

        /// <summary>
        /// Save status (create or update)
        /// </summary>
        /// <param name="status">Status data to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveStatus(StatusModel status)
        {
            if (status == null)
                return new ResultsModel { isValid = false, Message = "Invalid status data." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, StatusModel>(token, $"{_urlBase}{_endPointStatusSave}", status);
            return result ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        /// <summary>
        /// Delete status (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the status to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteStatus(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { isValid = false, Message = "Invalid status selected." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointStatusDelete}", uuid);
            return result ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }

        /// <summary>
        /// Get all status ranges for a specific status
        /// </summary>
        /// <param name="statusUUID">UUID of the status</param>
        /// <returns>List of status ranges or null if failed</returns>
        public async Task<List<StatusRangesModel>?> GetStatusRanges(string statusUUID)
        {
            if (string.IsNullOrEmpty(statusUUID)) return null;

            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<StatusRangesModel>>(token, $"{_urlBase}{_endPointStatusRanges}/{statusUUID}");
            return result;
        }

        /// <summary>
        /// Save status range (create or update)
        /// </summary>
        /// <param name="statusUUID">UUID of the status</param>
        /// <param name="range">Status range data to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveStatusRange(string statusUUID, StatusRangesModel range)
        {
            if (string.IsNullOrEmpty(statusUUID) || range == null)
                return new ResultsModel { isValid = false, Message = "Invalid status range data." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var endpoint = string.Format(_endPointStatusRangesSave, statusUUID);
            var result = await _aPIConnect.PostAsync<ResultsModel, StatusRangesModel>(token, $"{_urlBase}{endpoint}", range);
            return result ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        /// <summary>
        /// Delete status range (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the status range to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteStatusRange(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { isValid = false, Message = "Invalid status range selected." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointStatusRangesDelete}", uuid);
            return result ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }
    }
}