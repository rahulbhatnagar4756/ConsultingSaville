using Library.Database.DAL;
using Library.Goals.DataAccess.Employees;
using Library.Goals.Models;
using Library.Goals.Models.EmployeeResult;
using Library.Goals.Models.Employees;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services;

/// <summary>
/// Interface for employee service operations.
/// Defines a contract for retrieving employee data.
/// </summary>
public interface IEmployeesService
{
    /// <summary>
    /// Asynchronously retrieves a list of employees for the given context.
    /// </summary>
    /// <param name="basic">Basic model containing company and user identifiers.</param>
    /// <param name="jsonFilter">Optional JSON string for search and filtering criteria.</param>
    /// <returns>A list of employees or null if none are found.</returns>
    Task<List<EmployeeDto>?> GetEmployeesAsync(BasicModel basic, string? jsonFilter = null);

    /// <summary>
    /// Checks whether the specified user is a manager or not.
    /// </summary>
    /// <param name="usersUUID">The UUID of the user to check.</param>
    /// <param name="basic">Basic model containing tenant/company context.</param>
    /// <returns>True if the user is a manager; otherwise false.</returns>
    Task<bool> IsUserManagerAsync(Guid usersUUID, BasicModel basic);

    /// <summary>
    /// Handles business logic for saving (insert/update) a Result.
    /// Delegates DB operations to ResultsDataAccess.Save().
    /// </summary>
    /// <param name="req">Incoming Result data for saving.</param>
    /// <param name="basic">Basic tenant/user context extracted from JWT.</param>
    /// <returns>
    /// A single SaveResultResponse representing the DB result.
    /// If no response is returned, a default fail response is generated.
    /// </returns>
    Task<SaveResultResponse> SaveResult(SaveResultRequest req, BasicModel basic);
}

/// <summary>
/// Concrete implementation of the employee service.
/// Handles business logic and delegates data access operations related to employees.
/// </summary>
public class EmployeesService : IEmployeesService
{
    // Field to hold the injected SQL data access dependency.
    private readonly ISqlDataAccess _sql;

    /// <summary>
    /// Constructor for EmployeesService.
    /// Initializes the service with a SQL data access layer implementation.
    /// </summary>
    /// <param name="sql">The SQL data access object (implements ISqlDataAccess).</param>
    public EmployeesService(ISqlDataAccess sql)
    {
        _sql = sql;  // Assigning the injected SQL data access instance to the local field.
    }

    /// <summary>
    /// Retrieves a list of employees based on the given BasicModel and optional filter.
    /// </summary>
    /// <param name="basic">Basic model containing tenant and user info.</param>
    /// <param name="jsonFilter">Optional JSON filter to apply search and filters.</param>
    /// <returns>List of employees or null if none found.</returns>
    public async Task<List<EmployeeDto>?> GetEmployeesAsync(BasicModel basic, string? jsonFilter = null)
    {
        // Call the data access method to retrieve employee data using the stored procedure.
        var data = await EmployeesDataAccess.GetAll(_sql, basic, jsonFilter);
        // Convert the result to a List. If data is null, return null.
        return data?.ToList();
    }

    /// <summary>
    /// Checks whether a given user is a manager based on the stored procedure [Goals].[sp_IsUserManager].
    /// </summary>
    /// <param name="usersUUID">The UUID of the user to check.</param>
    /// <param name="basic">Basic model containing company context.</param>
    /// <returns>True if user is manager, false otherwise.</returns>
    public async Task<bool> IsUserManagerAsync(Guid usersUUID, BasicModel basic)
    {
        // Delegate the data access logic to the EmployeesDataAccess class
        return await EmployeesDataAccess.CheckIsManagerAsync(_sql, usersUUID, basic);
    }

    /// <summary>
    /// Handles business logic for saving (insert/update) a Result.
    /// Delegates DB operations to ResultsDataAccess.Save().
    /// </summary>
    /// <param name="req">Incoming Result data for saving.</param>
    /// <param name="basic">Basic tenant/user context extracted from JWT.</param>
    /// <returns>
    /// A single SaveResultResponse representing the DB result.
    /// If no response is returned, a default fail response is generated.
    /// </returns>
    public async Task<SaveResultResponse> SaveResult(SaveResultRequest req, BasicModel basic)
    {
        var result = await EmployeesDataAccess.Save(_sql, req, basic);

        return result?.FirstOrDefault() ?? new SaveResultResponse
        {
            Id = 0,
            isValid = false,
            Message = "No data returned from database."
        };
    }


}
