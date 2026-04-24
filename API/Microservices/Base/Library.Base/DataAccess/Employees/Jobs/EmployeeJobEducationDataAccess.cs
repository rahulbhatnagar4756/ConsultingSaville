using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Database.DAL;

namespace Library.Base.DataAccess.Employees.Jobs;

internal static class EmployeeJobEducationDataAccess
{
    /// <summary>
    /// Get employee job education records by job UUID
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="basic"></param>
    /// <param name="employeeJobsUUID"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<EmployeeJobEducationBaseModel>> GetEmployeeJobEducationByJobUUID(ISqlDataAccess sql, EmployeeBasicGetModel basic, string employeeJobsUUID)
    {
        var parameters = new
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = employeeJobsUUID
        };

        return await sql.LoadDataAsync<EmployeeJobEducationBaseModel, dynamic>("[base].[spEmployeeJobEducation]", parameters);
    }

    /// <summary>
    /// Save employee job education record
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<ResultsModel>> SaveEmployeeJobEducation(ISqlDataAccess sql, EmployeeJobEducationSaveModel model) =>
        await sql.LoadDataAsync<ResultsModel, EmployeeJobEducationSaveModel>("[base].[spEmployeeJobEducationSave]", model);

    /// <summary>
    /// Delete employee job education record
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="delete"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<ResultsModel>> DeleteEmployeeJobEducation(ISqlDataAccess sql, DeleteModel delete)
    {
        if (delete == null || string.IsNullOrEmpty(delete.UUID))
            return new List<ResultsModel> { new ResultsModel { isValid = false, Message = "Invalid education record selected." } };

        return await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeJobEducationDelete]", delete);
    }
}
