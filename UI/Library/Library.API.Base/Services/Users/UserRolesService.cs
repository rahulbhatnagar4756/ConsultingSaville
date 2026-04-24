using Library.API.Base.Models;
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
    public interface IUserRolesService
    {
        Task<List<RoleBasicModel>?> GetUserRolesByUsersUUID(string usersUUID);
        Task<List<RoleBasicModel>?> GetUserRolesFromTokenUsersUUID();
    }

    public class UserRolesService : IUserRolesService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly ICompanyService _companyService;
        private readonly string _urlBase;
        private readonly string _endPointUserRoless = "Users/RolesByUsersUUID";

        public UserRolesService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService, ICompanyService companyService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _companyService = companyService;
            _urlBase = _config.GetSection("API:Base:URL").Value;

        }

        /// <summary>
        /// get the current logged in users information
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleBasicModel>?> GetUserRolesFromTokenUsersUUID()
        {
            var user = await _securityService.GetUserInformation();
            if (user == null) return null;
            return await GetUserRolesByUsersUUID(user.UsersUUID);
        }

        /// <summary>
        /// get the company information by the company UUID
        /// </summary>
        /// <param name="companyUUID"></param>
        /// <returns></returns>
        public async Task<List<RoleBasicModel>?> GetUserRolesByUsersUUID(string usersUUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            List<RoleBasicModel>? Results
                = await _aPIConnect.GetAsync<List<RoleBasicModel>?>(Token, $"{_urlBase}{_endPointUserRoless}/{usersUUID}");

            return Results;
        }



    }
}
