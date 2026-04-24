
using Library.Database.DAL;
using Library.TalentManagement.Models;
using Library.TalentManagement.Models.SuccessionReadiness;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.DataAccess
{
    internal static class SuccessionPlanningDataAccess
    {

//        Alter PROCEDURE[TM].[spSuccessionReadiness_Filter_Json]
//        @CompanyUUID NVARCHAR(200)
//, @UsersUUIDLoggedIn NVARCHAR(200)
//, @isOnlyAvailableJobs bit = 0
//, @json NVARCHAR(MAX) = NULL
//AS
//BEGIN
        public static async Task<IEnumerable<SuccessionPlanningModel>?> GetSuccessionPlanning (ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn, bool isOnlyAvailableJobs, string json)
        {
            var p = new
            {
                CompanyUUID = companyUUID,
                UsersUUIDLoggedIn = usersUUIDLoggedIn,
                IsOnlyAvailableJobs = isOnlyAvailableJobs,
                Json = json
            };
            var result = await sql.LoadDataAsync<SuccessionPlanningModel, dynamic>("[TM].[spSuccessionReadiness_Filter_Json]", p);
            return result;
        }


        //CREATE PROCEDURE [TM].[spSuccessionReadiness_Role_Employee_Potential]
      //  @EmployeeJobsGenericNamesUUID NVARCHAR(200)
  //, @UsersUUIDLoggedIn NVARCHAR(200)
  //, @CompanyUUID NVARCHAR(200)
//AS
//BEGIN

        public static async Task<List<SuccessionReadinessRoleUsersPotentialModel>?> SuccessionReadinessRoleUsersPotential(ISqlDataAccess sql, string employeeJobsGenericNamesUUID, string usersUUIDLoggedIn, string companyUUID, string Search)
        {
            var p = new
            {
                EmployeeJobsGenericNamesUUID = employeeJobsGenericNamesUUID,
                UsersUUIDLoggedIn = usersUUIDLoggedIn,
                CompanyUUID = companyUUID,
                Json = Search
            };
            var result = await sql.LoadDataAsync<SuccessionReadinessRoleUsersPotentialModel, dynamic>("[TM].[spSuccessionReadiness_Role_Employee_Potential]", p);
            return result?.ToList()??null;
        }

        public static async Task<List<SuccessionReadinessRoleUsersCurrentModel>?> SuccessionReadinessRoleUsersCurrent(ISqlDataAccess sql, string employeeJobsGenericNamesUUID, string usersUUIDLoggedIn, string companyUUID, string Search)
        {
            var p = new
            {
                EmployeeJobsGenericNamesUUID = employeeJobsGenericNamesUUID,
                UsersUUIDLoggedIn = usersUUIDLoggedIn,
                CompanyUUID = companyUUID,
                Json = Search
            };
            var result = await sql.LoadDataAsync<SuccessionReadinessRoleUsersCurrentModel, dynamic>("[TM].[spSuccessionReadiness_Role_Employee_Current]", p);
            return result?.ToList() ?? null;
        }

        public static async Task<SuccessionReadinessRolesModel?> SuccessionReadinessRole(ISqlDataAccess sql, string employeeJobsGenericNamesUUID, string usersUUIDLoggedIn, string companyUUID, string Search)
        {
            var p = new
            {
                EmployeeJobsGenericNamesUUID = employeeJobsGenericNamesUUID,
                UsersUUIDLoggedIn = usersUUIDLoggedIn,
                CompanyUUID = companyUUID 
            };
            var result = await sql.LoadFirstDataAsync<SuccessionReadinessRolesModel, dynamic>("[TM].[spSuccessionReadiness_Role]", p);
            return result;
        }

        
        public static async Task<SuccessionReadinessRoleVulnerabilityModel?> SuccessionReadinessRoleVulnerability(ISqlDataAccess sql, string employeeJobsGenericNamesUUID, string usersUUIDLoggedIn, string companyUUID, string Search)
        {
            var p = new
            {
                EmployeeJobsGenericNamesUUID = employeeJobsGenericNamesUUID,
                UsersUUIDLoggedIn = usersUUIDLoggedIn,
                CompanyUUID = companyUUID,
                Json = Search
            };
            var result = await sql.LoadFirstDataAsync<SuccessionReadinessRoleVulnerabilityModel, dynamic>("[TM].[spSuccessionReadiness_Role_Vulnerability]", p);
            return result;
        }



    }
}
