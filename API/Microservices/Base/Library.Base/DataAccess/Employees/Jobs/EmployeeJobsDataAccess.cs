using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Employees.Jobs;

internal static class EmployeeJobsDataAccess
{
    public static async Task<IEnumerable<EmployeeJobsModel>> GetEmployeeJobs(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
        await sql.LoadDataAsync<EmployeeJobsModel, dynamic>("[base].[spEmployeeJobsUUID]", basic);

    public static async Task<IEnumerable<TypesModel>> GetEmployeeAges(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
        await sql.LoadDataAsync<TypesModel, dynamic>("[base].[spEmployeeAge]", basic);

    /// <summary>
    /// get the employee base job information
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="basic"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<EmployeeJobsBaseModel>> GetEmployeeBaseJobs(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
        await sql.LoadDataAsync<EmployeeJobsBaseModel, dynamic>("[base].[spEmployeeJobs]", basic);

    /// <summary>
    /// save employee jobs
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<ResultsModel>> SaveEmployeeBaseJobs(ISqlDataAccess sql, EmployeeJobsSaveModel model) =>
        await sql.LoadDataAsync<ResultsModel, EmployeeJobsSaveModel>("[base].[spEmployeeJobsSave]", model);
    

    /// <summary>
    /// delete employee base jobs
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="delete"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<ResultsModel>> DeleteEmployeeBaseJobs(ISqlDataAccess sql, DeleteModel delete)
    {
        if (delete == null || string.IsNullOrEmpty(delete.UUID)) return new List<ResultsModel> { new ResultsModel { isValid = false, Message = "Invalid job selected." } };
        return await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeJobsDelete]", delete);

    }
}
