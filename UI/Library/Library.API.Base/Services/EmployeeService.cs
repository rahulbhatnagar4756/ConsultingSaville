using Library.API.Base.Enumerables;
using Library.API.Base.Models.Employees;
using Library.API.Service;
using Library.Security.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.API.Base.AddOns;
using System.Runtime.CompilerServices;
using Library.API.Base.Models;
using Library.API.Base.Models.BusinessHierarchy;
using Library.API.Base.Models.BusinessHierarchy.EmployeeCriteria;
using Library.API.Base.Models.BusinessHierarchy.EmployeeHierarchy;
using Library.API.Base.Models.BusinessHierarchy.Jobs;

namespace Library.API.Base.Services;

public interface IEmployeeService
{
    Task<ResultsModel?> DeleteEmployeeHierarchy(string employeeHierarchyUUID);
    Task<EmployeeHierarchyListsModel> EmployeeHierarchyListsNoEmployees();
    EmployeeCriteriaResultsModel? FilterEmployeeCriteria(EmployeeFilterTypes filterType, EmployeeCriteriaResultsModel? original, EmployeeCriteriaResultsModel? filterResults = null);
    Task<List<EmployeeBusinessUnitsModel>?> GetEmployeeBusinessUnits();
    Task<List<EmployeeDepartmentsModel>?> GetEmployeeDepartments();
    Task<EmployeeCriteriaResultsModel?> GetEmployeeFilteringCriteria();
    Task<EmployeeBasicModel?> GetEmployeeInformation(string usersUUID);
    Task<List<EmployeeJobDisciplinesModel>?> GetEmployeeJobDisciplines();
    Task<List<EmployeeJobsModel>?> GetEmployeeJobs();
    Task<List<EmployeeCriticalRolesModel>?> GetEmployeeJobsCriticalRoles();
    Task<List<EmployeeLevelsModel>?> GetEmployeeLevels();
    Task<List<EmployeesModel>?> GetEmployees(EmployeeSearchModel filterResults);
    Task<ResultsModel?> SetEmployeeBasicInformationByUUID(EmployeeBasicInformationModel employeeBasicInformation);
    Task<ResultsModel?> SetEmployeeHierarchy(EmployeeHierarchyModel hierarchy);
}

public class EmployeeService : IEmployeeService
{
    private readonly IConfiguration _config;
    private readonly IAPIConnectService _aPIConnect;
    private readonly ITokenService _token;
    private readonly string _urlBase;
    private readonly string _endPointAllCriteria = "Employee/EmployeeCriteria";
    private readonly string _endPointSearch = "Employee/Search";
    private readonly string _endPointEmployeeInformation = "Employee/BasicInformation";
    private readonly string _endPointSaveEmployeeInformation = "Employee/SaveBasicInformation";
    private readonly string _endPointEmployeeBusinessUnit = "Employee/BusinessUnits";
    private readonly string _endPointEmployeeDepartment = "Employee/EmployeeDepartments";
    private readonly string _endPointEmployeeJob = "Employee/EmployeeJobs";
    private readonly string _endPointEmployeeJobCriticalRole = "Employee/CriticalRoles";
    private readonly string _endPointEmployeeJobDisciplines = "Employee/Disciplines";
    private readonly string _endPointEmployeeJobLevel = "Employee/Levels";
    private readonly string _endPointEmployeeHierarchy = "Employee/Hierarchy";
    private readonly string _endPointDeleteEmployeeHierarchy = "Employee/DeleteHierarchy";


    public EmployeeService(IConfiguration config, IAPIConnectService aPIConnect, ITokenService token)
    {
        _config = config;
        _aPIConnect = aPIConnect;
        _token = token;
        _urlBase = _config.GetSection("API:Base:URL").Value;

    }

    public async Task<EmployeeCriteriaResultsModel?> GetEmployeeFilteringCriteria()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        EmployeeCriteriaResultsModel? Results
            = await _aPIConnect.GetAsync<EmployeeCriteriaResultsModel>(Token, $"{_urlBase}{_endPointAllCriteria}");

        return Results;
    }

    /// <summary>
    /// Filter the business unit, department, and finally the positions 
    /// </summary>
    /// <param name="filterType">this is element that caused the event</param>
    /// <param name="original">this is all the original data</param>
    /// <param name="filterResults">this is the data connected to the filter</param>
    /// <returns></returns>
    public EmployeeCriteriaResultsModel? FilterEmployeeCriteria(Enumerables.EmployeeFilterTypes filterType, EmployeeCriteriaResultsModel? original, EmployeeCriteriaResultsModel? filterResults = null)
    {
        if(original == null) return null;
        if (filterResults == null) filterResults = (EmployeeCriteriaResultsModel?)original.Clone();
        switch (filterType)
        {
            case Enumerables.EmployeeFilterTypes.BusinessUnitTypes:
                filterResults.BusinessUnits = original.BusinessUnits.Clone();
                goto case Enumerables.EmployeeFilterTypes.BusinessUnits;
            case Enumerables.EmployeeFilterTypes.BusinessUnits:
                filterResults.Departments = original.Departments.Clone();
                break;
        }
        filterResults.Position = original.Position.Clone();

        filterResults = new Builder.Employee.FilterCriteriaBuilder().FilterModel(filterType, filterResults);

        return filterResults;
    }

    /// <summary>
    /// Get the employees based on the search criteria
    /// </summary>
    /// <param name="search"></param>
    /// <param name="isIncludeTeam"></param>
    /// <param name="filterResults"></param>
    /// <returns></returns>
    public async Task<List<Models.Employees.EmployeesModel>?> GetEmployees(EmployeeSearchModel filterResults)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        List<Models.Employees.EmployeesModel>? Results 
            = await _aPIConnect.PostAsync<List<Models.Employees.EmployeesModel>, EmployeeSearchModel>(Token, $"{_urlBase}{_endPointSearch}", filterResults);

        return Results;
    }

    /// <summary>
    /// get the employees information
    /// Basic User information
    /// The employees Positions in teh company
    /// Basic employee information
    /// </summary>
    /// <param name="usersUUID"></param>
    /// <returns></returns>
    public async Task<Models.Employees.EmployeeBasicModel?> GetEmployeeInformation(string usersUUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        Models.Employees.EmployeeBasicModel? Results
            = await _aPIConnect.GetAsync<Models.Employees.EmployeeBasicModel>(Token, $"{_urlBase}{_endPointEmployeeInformation}/{usersUUID}");
        if(Results == null) Results = new();
        if (Results.UserInformation == null) Results.UserInformation = new();
        if (Results.EmployeeHierarchy == null) Results.EmployeeHierarchy = new();
        if (Results.EmployeeInformation == null) Results.EmployeeInformation = new();

        return Results;
    }

    /// <summary>
    /// Set the employee basic information
    /// </summary>
    /// <param name="employeeBasicInformation"></param>
    /// <returns></returns>
    public async Task<Models.ResultsModel?> SetEmployeeBasicInformationByUUID(EmployeeBasicInformationModel employeeBasicInformation)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        Models.ResultsModel? Results
            = await _aPIConnect.PostAsync<Models.ResultsModel, EmployeeBasicInformationModel>(Token, $"{_urlBase}{_endPointSaveEmployeeInformation}", employeeBasicInformation);
        return Results;

    }

    /// <summary>
    /// get all the business units for the logged in company
    /// </summary>
    /// <returns></returns>
    public async Task<List<EmployeeBusinessUnitsModel>?> GetEmployeeBusinessUnits()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;

        List<EmployeeBusinessUnitsModel>? Results
            = await _aPIConnect.GetAsync<List<EmployeeBusinessUnitsModel>?>(Token, $"{_urlBase}{_endPointEmployeeBusinessUnit}");

        if (Results == null) return null; 
        return Results;
    }

    /// <summary>
    /// get all the departments for the logged in company
    /// </summary>
    /// <returns></returns>
    public async Task<List<EmployeeDepartmentsModel>?> GetEmployeeDepartments()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        List<EmployeeDepartmentsModel>? Results
            = await _aPIConnect.GetAsync<List<EmployeeDepartmentsModel>?>(Token, $"{_urlBase}{_endPointEmployeeDepartment}");
        if (Results == null) return null;
        return Results;
    }

    /// <summary>
    /// get all the jobs for the logged in company
    /// </summary>
    /// <returns></returns>
    public async Task<List<EmployeeJobsModel>?> GetEmployeeJobs()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        List<EmployeeJobsModel>? Results
            = await _aPIConnect.GetAsync<List<EmployeeJobsModel>?>(Token, $"{_urlBase}{_endPointEmployeeJob}");
        if (Results == null) return null;
        return Results;
    }

    public async Task<List<EmployeeCriticalRolesModel>?> GetEmployeeJobsCriticalRoles()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        List<EmployeeCriticalRolesModel>? Results
            = await _aPIConnect.GetAsync<List<EmployeeCriticalRolesModel>?>(Token, $"{_urlBase}{_endPointEmployeeJobCriticalRole}");
        if (Results == null) return null;
        return Results;
    }


    public async Task<List<EmployeeJobDisciplinesModel>?> GetEmployeeJobDisciplines()
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        List<EmployeeJobDisciplinesModel>? Results
            = await _aPIConnect.GetAsync<List<EmployeeJobDisciplinesModel>?>(Token, $"{_urlBase}{_endPointEmployeeJobDisciplines}");
        if (Results == null) return null;
        return Results;
    }

    public async Task<List<EmployeeLevelsModel>?> GetEmployeeLevels()
    { 
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        List<EmployeeLevelsModel>? Results
            = await _aPIConnect.GetAsync<List<EmployeeLevelsModel>?>(Token, $"{_urlBase}{_endPointEmployeeJobLevel}");
        if (Results == null) return null;
        return Results;
    }

    /// <summary>
    /// gets the dropdown data for the employee hierarchy excluding the employee information 
    /// employee information will be called when the user types in a name or id number or ...
    /// </summary>
    /// <returns></returns>
    public async Task<EmployeeHierarchyListsModel> EmployeeHierarchyListsNoEmployees()
    {
        EmployeeHierarchyListsModel HierarchyLists = new EmployeeHierarchyListsModel();

        var BusinessUnitsTask = GetEmployeeBusinessUnits();
        var DepartmentsTask = GetEmployeeDepartments();
        var JobsTask = GetEmployeeJobs();
        var CriticalRolesTask = GetEmployeeJobsCriticalRoles();
        var DisciplinesTask = GetEmployeeJobDisciplines();
        var LevelsTask = GetEmployeeLevels();

        //wait for all tasks to finish
        await Task.WhenAll(BusinessUnitsTask, DepartmentsTask, JobsTask, CriticalRolesTask, DisciplinesTask, LevelsTask);

        HierarchyLists.BusinessUnits = BusinessUnitsTask.Result;
        HierarchyLists.Departments = DepartmentsTask.Result;
        HierarchyLists.Jobs = JobsTask.Result;
        HierarchyLists.CriticalRoles = CriticalRolesTask.Result;
        HierarchyLists.Disciplines = DisciplinesTask.Result;
        HierarchyLists.Levels = LevelsTask.Result;

        return HierarchyLists;

    }


    public async Task<ResultsModel?> SetEmployeeHierarchy(EmployeeHierarchyModel hierarchy)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        ResultsModel? Results
            = await _aPIConnect.PostAsync<ResultsModel, EmployeeHierarchyModel>(Token, $"{_urlBase}{_endPointEmployeeHierarchy}", hierarchy);
        if (Results == null) return null;
        return Results;
    }

    public async Task<ResultsModel?> DeleteEmployeeHierarchy(string employeeHierarchyUUID)
    {
        string? Token = await _token.GetToken();
        if (Token == null) return null;
        ResultsModel? Results
            = await _aPIConnect.DeleteAsync<ResultsModel>(Token, $"{_urlBase}{_endPointDeleteEmployeeHierarchy}/{employeeHierarchyUUID}","");
        if (Results == null) return null;
        return Results;
    }

}