using Library.API.Assess.Models.Projects;
using Library.API.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Services.Projects;

public interface ICandidateLoginService
{
    Task<CandidateAutoLoginResult> ValidateCandidateAutoLoginToken(string token);
}

public class CandidateLoginService : ICandidateLoginService
{
    private readonly IConfiguration _config;
    private readonly IAPIConnectService _aPIConnect;
    private readonly string _urlBase;
    private readonly string _endPointAutoLogin = "Assess/Projects/CandidateRegistration/CandidateAutoLoginWithToken";

    public CandidateLoginService(IConfiguration config, IAPIConnectService aPIConnect)
    {
        _config = config;
        _aPIConnect = aPIConnect;
        _urlBase = _config.GetSection("API:Base:URL").Value;
    }

    public async Task<CandidateAutoLoginResult> ValidateCandidateAutoLoginToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return new CandidateAutoLoginResult
            {
                IsValid = false,
                Message = "Token is required for assessment access."
            };

        try
        {
            var response = await _aPIConnect.GetAsync<CandidateAutoLoginResult>(
                $"{_urlBase}{_endPointAutoLogin}/{token}");

            return response ?? new CandidateAutoLoginResult
            {
                IsValid = false,
                Message = "Failed to validate token. Please try again."
            };
        }
        catch (Exception ex)
        {
            return new CandidateAutoLoginResult
            {
                IsValid = false,
                Message = "An unexpected error occurred during token validation."
            };
        }
    }

}
