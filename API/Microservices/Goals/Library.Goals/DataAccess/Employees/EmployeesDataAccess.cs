
using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.EmployeeResult;
using Library.Goals.Models.Employees;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Employees
{
    /// <summary>
    /// Static class to handle data access for employees using stored procedures.
    /// </summary>
    public static class EmployeesDataAccess
    {
        /// <summary>
        /// Retrieves all employees using the stored procedure [Goals].[spContacts_Employees].
        /// </summary>
        /// <param name="sql">SQL data access instance</param>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="jsonFilter">Optional JSON filter string for search and filters</param>
        /// <returns>A collection of EmployeeDto</returns>
        public static async Task<IEnumerable<EmployeeDto>> GetAll(ISqlDataAccess sql, BasicModel basic, string? jsonFilter)
        {
            return await sql.LoadDataAsync<EmployeeDto, dynamic>(
                "[Goals].[spContacts_Employees]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    json = jsonFilter ?? "{}" // Use empty filter if none is provided
                }
            );
        }

        /// <summary>
        /// Checks if a specific user is a manager using the stored procedure [Goals].[sp_IsUserManager].
        /// </summary>
        /// <param name="sql">SQL data access instance</param>
        /// <param name="usersUUID">UUID of the user to check</param>
        /// <param name="basic">Basic model containing company context</param>
        /// <returns>True if the user is a manager, otherwise false</returns>
        public static async Task<bool> CheckIsManagerAsync(ISqlDataAccess sql, Guid usersUUID, BasicModel basic)
        {
            // Execute the stored procedure and retrieve a single value (1 or 0)
            var result = await sql.LoadDataAsync<int, dynamic>(
                "[Goals].[sp_IsUserManager]",
                new
                {
                    UsersUUID = usersUUID
                }
            );

            // If the stored procedure returns 1, the user is a manager
            return result.FirstOrDefault() == 1;
        }

        /// <summary>
        /// Executes the stored procedure [Goals].[spResults_Save] to create/update a Result.
        /// </summary>
        /// <param name="sql">The injected SQL DataAccess implementation.</param>
        /// <param name="req">Model containing property values for insert/update.</param>
        /// <param name="basic">Contains CompanyUUID and UsersUUIDLoggedIn for access validation.</param>
        /// <returns>
        /// A collection of SaveResultResponse objects returned by the stored procedure.
        /// Usually contains exactly one row representing the saved result.
        /// </returns>
        public static async Task<IEnumerable<SaveResultResponse>?> Save(ISqlDataAccess sql, SaveResultRequest req, BasicModel basic)
        {
            var result = await sql.LoadDataAsync<SaveResultResponse, dynamic>(
                "[Goals].[spResults_UPSERT]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    Id = req.Id,
                    KPI_UUID = req.KPI_UUID,
                    RatingPeriod_UUID = req.RatingPeriod_UUID,
                    ResultValue = req.ResultValue,
                    isActive = req.isActive
                }
            );

            return result;
        }
       
    }
}
