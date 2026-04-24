using Library.API.Goals.Models;
using Library.API.Goals.Models.RatingPeriod;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;

namespace Library.API.Goals.Services
{
    public interface IRatingPeriodsServices
    {
        // Rating Period Types
        Task<List<RatingPeriodDto>?> GetRatingPeriodTypes();
        Task<ResultsModel?> CreateRatingPeriodType(RatingPeriodDto ratingPeriod);
        Task<DeleteResultDto?> DeleteRatingPeriodType(string uuid);

        // Rating Period Dates
        Task<List<RatingPeriodDateDto>?> GetRatingPeriodDates(string? periodTypeUUID = null);
        Task<ResultsModel?> CreateRatingPeriodDate(RatingPeriodDateDto ratingPeriodDate);
        Task<DeleteResultDto?> DeleteRatingPeriodDate(string uuid);
        Task<DeleteResultDto?> UpdateRatingPeriodDateIsActive(string uuid, bool isActive);

        // Rating Access Check
        Task<RatingAccessCheckDto?> CheckRatingAccess(string ratingPeriodDateUUID);
        Task<List<RatingPeriodModel>?> GetRatingPeriods();
    }

    public class RatingPeriodsServices : IRatingPeriodsServices
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;

        // Endpoint definitions
        private readonly string _endPointRatingPeriods = "Goals/RatingPeriods/GetRatingPeriods";

        private readonly string _endPointTypes = "Goals/RatingPeriods/Types";
        private readonly string _endPointTypesCreate = "Goals/RatingPeriods/Types/Create";
        private readonly string _endPointTypesDelete = "Goals/RatingPeriods/Types";
        private readonly string _endPointDates = "Goals/RatingPeriods/Dates";
        private readonly string _endPointDatesCreate = "Goals/RatingPeriods/Dates/Create";
        private readonly string _endPointDatesDelete = "Goals/RatingPeriods/Dates";
        private readonly string _endPointAccess = "Goals/RatingPeriods/Access";
        private readonly string _endPointDatesUpdateIsActive = "Goals/RatingPeriods/Dates";

        public RatingPeriodsServices(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        #region Rating Period Types

        public async Task<List<RatingPeriodModel>?> GetRatingPeriods()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<RatingPeriodModel>>(token, $"{_urlBase}{_endPointRatingPeriods}");
            return result;
        }


        /// <summary>
        /// Get all rating period types (Month, Quarter, Year) for administrative setup
        /// </summary>
        /// <returns>List of rating period types or null if failed</returns>
        public async Task<List<RatingPeriodDto>?> GetRatingPeriodTypes()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<RatingPeriodDto>>(token, $"{_urlBase}{_endPointTypes}");
            return result;
        }

        /// <summary>
        /// Create new rating period type (e.g., define "Quarter" with display "Qtr")
        /// </summary>
        /// <param name="ratingPeriod">Rating period type data to create</param>
        /// <returns>Result of the create operation</returns>
        public async Task<ResultsModel?> CreateRatingPeriodType(RatingPeriodDto ratingPeriod)
        {
            if (ratingPeriod == null)
                return new ResultsModel { isValid = false, Message = "Invalid rating period type data." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, RatingPeriodDto>(token, $"{_urlBase}{_endPointTypesCreate}", ratingPeriod);
            return result ?? new ResultsModel { isValid = false, Message = "Create operation failed." };
        }

        /// <summary>
        /// Delete rating period type (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the rating period type to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<DeleteResultDto?> DeleteRatingPeriodType(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return new DeleteResultDto { isValid = false, Message = "Rating period UUID is required." };

            string? token = await _token.GetToken();
            if (token == null)
                return new DeleteResultDto { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<DeleteResultDto, string>(token, $"{_urlBase}{_endPointTypesDelete}/{uuid}", uuid);
            return result ?? new DeleteResultDto { isValid = false, Message = "Delete operation failed." };
        }

        #endregion

        #region Rating Period Dates

        /// <summary>
        /// Get all rating period date instances, optionally filtered by period type
        /// </summary>
        /// <param name="periodTypeUUID">Optional filter by period type UUID</param>
        /// <returns>List of rating period dates or null if failed</returns>
        public async Task<List<RatingPeriodDateDto>?> GetRatingPeriodDates(string? periodTypeUUID = null)
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            string endpoint = _endPointDates;
            if (!string.IsNullOrWhiteSpace(periodTypeUUID))
            {
                endpoint += $"?periodTypeUUID={periodTypeUUID}";
            }

            var result = await _aPIConnect.GetAsync<List<RatingPeriodDateDto>>(token, $"{_urlBase}{endpoint}");
            return result;
        }

        /// <summary>
        /// Create/Update specific rating period date (e.g., "Q1 2025" with date ranges)
        /// </summary>
        /// <param name="ratingPeriodDate">Rating period date data to create</param>
        /// <returns>Result of the create operation</returns>
        public async Task<ResultsModel?> CreateRatingPeriodDate(RatingPeriodDateDto ratingPeriodDate)
        {
            if (ratingPeriodDate == null)
                return new ResultsModel { isValid = false, Message = "Invalid rating period date data." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, RatingPeriodDateDto>(token, $"{_urlBase}{_endPointDatesCreate}", ratingPeriodDate);
            return result ?? new ResultsModel { isValid = false, Message = "Create operation failed." };
        }

        /// <summary>
        /// Delete rating period date (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the rating period date to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<DeleteResultDto?> DeleteRatingPeriodDate(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return new DeleteResultDto { isValid = false, Message = "Rating period date UUID is required." };

            string? token = await _token.GetToken();
            if (token == null)
                return new DeleteResultDto { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<DeleteResultDto, string>(token, $"{_urlBase}{_endPointDatesDelete}/{uuid}", uuid);
            return result ?? new DeleteResultDto { isValid = false, Message = "Delete operation failed." };
        }

        #endregion

        #region Rating Access Check

        /// <summary>
        /// Check if KPI rating is currently accessible - Used by KPI rating system
        /// </summary>
        /// <param name="ratingPeriodDateUUID">UUID of the rating period date to check access for</param>
        /// <returns>Rating access check result or null if failed</returns>
        public async Task<RatingAccessCheckDto?> CheckRatingAccess(string ratingPeriodDateUUID)
        {
            if (string.IsNullOrWhiteSpace(ratingPeriodDateUUID))
                return new RatingAccessCheckDto
                {
                    IsAccessible = false,
                    Message = "Rating period date UUID is required.",
                    RatingPeriodDateUUID = ratingPeriodDateUUID
                };

            string? token = await _token.GetToken();
            if (token == null)
                return new RatingAccessCheckDto
                {
                    IsAccessible = false,
                    Message = "Authentication failed.",
                    RatingPeriodDateUUID = ratingPeriodDateUUID
                };

            var result = await _aPIConnect.GetAsync<RatingAccessCheckDto>(token, $"{_urlBase}{_endPointAccess}/{ratingPeriodDateUUID}");
            return result ?? new RatingAccessCheckDto
            {
                IsAccessible = false,
                Message = "Access check failed.",
                RatingPeriodDateUUID = ratingPeriodDateUUID
            };
        }


        /// <summary>
        /// Activate or deactivate a rating period date
        /// </summary>
        /// <param name="uuid">UUID of the rating period date</param>
        /// <param name="isActive">True to activate, False to deactivate</param>
        /// <returns>Result of the update operation</returns>
        public async Task<DeleteResultDto?> UpdateRatingPeriodDateIsActive(string uuid, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return new DeleteResultDto { isValid = false, Message = "Rating period date UUID is required." };

            string? token = await _token.GetToken();
            if (token == null)
                return new DeleteResultDto { isValid = false, Message = "Authentication failed." };

            // Build the full endpoint URL with uuid and isActive
            string endpoint = $"{_urlBase}{_endPointDatesUpdateIsActive}/{uuid}/{isActive.ToString().ToLower()}";

            var result = await _aPIConnect.PostAsync<DeleteResultDto, string>(token, endpoint, uuid);
            return result ?? new DeleteResultDto { isValid = false, Message = "Update isActive operation failed." };
        }

        #endregion
    }
}
