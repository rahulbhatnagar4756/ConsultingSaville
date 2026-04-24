using Library.API.Base.Models.Users;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Services.Users
{
    public interface IUserDataService
    {
        Task<UserDataBasicModel?> GetUserDataInformationByUsersUUID(string usersUUID);
    }

    public class UserDataService : IUserDataService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly ICompanyService _companyService;
        private readonly IUserRolesService _userRolesService;
        private readonly string _urlBase;
        private readonly string _endPointUserDataInformation = "Users/Data/EmployeeInformation";

        public UserDataService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token
            , ISecurityService securityService, ICompanyService companyService, IUserRolesService userRolesService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _companyService = companyService;
            _userRolesService = userRolesService;
            _urlBase = _config.GetSection("API:Base:URL").Value;

        }

        public async Task<UserDataBasicModel?> GetUserDataInformationByUsersUUID(string usersUUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            UserDataBasicModel? Results
                = await _aPIConnect.GetAsync<UserDataBasicModel>(Token, $"{_urlBase}{_endPointUserDataInformation}/{usersUUID}");

            return Results;
        }

    }
}
