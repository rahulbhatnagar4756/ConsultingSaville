using Library.API.Base.Models;
using Library.API.Base.Models.BusinessHierarchy;
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
    public interface IPositionService
    {
        Task<ResultsModel> Delete(string UUID);
        Task<List<PositionsModel>?> GetPositions();
        Task<ResultsModel> Save(PositionsModel position);
    }

    /// <summary>
    /// Position is the EmployeeJobs Service on the API
    /// </summary>
    public class PositionService : IPositionService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointPositions = "Employee/Positions/All";
        private readonly string _endPointPositionsSave = "Employee/Positions/Save";
        private readonly string _endPointPositionsDelete = "Employee/Positions/Delete";

        public PositionService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        public async Task<List<PositionsModel>?> GetPositions()
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;
            var response = await _aPIConnect.GetAsync<List<PositionsModel>>(Token, $"{_urlBase}{_endPointPositions}");
            return response;
        }

        public async Task<ResultsModel> Save(PositionsModel position)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };
            var response = await _aPIConnect.PostAsync<ResultsModel, PositionsModel>(Token, $"{_urlBase}{_endPointPositionsSave}", position);
            return response;
        }

        public async Task<ResultsModel> Delete(string UUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };
            var response = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointPositionsDelete}", UUID);
            return response;
        }
    }
}
