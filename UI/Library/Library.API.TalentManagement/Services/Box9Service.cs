using Library.API.Base.Models.Employees;
using Library.API.Base.Services;
using Library.API.Service;
using Library.API.TalentManagement.Mapper;
using Library.API.TalentManagement.Mapper.Box9;
using Library.API.TalentManagement.Models;
using Library.API.TalentManagement.Models.Box9;
using Library.API.TalentManagement.Models.SuccessionReadiness;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.TalentManagement.Services
{
    public interface IBox9Service
    {
        Task<List<Box9EmployeesModel>?> GetBox9Employees(EmployeeSearchModel search);
        Task<TalentManagementStatisticModel?> GetBox9Statistics(EmployeeSearchModel search);
        Task<TalentManagementUsersStatisticModel> GetBox9StatisticsForUser(string usersUUID);
        Task<List<SuccessionPlanningModel>?> GetSuccessionPlannings( SearchModel search);
        Task<DataTable?> GetSuccessionPlanningsToDataTable(SearchModel search);
        Task<TalentManagementUserInformationModel?> GetTalentManagementUserInformation(string usersUUID);
        Task<SuccessionReadinessModel?> SuccessionReadinessRoleInformation(string employeeJobsGenericNamesUUID, EmployeeSearchModel Search);
    }

    public class Box9Service : IBox9Service
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _aPIConnect;
        private readonly ITokenService _token;
        private readonly ISecurityService _securityService;
        private readonly IEmployeeService _employeeService;
        private readonly ICompanyService _companyService;
        private readonly string _urlBase;
        private readonly string _endPointBox9Stats = "Box9/Statistics";
        private readonly string _endPointBox9StatsUser = "Box9/Statistics/User";
        private readonly string _endPointUserInformation = "Box9/Statistics/UserInformation";
        private readonly string _endPointSuccessionPlanningSearch = "SuccessionPlanning/Search";
        private readonly string _endPointSuccessionPlanningSearchReturnDataTable = "SuccessionPlanning/Search/Return/DataTable";
        private readonly string _endPointSuccessionPlanningSearchReturnRaw = "SuccessionPlanning/Search/Return/Raw";
        private readonly string _endPointSuccessionReadinessRole = "SuccessionReadiness/RoleInformation";


        public Box9Service(IConfiguration config,
                           IAPIConnectService aPIConnect,
                           ITokenService token,
                           ISecurityService securityService,
                           IEmployeeService employeeService)
        {
            _config = config;
            _aPIConnect = aPIConnect;
            _token = token;
            _securityService = securityService;
            _employeeService = employeeService;
            _urlBase = _config.GetSection("API:Base:URL").Value;

        }

        /// <summary>
        /// get the current logged in users information
        /// </summary>
        /// <returns></returns>
        public async Task<TalentManagementStatisticModel?> GetBox9Statistics(Library.API.Base.Models.Employees.EmployeeSearchModel search)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            TalentManagementStatisticModel? Results
           = await _aPIConnect.PostAsync<TalentManagementStatisticModel, Library.API.Base.Models.Employees.EmployeeSearchModel>(Token, $"{_urlBase}{_endPointBox9Stats}", search);

            return Results;
        }

        public async Task<List<Models.Box9.Box9EmployeesModel>?> GetBox9Employees(Library.API.Base.Models.Employees.EmployeeSearchModel search)
        {
            var Employees = await _employeeService.GetEmployees(search);
            if(Employees == null) return null;
            var EmployeesLevels = Employees.MapperEmployeeModelToBox9Employees();
            if (EmployeesLevels == null) return null;
            return EmployeesLevels;
        }

        /// <summary>
        /// get the 9 box statistics for a specific user
        /// </summary>
        /// <param name="usersUUID"></param>
        /// <returns></returns>
        public async Task<TalentManagementUsersStatisticModel> GetBox9StatisticsForUser(string usersUUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            TalentManagementUsersStatisticModel? Results = await _aPIConnect.GetAsync<TalentManagementUsersStatisticModel>(Token, $"{_urlBase}{_endPointBox9StatsUser}/{usersUUID}");
            return Results;
        }

        /// <summary>
        /// gets the employees information from the IDP and Succession Planning status and user companies tables  
        /// </summary>
        /// <param name="usersUUID"></param>
        /// <returns></returns>
        public async Task<TalentManagementUserInformationModel?> GetTalentManagementUserInformation(string usersUUID)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            TalentManagementUserInformationModel? Results = await _aPIConnect.GetAsync<TalentManagementUserInformationModel>(Token, $"{_urlBase}{_endPointUserInformation}/{usersUUID}");
            return Results;
        }

        /// <summary>
        /// get the succession planning information for the users that are apart of the search
        /// </summary>
        /// <param name="companyUUID"></param>
        /// <param name="usersUUID"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<SuccessionPlanningModel>?> GetSuccessionPlannings(SearchModel search)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;
            List<SuccessionPlanningModel>? Results = await _aPIConnect.PostAsync<List<SuccessionPlanningModel>, SearchModel>(Token, $"{_urlBase}{_endPointSuccessionPlanningSearch}", search);
            return Results;
        }

        public async Task<DataTable?> GetSuccessionPlanningsToDataTable(SearchModel search)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;
            var Results  = await _aPIConnect.PostAsync<List<SuccessionPlanningRawModel>, SearchModel>(Token, $"{_urlBase}{_endPointSuccessionPlanningSearchReturnRaw}", search);
            //convert the results to a datatable
            DataTable table = Results.MapSuccessionPlanningToDataTable();
            return table;
        }

        /// <summary>
        /// get the succession readiness role information for the specified employeeJobsGenericNamesUUID
        /// </summary>
        /// <param name="employeeJobsGenericNamesUUID"></param>
        /// <returns></returns>
        public async Task<SuccessionReadinessModel?> SuccessionReadinessRoleInformation(string employeeJobsGenericNamesUUID, EmployeeSearchModel? search)
        {
            string? Token = await _token.GetToken();
            if (Token == null) return null;

            UUIDSearchModel UUIDModel = new UUIDSearchModel
            {
                UUID = employeeJobsGenericNamesUUID,
                Search = search
            };

            SuccessionReadinessModel? Results = await _aPIConnect.PostAsync<SuccessionReadinessModel, UUIDSearchModel>(Token, $"{_urlBase}{_endPointSuccessionReadinessRole}", UUIDModel);
            return Results;
        }

    }
}
