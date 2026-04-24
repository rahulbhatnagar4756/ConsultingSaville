using Library.API.Assess.Models;
using Library.API.Assess.Models.Jobs;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Assess.Services.Jobs
{
    public interface IJobsService
    {
        Task<ResultsModel> Delete(string UUID);
        Task<List<JobsBaseModel>?> GetJobs();
        Task<ResultsModel> Save(JobsBaseModel job);
    }

    public class JobsService : IJobsService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointJobs = "Assess/Jobs/All";
        private readonly string _endPointJobsSave = "Assess/Jobs/Save";
        private readonly string _endPointJobsDelete = "Assess/Jobs/Delete";

        public JobsService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        public async Task<List<JobsBaseModel>?> GetJobs()
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            var response = await _aPIConnect.GetAsync<List<JobsBaseModel>>(Token, $"{_urlBase}{_endPointJobs}");
            return response;
        }

        public async Task<ResultsModel> Save(JobsBaseModel job)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var response = await _aPIConnect.PostAsync<ResultsModel, JobsBaseModel>(Token, $"{_urlBase}{_endPointJobsSave}", job);
            return response ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        public async Task<ResultsModel> Delete(string UUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var response = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointJobsDelete}", UUID);
            return response ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }
    }
}