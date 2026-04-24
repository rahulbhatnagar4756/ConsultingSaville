using Library.API.Base.Mappers;
using Library.API.Base.Models;
using Library.API.Base.Models.BusinessHierarchy;
using Library.API.Base.Models.BusinessHierarchy.Jobs;
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
    public interface IJobDisciplinesService
    {
        Task<List<EmployeeJobDisciplineBaseModel>?> GetJobDisciplines();
        Task<ResultsModel> Save(EmployeeJobDisciplineBaseModel discipline);
        Task<ResultsModel> Delete(string UUID);
        Task<List<EmployeeBaseModel>?> GetJobDisciplinesToEmployeeBaseModel();
    }

    public class JobDisciplinesService : IJobDisciplinesService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly string _urlBase;
        private readonly string _endPointJobDisciplines = "Employee/JobDiscipline/All";
        private readonly string _endPointJobDisciplinesSave = "Employee/JobDiscipline/Save";
        private readonly string _endPointJobDisciplinesDelete = "Employee/JobDiscipline/Delete";

        public JobDisciplinesService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token, ISecurityService securityService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _urlBase = _config.GetSection("API:Base:URL").Value;
        }

        /// <summary>
        /// Get all job disciplines for the logged-in user's company
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeJobDisciplineBaseModel>?> GetJobDisciplines()
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            var response = await _aPIConnect.GetAsync<List<EmployeeJobDisciplineBaseModel>>(Token, $"{_urlBase}{_endPointJobDisciplines}");
            return response;
        }

        public async Task<List<EmployeeBaseModel>?> GetJobDisciplinesToEmployeeBaseModel()
        {
            var disciplines = await GetJobDisciplines();
            if (disciplines == null || disciplines.Count == 0) return null;
            // Convert EmployeeJobDisciplineBaseModel to EmployeeBaseModel format
            return disciplines.ToEmployeeBaseModel();
        }

        /// <summary>
        /// Save (create or update) a job discipline
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
        public async Task<ResultsModel> Save(EmployeeJobDisciplineBaseModel discipline)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var response = await _aPIConnect.PostAsync<ResultsModel, EmployeeJobDisciplineBaseModel>(Token, $"{_urlBase}{_endPointJobDisciplinesSave}", discipline);
            return response ?? new ResultsModel { isValid = false, Message = "Save operation failed." };
        }

        /// <summary>
        /// Delete a job discipline by UUID
        /// </summary>
        /// <param name="UUID"></param>
        /// <returns></returns>
        public async Task<ResultsModel> Delete(string UUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return new ResultsModel { isValid = false, Message = "Authentication failed." };

            var response = await _aPIConnect.PostAsync<ResultsModel, string>(Token, $"{_urlBase}{_endPointJobDisciplinesDelete}", UUID);
            return response ?? new ResultsModel { isValid = false, Message = "Delete operation failed." };
        }
    }
}
