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

namespace Library.API.Base.Services.BusinessHierarchy
{
    public interface IBusinessUnitsService
    {
        Task<ResultsModel> Delete(string UUID);
        Task<List<EmployeeBaseModel>?> GetBusinessUnits();
        Task<ResultsModel> Save(EmployeeBaseModel businessUnit);
    }

    public class BusinessUnitsService : IBusinessUnitsService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointBusinessUnit = "Employee/BusinessUnits/All";
        private readonly string _endPointBusinessUnitSave = "Employee/BusinessUnits/Save";
        private readonly string _endPointBusinessUnitDelete = "Employee/BusinessUnits/Delete";

        public BusinessUnitsService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        public async Task<List<EmployeeBaseModel>?> GetBusinessUnits()
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;
            var response = await _aPIConnect.GetAsync<List<EmployeeBaseModel>>(Token, $"{_urlBase}{_endPointBusinessUnit}");
            return response;
        }

        public async Task<ResultsModel> Save(EmployeeBaseModel businessUnit)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return new ResultsModel {isValid = false, Message = "Authentication failed." };
            var response = await _aPIConnect.PostAsync<ResultsModel, EmployeeBaseModel>(Token, $"{_urlBase}{_endPointBusinessUnitSave}", businessUnit);
            return response;
        }

        public async Task<ResultsModel> Delete(string UUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };
            var response = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointBusinessUnitDelete}", UUID);
            return response;
        }
    }
}
