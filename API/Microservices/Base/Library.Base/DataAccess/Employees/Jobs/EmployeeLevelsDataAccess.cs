using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Employees.Jobs;

internal static class EmployeeLevelsDataAccess
{
    public static async Task<IEnumerable<TypesModel>> GetEmployeeLevels(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
        await sql.LoadDataAsync<TypesModel, dynamic>("[base].[spEmployeeLevelsUUID]", basic);

    /// <summary>
    /// Retrieves a collection of employee level base models from the database.
    /// </summary>
    /// <param name="sql">The SQL data access instance used to execute the database query.</param>
    /// <param name="basic">The basic employee information used as parameters for the query.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of <see
    /// cref="EmployeeLevelBaseModel"/>.</returns>
    public static async Task<IEnumerable<EmployeeLevelBaseModel>> GetEmployeeLevelsBase(ISqlDataAccess sql, EmployeeBasicGetModel basic) =>
        await sql.LoadDataAsync<EmployeeLevelBaseModel, dynamic>("[base].[spEmployeeLevelsBase]", basic);

    /// <summary>
    /// save level
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="save"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<ResultsModel>> SaveEmployeeLevels(ISqlDataAccess sql, EmployeeLevelSaveModel save) =>
        await sql.LoadDataAsync<ResultsModel, dynamic>("[base].[spEmployeeLevelsSave]", save);

    /// <summary>
    /// delete level
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="delete"></param>
    /// <returns></returns>
    internal static async Task<IEnumerable<ResultsModel>> DeleteEmployeeLevels(ISqlDataAccess sql, DeleteModel delete) =>
        await sql.LoadDataAsync<ResultsModel, DeleteModel>("[base].[spEmployeeLevelsDelete]", delete);

}
