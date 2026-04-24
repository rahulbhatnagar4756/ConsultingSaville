using Library.API.Goals.Models;
using Library.API.Goals.Models.ScoringPeriod;
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
    public interface IScoringPeriodsService
    {
        // Scoring Periods CRUD
        Task<List<ScoringPeriodDto>?> GetScoringPeriods();
        Task<ResultsModel?> CreateScoringPeriod(ScoringPeriodDto scoringPeriod);
        Task<DeleteResultDto?> DeleteScoringPeriod(string uuid);
    }

    public class ScoringPeriodsService : IScoringPeriodsService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;

        // Endpoint 
        private readonly string _endPointScoringPeriods = "Goals/ScoringPeriods";
        private readonly string _endPointScoringPeriodsCreate = "Goals/ScoringPeriods/Create";
        private readonly string _endPointScoringPeriodsDelete = "Goals/ScoringPeriods";

        public ScoringPeriodsService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        #region Scoring Periods

        /// <summary>
        /// BUSINESS RULE: Retrieve all scoring periods for the current company.
        /// - Returns only non-deleted records (handled in SP).
        /// - Supports future filtering (active/inactive).
        /// </summary>
        public async Task<List<ScoringPeriodDto>?> GetScoringPeriods()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            return await _aPIConnect.GetAsync<List<ScoringPeriodDto>>(token, $"{_urlBase}{_endPointScoringPeriods}");
        }

        /// <summary>
        /// BUSINESS RULE: Create a new scoring period
        /// - Requires Name, StartDay, StartMonth, EndDay, EndMonth
        /// - Sets isActive = true by default, DateDeactivated = null
        /// </summary>
        public async Task<ResultsModel?> CreateScoringPeriod(ScoringPeriodDto scoringPeriod)
        {
            if (scoringPeriod == null)
                return new ResultsModel { isValid = false, Message = "Invalid scoring period data." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            return await _aPIConnect.PostAsync<ResultsModel, ScoringPeriodDto>(
                token, $"{_urlBase}{_endPointScoringPeriodsCreate}", scoringPeriod);
        }      

        /// <summary>
        /// BUSINESS RULE: Soft-delete scoring period
        /// - Sets isDeleted = 1, isActive = 0, DateDeactivated = now
        /// - Logs delete operation in Audit Trail (handled in SP)
        /// - Prevents delete if already deleted (handled in SP)
        /// </summary>
        public async Task<DeleteResultDto?> DeleteScoringPeriod(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return new DeleteResultDto { isValid = false, Message = "Scoring period UUID is required." };

            string? token = await _token.GetToken();
            if (token == null)
                return new DeleteResultDto { isValid = false, Message = "Authentication failed." };

            return await _aPIConnect.PostAsync<DeleteResultDto, string>(
                token, $"{_urlBase}{_endPointScoringPeriodsDelete}/{uuid}", uuid);
        }

        #endregion
    }

}
