using Library.API.Admin.Models;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Admin.Services
{
    public interface IIconsService
    {
        Task<List<NameOnlyModel>?> GetIcons();
    }

    public class IconsService : IIconsService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointIcons = "Admin/Icons/All";

        public IconsService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        public async Task<List<NameOnlyModel>?> GetIcons()
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;
            var response = await _aPIConnect.GetAsync<List<NameOnlyModel>>(Token, $"{_urlBase}{_endPointIcons}");
            return response;
        }
    }
}
