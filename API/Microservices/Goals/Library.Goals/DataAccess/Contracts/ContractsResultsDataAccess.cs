using Library.API.Goals.Models.ContractPeriod;
using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.ContractPeriods;
using Library.Goals.Models.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Contracts
{
    /// <summary>
    /// Static class that handles data access operations for contract results.
    /// Communicates with stored procedures in the "Goals" schema.
    /// </summary>
    public static class ContractsResultsDataAccess
    {

        /// <summary>
        /// Retrieves detailed contract results, including all nested structures, from the database.
        /// </summary>
        /// <param name="sql">
        /// The SQL data access instance used to query the database.
        /// </param>
        /// <param name="basic">
        /// Basic contextual information, such as the company identifier and the logged-in user UUID.
        /// </param>
        /// <param name="contractsUUID">
        /// The unique identifier (UUID) of the contract to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="ContractResultsResponseDto"/> containing the hierarchical contract data,
        /// or <c>null</c> if no matching contract is found.
        /// </returns>
        public static async Task<ContractResultsResponseDto?> GetContractResults(ISqlDataAccess sql, BasicModel basic, string contractsUUID, string? selectedUsersUUID)
        {
            // Fetch contract result JSON from the SP
            var result = await sql.LoadDataAsync<JsonResultDto, dynamic>(
                "[Goals].[spContracts_Results]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = !string.IsNullOrEmpty(selectedUsersUUID) ? selectedUsersUUID : basic.UsersUUIDLoggedIn,
                    ContractsUUID = contractsUUID
                }
            );

            var jsonResult = result.FirstOrDefault();

            if (jsonResult == null || string.IsNullOrEmpty(jsonResult.JsonResult))
                return null;

            // Deserialize the JSON string into DTO
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var contractResults = JsonSerializer.Deserialize<ContractResultsResponseDto>(
                jsonResult.JsonResult,
                options
            );

            if (contractResults?.Data != null && contractResults.Data.Any())
            {
                // Fetch employee details for all contracts in parallel
                var employeeTasks = contractResults.Data
                    .Where(c => !string.IsNullOrEmpty(c.UsersUUID))
                    .Select(async contract =>
                    {
                        var employeeResult = await sql.LoadDataAsync<EmployeeDetailsDto, dynamic>(
                            "[Goals].[spEmployees_GetDetails]",
                            new { UsersUUID = contract.UsersUUID }
                        );

                        contract.EmployeeDetails = employeeResult.FirstOrDefault();
                    }).ToArray();

                await Task.WhenAll(employeeTasks);
            }

            // Return full result
            return contractResults;
        }


        public static async Task<List<RatingPeriodsDto>?> GetRatingPeriods(ISqlDataAccess sql, BasicModel basic)
        {
            var result = await sql.LoadDataAsync<RatingPeriodsDto, dynamic>(
                "[Goals].[sp_GetRatingPeriodsByCompany]",
                new { CompanyUUID = basic.CompanyUUID }
            );

            return result.ToList();
        }

        
        /// <summary>
        /// Helper DTO for capturing the JSON result from the stored procedure.
        /// </summary>
        internal class JsonResultDto
        {
            /// <summary>
            /// Gets or sets the JSON result string returned by the stored procedure.
            /// </summary>
            public string? JsonResult { get; set; }
        }
    }


}
