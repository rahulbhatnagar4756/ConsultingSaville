using Library.API.Goals.Models;
using Library.API.Goals.Models.Pillars;
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
    public interface IPillarsServices
    {
        Task<List<PillarsModel>?> GetPillars();
        Task<ResultsModel?> SavePillar(PillarsModel pillar);
        Task<ResultsModel?> DeletePillar(string uuid);
    }

    public class PillarsServices : IPillarsServices
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;

        private readonly string _urlBase;
        private readonly string _endPointPillars = "Goals/Pillars";
        private readonly string _endPointPillarsSave = "Goals/Pillars/Save";
        private readonly string _endPointPillarsDelete = "Goals/Pillars/Delete";

        public PillarsServices(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Goals:URL").Value ?? _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Get all pillars for the authenticated user's company
        /// </summary>
        /// <returns>List of pillars or null if failed</returns>
        public async Task<List<PillarsModel>?> GetPillars()
        {
            string? token = await _token.GetToken();
            if (token == null) return null;

            var result = await _aPIConnect.GetAsync<List<PillarsModel>>(token, $"{_urlBase}{_endPointPillars}");
            return result;
        }

        /// <summary>
        /// Save pillar (create or update)
        /// </summary>
        /// <param name="pillar">Pillar data to save</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SavePillar(PillarsModel pillar)
        {
            if (pillar == null)
                return new ResultsModel { isValid = false, Message = "Invalid pillar data." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, PillarsModel>(token, $"{_urlBase}{_endPointPillarsSave}", pillar);
            return result ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        /// <summary>
        /// Delete pillar (soft delete)
        /// </summary>
        /// <param name="uuid">UUID of the pillar to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeletePillar(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { isValid = false, Message = "Invalid pillar selected." };

            string? token = await _token.GetToken();
            if (token == null)
                return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var result = await _aPIConnect.PostAsync<ResultsModel, string>(token, $"{_urlBase}{_endPointPillarsDelete}", uuid);
            return result ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }
    }
}