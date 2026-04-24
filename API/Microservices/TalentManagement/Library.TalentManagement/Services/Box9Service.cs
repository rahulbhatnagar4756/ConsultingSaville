using Library.Base.Models.Employees;
using Library.Database.DAL;
using Library.TalentManagement.Models.Box9;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.TalentManagement.DataAccess;
using Library.TalentManagement.Models;
using Library.TalentManagement.Models.SuccessionReadiness;
using System.Data;

namespace Library.TalentManagement.Services;

public interface IBox9Service
{
    Task<TalentManagementStatisticModel?> GetStatisticsFilter(string companyUUID, string usersUUIDLoggedin, EmployeeSearchModel search);
    Task<TalentManagementUsersStatisticModel> GetStatisticsForUser(string companyUUID, string usersUUIDLoggedIn, string usersUUID);
    Task<List<SuccessionPlanningGenericModel>?> GetSuccessionPlannings(string companyUUID, string usersUUIDLoggedIn, SearchModel search);
    Task<DataTable?> GetSuccessionPlanningsToDataTable(string companyUUID, string usersUUIDLoggedIn, SearchModel search);
    Task<List<SuccessionPlanningModel>?> GetSuccessionPlanningsToRaw(string companyUUID, string usersUUIDLoggedIn, SearchModel search);
    Task<SuccessionReadinessModel?> GetSuccessionReadinessRoleInformation(string companyUUID, string usersUUIDLoggedIn, string employeeJobsGenericNamesUUID, EmployeeSearchModel? Search);
    Task<TalentManagementUserInformationModel?> GetTalentManagementUserInformation(string companyUUID, string usersUUIDLoggedIn, string usersUUID);
}

public class Box9Service : IBox9Service
{
    private readonly ISqlDataAccess _sql;

    public Box9Service(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// gets all the employees for the specified company and search criteria
    /// And counts the totals for each 9 box 
    /// </summary>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    public async Task<TalentManagementStatisticModel?> GetStatisticsFilter(string companyUUID, string usersUUIDLoggedIn, EmployeeSearchModel search)
    {
        string json = JsonConvert.SerializeObject(search);
        IEnumerable<TalentManagementStatisticModel>? data = await DataAccess.Box9.Box9DataAccess.Statistics(_sql, companyUUID, usersUUIDLoggedIn, json);
        if (data == null || data.Count() == 0) return null;
        return data.FirstOrDefault();
    }

    /// <summary>
    /// gets the 9 box statistics for a specific user 
    /// Using the users Job Match, CPP and the Final Goal Score there Current Positions in the origination 
    /// </summary>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="usersUUID"></param>
    /// <returns></returns>
    public async Task<TalentManagementUsersStatisticModel?> GetStatisticsForUser(string companyUUID, string usersUUIDLoggedIn, string usersUUID)
    {
        var data = await DataAccess.Box9.Box9DataAccess.Box9ResultStatisticsUser(_sql, companyUUID, usersUUIDLoggedIn, usersUUID);
        if(data == null) return null;
        TalentManagementUsersStatisticModel TM = new() { CPPMatch = data?.CPPCode ?? string.Empty,
                                                         FinalGoalScore = (int)Math.Round(data?.FinalGoalScore ?? 0, 0, MidpointRounding.AwayFromZero),
                                                         PersonalityMatch = data?.JobMatchCode,
                                                         Box9Types = data?.Box9Id,
                                                         Box9TypeActions = data?.InformationHTML ?? string.Empty,
                                                         Year = data?.Year ?? 0
        };

        return TM;
    }

    /// <summary>
    /// gets the employees information from the IDP and Succession Planning AND user companies tables 
    /// </summary>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="usersUUID"></param>
    /// <returns></returns>
    public async Task<TalentManagementUserInformationModel?> GetTalentManagementUserInformation(string companyUUID, string usersUUIDLoggedIn, string usersUUID) =>
        await DataAccess.Box9.Box9DataAccess.TalentManagementUserInformation(_sql, companyUUID, usersUUIDLoggedIn, usersUUID);


    private async Task<List<SuccessionPlanningModel>?> GetSuccessionPlanningsData(string companyUUID, string usersUUIDLoggedIn, SearchModel search)
    {
        string json = JsonConvert.SerializeObject(search);
        bool isOnlyAvailableJobs = false;
        if (search.isOnlyAvailableJobs != null) isOnlyAvailableJobs = search.isOnlyAvailableJobs.Value;

        IEnumerable<SuccessionPlanningModel>? data = await SuccessionPlanningDataAccess.GetSuccessionPlanning(_sql, companyUUID, usersUUIDLoggedIn, isOnlyAvailableJobs, json);
        if (data == null || data.Count() == 0) return new List<SuccessionPlanningModel>();
        return data.ToList();
    }

    /// <summary>
    /// Get SuccessionPlanning data for the specified company and search criteria
    /// </summary>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    public async Task<List<SuccessionPlanningGenericModel>?> GetSuccessionPlannings(string companyUUID, string usersUUIDLoggedIn, SearchModel search)
  {
        List<SuccessionPlanningModel>? data = await GetSuccessionPlanningsData( companyUUID, usersUUIDLoggedIn, search);
        if (data == null || data.Count() == 0) return null;
        var results = Mappers.SuccessionPlanningMapper.MapSuccessionPlanningToSuccessionPlanningGeneric(data);
        return results;

    }

    /// <summary>
    /// Get SuccessionPlanning data into DataTable for the specified company and search criteria 
    /// </summary>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    public async Task<List<SuccessionPlanningModel>?> GetSuccessionPlanningsToRaw(string companyUUID, string usersUUIDLoggedIn, SearchModel search)
    {
        List<SuccessionPlanningModel>? data = await GetSuccessionPlanningsData(companyUUID, usersUUIDLoggedIn, search);
        if (data == null || data.Count() == 0) return null;
        return data;
    }

    /// <summary>
    /// Get SuccessionPlanning data into DataTable for the specified company and search criteria 
    /// </summary>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    public async Task<System.Data.DataTable?> GetSuccessionPlanningsToDataTable(string companyUUID, string usersUUIDLoggedIn, SearchModel search)
    {
        List<SuccessionPlanningModel>? data = await GetSuccessionPlanningsData(companyUUID, usersUUIDLoggedIn, search);
        if (data == null || data.Count() == 0) return null;
        var results = Mappers.SuccessionPlanningMapper.MapSuccessionPlanningToDatatable(data);
        return results;
    }




    public async Task<SuccessionReadinessModel?> GetSuccessionReadinessRoleInformation(string companyUUID, string usersUUIDLoggedIn, string employeeJobsGenericNamesUUID, EmployeeSearchModel? Search)
    {
        SuccessionReadinessModel results = new();

        string json = Search != null ? JsonConvert.SerializeObject(Search) : string.Empty;


        var currentTask = SuccessionPlanningDataAccess.SuccessionReadinessRoleUsersCurrent(_sql, employeeJobsGenericNamesUUID, usersUUIDLoggedIn, companyUUID, json);
        var potentialTask = SuccessionPlanningDataAccess.SuccessionReadinessRoleUsersPotential(_sql, employeeJobsGenericNamesUUID, usersUUIDLoggedIn, companyUUID, json);
        var roleTask = SuccessionPlanningDataAccess.SuccessionReadinessRole(_sql, employeeJobsGenericNamesUUID, usersUUIDLoggedIn, companyUUID, json);
        var VulnerabilityTask = SuccessionPlanningDataAccess.SuccessionReadinessRoleVulnerability(_sql, employeeJobsGenericNamesUUID, usersUUIDLoggedIn, companyUUID, json);
        
        Task.WaitAll(currentTask, potentialTask, roleTask, VulnerabilityTask);
        results.Role = roleTask.Result;
        results.EmployeeCurrent = currentTask.Result;
        results.EmployeePotential = potentialTask.Result;
        results.Vulnerability = VulnerabilityTask.Result;

        return results;
    }
    }
