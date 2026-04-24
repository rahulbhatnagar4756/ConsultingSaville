using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.ContractPeriods;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.ContractPeriods
{
    /// <summary>
    /// Static class that handles data access operations for contract periods.
    /// Communicates with stored procedures in the "Goals" schema.
    /// </summary>
    public static class ContractPeriodsDataAccess
    {
        /// <summary>
        /// Get all contract periods from the database.
        /// If a UUID is provided, retrieves a specific contract period.
        /// </summary>
        /// <param name="sql">SQL data access instance</param>
        /// <param name="basic">Basic info like company and logged-in user UUIDs</param>
        /// <returns>A collection of contract period DTOs</returns>
        public static async Task<IEnumerable<ContractPeriodDto>> GetAll(ISqlDataAccess sql, BasicModel basic)
        {
            // Call the stored procedure to fetch contract periods using the provided parameters
            return await sql.LoadDataAsync<ContractPeriodDto, dynamic>(
                "[Goals].[spContractPeriods_Get]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn
                }
            );
        }

        /// <summary>
        /// Save a contract period to the database.
        /// Uses insert or update logic depending on whether a UUID is present in the model.
        /// </summary>
        /// <param name="sql">SQL data access instance</param>
        /// <param name="saveModel">The contract period data to save</param>
        /// <returns>A collection of result models indicating success/failure</returns>
        public static async Task<IEnumerable<ResultsModel>> Save(ISqlDataAccess sql, ContractPeriodSaveModel saveModel)
        {
            // Call the stored procedure to insert or update the contract period.
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spContractPeriods_Save]",
                saveModel
            );
        }

        /// <summary>
        /// Soft delete a contract period by UUID.
        /// Marks it as deleted rather than removing it permanently.
        /// </summary>
        /// <param name="sql">SQL data access instance</param>
        /// <param name="basic">Basic info like company and user UUIDs</param>
        /// <param name="contractPeriodUUID">UUID of the contract period to delete</param>
        /// <returns>A collection of result models indicating the outcome</returns>
        public static async Task<IEnumerable<ResultsModel>> Delete(ISqlDataAccess sql, BasicModel basic, string contractPeriodUUID)
        {
            // Call the stored procedure to soft delete the contract period.
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spContractPeriods_Delete]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    ContractPeriodsUUID = contractPeriodUUID
                }
            );
        }

        /// <summary>
        /// Get a specific contract period by its UUID from the database.
        /// </summary>
        /// <param name="sql">SQL data access instance</param>
        /// <param name="basic">Basic info like company and logged-in user UUIDs</param>
        /// <param name="contractPeriodUUID">UUID of the contract period to retrieve</param>
        /// <returns>A collection containing the contract period DTO (should be single item or empty)</returns>
        public static async Task<IEnumerable<ContractPeriodDto>> GetById(ISqlDataAccess sql, BasicModel basic, string contractPeriodUUID)
        {
            // Call the stored procedure to fetch a specific contract period by UUID
            return await sql.LoadDataAsync<ContractPeriodDto, dynamic>(
                "[Goals].[spContractPeriods_Get]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    ContractPeriodsUUID = contractPeriodUUID
                }
            );
        }
    }
}
