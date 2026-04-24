using Library.API.Goals.Models;
using Library.API.Goals.Models.Tolerances;
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
    public interface ITolerancesServices
    {
        Task<List<ToleranceSetsModel>?> GetToleranceSets();
        Task<ResultsModel?> SaveToleranceSet(ToleranceSetsModel toleranceSet);
        Task<ResultsModel?> DeleteToleranceSet(string uuid);
        
        // ToleranceRanges methods
        Task<List<ToleranceRangesModel>?> GetToleranceRanges(string toleranceSetsUUID);
        Task<ResultsModel?> SaveToleranceRange(ToleranceRangesModel range);
        Task<ResultsModel?> DeleteToleranceRange(string uuid);
    }

    public class TolerancesServices : ITolerancesServices
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;

        private readonly string _urlBase;
        private readonly string _endPointToleranceSets = "Goals/Tolerances/Sets";
        private readonly string _endPointToleranceSetsSave = "Goals/Tolerances/Sets/Save";
        private readonly string _endPointToleranceSetsDelete = "Goals/Tolerances/Sets/Delete";
        private readonly string _endPointToleranceRanges = "Goals/Tolerances/Ranges";
        private readonly string _endPointToleranceRangesSave = "Goals/Tolerances/Ranges/Save";
        private readonly string _endPointToleranceRangesDelete = "Goals/Tolerances/Ranges/Delete";

        public TolerancesServices(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Get all tolerance sets for the authenticated user's company
        /// </summary>
        /// <returns>List of tolerance sets or null if failed</returns>
        public async Task<List<ToleranceSetsModel>?> GetToleranceSets()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<ToleranceSetsModel>>(token, $"{_urlBase}{_endPointToleranceSets}");
            return result;
        }

        /// <summary>
        /// Save tolerance set (create or update)
        /// </summary>
        /// <param name="toleranceSet">Tolerance set data to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveToleranceSet(ToleranceSetsModel toleranceSet)
        {
            if (toleranceSet == null) 
                return new ResultsModel { isValid = false, Message = "Invalid tolerance set data." };

            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, ToleranceSetsModel>(token, $"{_urlBase}{_endPointToleranceSetsSave}", toleranceSet);
            return result ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        /// <summary>
        /// Delete tolerance set (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the tolerance set to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteToleranceSet(string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) 
                return new ResultsModel { isValid = false, Message = "Invalid tolerance set selected." };

            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointToleranceSetsDelete}", uuid);
            return result ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }

        /// <summary>
        /// Get all tolerance ranges for a specific tolerance set
        /// </summary>
        /// <param name="toleranceSetsUUID">UUID of the tolerance set</param>
        /// <returns>List of tolerance ranges or null if failed</returns>
        public async Task<List<ToleranceRangesModel>?> GetToleranceRanges(string toleranceSetsUUID)
        {
            if (string.IsNullOrEmpty(toleranceSetsUUID)) return null;

            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<ToleranceRangesModel>>(token, $"{_urlBase}{_endPointToleranceRanges}/{toleranceSetsUUID}");
            return result;
        }

        /// <summary>
        /// Save tolerance range (create or update)
        /// </summary>
        /// <param name="toleranceSetsUUID">UUID of the tolerance set</param>
        /// <param name="range">Tolerance range data to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveToleranceRange(ToleranceRangesModel range)
        {
            if (range == null) 
                return new ResultsModel { isValid = false, Message = "Invalid tolerance range data." };

            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, ToleranceRangesModel>(token, $"{_urlBase}{_endPointToleranceRangesSave}", range);
            return result ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        /// <summary>
        /// Delete tolerance range (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the tolerance range to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteToleranceRange(string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) 
                return new ResultsModel { isValid = false, Message = "Invalid tolerance range selected." };

            string? token = await _token.GetToken();
            if (token == null) 
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointToleranceRangesDelete}", uuid);
            return result ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }
    }
}
