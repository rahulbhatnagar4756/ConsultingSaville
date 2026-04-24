using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Database.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services;

/// <summary>
/// Service interface for managing employee hierarchy data.
/// </summary>
public interface IEmployeeHierarchyService
{
    /// <summary>
    /// Deletes an employee hierarchy record.
    /// </summary>
    /// <param name="CompanyUUID">The UUID of the company.</param>
    /// <param name="UsersUUIDLoggedIn">The UUID of the logged-in user performing the operation.</param>
    /// <param name="EmployeeHierarchyUUID">The UUID of the employee hierarchy record to delete.</param>
    /// <returns>A <see cref="ResultsModel"/> indicating success or failure.</returns>
    Task<ResultsModel> DeleteEmployeeHierarchy(string CompanyUUID, string UsersUUIDLoggedIn, string EmployeeHierarchyUUID);

    /// <summary>
    /// Saves or updates an employee hierarchy record.
    /// </summary>
    /// <param name="employeeHierarchy">The employee hierarchy data to save.</param>
    /// <returns>A <see cref="ResultsModel"/> indicating success or failure.</returns>
    Task<ResultsModel> SetEmployeeHierarchy(EmployeeHierarchyBasicModel employeeHierarchy);
}

/// <summary>
/// Implementation of <see cref="IEmployeeHierarchyService"/> for managing employee hierarchy.
/// </summary>
public class EmployeeHierarchyService: IEmployeeHierarchyService
{
    private readonly ISqlDataAccess _sql;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeHierarchyService"/> class.
    /// </summary>
    /// <param name="sql">The SQL data access service.</param>
    /// <param name="usersService">The users service.</param>
    public EmployeeHierarchyService(ISqlDataAccess sql)
    {
        _sql = sql;
    }


    /// <summary>
    /// Saves or updates an employee hierarchy record asynchronously.
    /// Delegates to data access layer.
    /// </summary>
    /// <param name="employeeHierarchy">The employee hierarchy model to save.</param>
    /// <returns>A <see cref="ResultsModel"/> with the operation result.</returns>
    public async Task<ResultsModel> SetEmployeeHierarchy(EmployeeHierarchyBasicModel employeeHierarchy) =>
         await DataAccess.Employees.EmployeeHierarchyDataAccess.SaveEmployeeHierarchy(_sql, employeeHierarchy);

    /// <summary>
    /// Deletes an employee hierarchy record asynchronously.
    /// Delegates to data access layer.
    /// </summary>
    /// <param name="CompanyUUID">The company UUID.</param>
    /// <param name="UsersUUIDLoggedIn">The UUID of the logged-in user.</param>
    /// <param name="EmployeeHierarchyUUID">The UUID of the employee hierarchy record.</param>
    /// <returns>A <see cref="ResultsModel"/> with the operation result.</returns>
    public async Task<ResultsModel> DeleteEmployeeHierarchy(string CompanyUUID, string UsersUUIDLoggedIn, string EmployeeHierarchyUUID) =>
        await DataAccess.Employees.EmployeeHierarchyDataAccess.DeleteEmployeeHierarchy(_sql, CompanyUUID, UsersUUIDLoggedIn, EmployeeHierarchyUUID);
}


