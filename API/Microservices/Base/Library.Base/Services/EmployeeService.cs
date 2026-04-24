using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library.Base.Services;

public interface IEmployeeService
{
    Task<EmployeeSearchResultsModel> GetAllSearchCriteria(EmployeeBasicGetModel Search);
    Task<EmployeeBasicModel?> GetEmployeeBasicInformation(string CompanyUUID, string UsersUUIDLoggedIn, string usersUUID);
    Task<List<EmployeeModel>?> SearchEmployees(string CompanyUUID, string UsersUUIDLoggedIn, EmployeeSearchModel search);
    Task<ResultsModel?> SetEmployeeBasicInformation(EmployeeBasicInformationModel employeeInformation);
}

public class EmployeeService : IEmployeeService
{
    private readonly ISqlDataAccess _sql;
    private readonly IUsersService _usersService;

    public EmployeeService(ISqlDataAccess sql, IUsersService usersService)
    {
        _sql = sql;
        _usersService = usersService;
    }

    /// <summary>
    /// get all the criteria data need to filter the employee data
    /// </summary>
    /// <param name="Search"></param>
    /// <returns></returns>
    public async Task<EmployeeSearchResultsModel> GetAllSearchCriteria(EmployeeBasicGetModel Search)
    {
        BusinessLogic.Employees.CriteriaBuilder criteriaBuilder = new BusinessLogic.Employees.CriteriaBuilder(_sql);
        return await criteriaBuilder.GetCriteriaData(Search);
    }

    public async Task<List<EmployeeModel>?> SearchEmployees(string CompanyUUID, string UsersUUIDLoggedIn, EmployeeSearchModel search)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(search);
        var employeeSearch = await Base.DataAccess.Employees.EmployeesDataAccess.EmployeeFilterByJson(_sql,CompanyUUID, UsersUUIDLoggedIn, json);
        return employeeSearch?.ToList() ?? null;
    }

    /// <summary>
    /// get the employees basic information
    /// user information
    /// the current job positions
    /// the basic employment information
    /// </summary>
    /// <param name="CompanyUUID"></param>
    /// <param name="UsersUUIDLoggedIn"></param>
    /// <param name="usersUUID"></param>
    /// <returns></returns>
    public async Task<EmployeeBasicModel?> GetEmployeeBasicInformation(string CompanyUUID, string UsersUUIDLoggedIn, string usersUUID)
    {
        EmployeeBasicModel? data = new EmployeeBasicModel();

        //call 3 async methods at once and wait for all of them to completes
        var userBasicInfoTask = _usersService.GetUserBasicInformationByUUID(CompanyUUID, UsersUUIDLoggedIn, usersUUID);
        var employeeHierarchyTask = DataAccess.Employees.EmployeeHierarchyDataAccess.EmployeeHierarchyByUsersUUID(_sql, CompanyUUID, UsersUUIDLoggedIn, usersUUID, false);
        var employeeInfoTask = DataAccess.Employees.EmployeesDataAccess.EmployeeInformationByUsersUUID(_sql, CompanyUUID, UsersUUIDLoggedIn, usersUUID);

        // Wait for all tasks to complete
        await Task.WhenAll(userBasicInfoTask, employeeHierarchyTask, employeeInfoTask);

        // Assign the results to the data model
        var Hierarchy = await employeeHierarchyTask;
        if(Hierarchy != null) data.EmployeeHierarchy = Hierarchy?.ToList()??null;
        data.UserInformation = await userBasicInfoTask;
        data.EmployeeInformation = await employeeInfoTask;

        return data;
    }


    public async Task<Models.ResultsModel?> SetEmployeeBasicInformation(EmployeeBasicInformationModel employeeInformation) =>
        await DataAccess.Employees.EmployeesDataAccess.SaveEmployeeBasicInformation(_sql, employeeInformation);

}
