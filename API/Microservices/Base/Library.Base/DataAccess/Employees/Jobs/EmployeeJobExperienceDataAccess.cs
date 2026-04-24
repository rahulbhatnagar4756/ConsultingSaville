using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Database.DAL;

namespace Library.Base.DataAccess.Employees.Jobs;

internal static class EmployeeJobExperienceDataAccess
{
    /// <summary>
    /// Get employee job experience records by job UUID
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="basic"></param>
    /// <param name="employeeJobsUUID"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<EmployeeJobExperienceBaseModel>> GetEmployeeJobExperienceByJobUUID(ISqlDataAccess sql, EmployeeBasicGetModel basic, string employeeJobsUUID)
    {
        var parameters = new
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = employeeJobsUUID
        };

        return await sql.LoadDataAsync<EmployeeJobExperienceBaseModel, dynamic>("[base].[spEmployeeJobExperience]", parameters);
    }

    /// <summary>
    /// Save employee job experience record
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<ResultsModel>> SaveEmployeeJobExperience(ISqlDataAccess sql, EmployeeJobExperienceSaveModel model) =>
        await sql.LoadDataAsync<ResultsModel, EmployeeJobExperienceSaveModel>("[base].[spEmployeeJobExperienceSave]", model);

    /// <summary>
    /// Delete employee job experience record
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="delete"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<ResultsModel>> DeleteEmployeeJobExperience(ISqlDataAccess sql, DeleteModel delete)
    {
        if (delete == null || string.IsNullOrEmpty(delete.UUID))
            return new List<ResultsModel> { new ResultsModel { isValid = false, Message = "Invalid experience record selected." } };

        return await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeJobExperienceDelete]", delete);
    }
}