using Library.API.Assess.Models;
using Library.API.Assess.Models.Projects;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.API.Assess.Services.Projects
{
    public interface IAssessmentUsersService
    {
        Task<List<CandidateTestModel>?> GetCandidateTestsAsync(BasicGetWithProjectModel model);
    }

    public class AssessmentUsersService : IAssessmentUsersService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointTests = "Assess/Candidate/AssessmentList ";

        public AssessmentUsersService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;

        }

        public async Task<List<CandidateTestModel>?> GetCandidateTestsAsync(
            BasicGetWithProjectModel model)
        {
            if (string.IsNullOrEmpty(model.CompanyUUID) || string.IsNullOrEmpty(model.UsersUUID))
                return null;

            try
            {
                //setup the token
                string? Token = await _token.GetToken();
                if (Token == null) return null;

                var endpoint = $"{_urlBase}{_endPointTests}";
                var response = await _aPIConnect.PostAsync<List<CandidateTestModel>, BasicGetWithProjectModel>(Token, endpoint, model);
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}
