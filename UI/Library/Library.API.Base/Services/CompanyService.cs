using Library.API.Base.Models;
using Library.API.Base.Models.Employees;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Services
{
    public interface ICompanyService
    {
        Task<CompanyBasicModel?> GetCompanyByUUID(string companyUUID);
        Task<CompanyBasicModel?> GetCompanyFromToken();
    }

    public class CompanyService : ICompanyService
    {

        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointCompany = "Company/CompanyByUUID";

        public CompanyService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;

        }

        /// <summary>
        /// get the current logged in users company UUID and return the company information
        /// </summary>
        /// <returns></returns>
        public async Task<CompanyBasicModel?> GetCompanyFromToken()
        {
            var user = await _securityService.GetUserInformation();
            if (user == null) return null;
            return await GetCompanyByUUID(user.CompanyUUID);
        }

        /// <summary>
        /// get the company information by the company UUID
        /// </summary>
        /// <param name="companyUUID"></param>
        /// <returns></returns>
        public async Task<CompanyBasicModel?> GetCompanyByUUID(string companyUUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            Library.API.Base.Models.CompanyBasicModel? Results
                = await _aPIConnect.GetAsync<Library.API.Base.Models.CompanyBasicModel>(Token, $"{_urlBase}{_endPointCompany}/{companyUUID}");

            return Results;
        }


    }
}
