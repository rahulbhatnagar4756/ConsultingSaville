using Dapper;

using Library.API.Goals.Models.PerformanceReview;
using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Contracts
{
    public static class ContractsDataAccess
    {
        /// <summary>
        /// Get contract information by ContractsUUID
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="basic"></param>
        /// <param name="contractsUUID"></param>
        /// <returns>List of ContractsModel from JSON stored procedure</returns>
        public static async Task<ContractsModel?> GetContract(ISqlDataAccess sql, BasicModel basic, string contractsUUID)
        {
            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                ContractsUUID = contractsUUID
            };

            var result = await sql.LoadDataJsonAsync<ContractsModel, dynamic>("[Goals].[spContracts]", parameters);
            return result?.FirstOrDefault() ?? default;
        }

        public static async Task<ContractsModel?> GetContractByUserContractPeriod(ISqlDataAccess sql, BasicModel basic, ContractUsersPeriodsModel? cup)
        {
            if (cup == null || string.IsNullOrEmpty(cup.UsersUUID) || string.IsNullOrEmpty(cup.ContractPeriodsUUID))
                return null;

            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UsersUUID = cup.UsersUUID,
                ContractPeriodsUUID = cup.ContractPeriodsUUID
            };

            var result = await sql.LoadDataJsonAsync<ContractsModel, dynamic>("[Goals].[spContracts_User_ContractPeriod]", parameters);
            return result?.FirstOrDefault() ?? default;
        }

        /// <summary>
        /// Gets or creates the latest contract for a user
        /// </summary>
        /// <param name="sql">SQL Data Access interface</param>
        /// <param name="basic">Basic model containing company and logged-in user info</param>
        /// <param name="usersUUID">UUID of the user to get/create contract for</param>
        /// <returns>Contracts UUID if successful, null otherwise</returns>
        public static async Task<ResultsModel?> GetOrCreateLatestUserContract(ISqlDataAccess sql, BasicModel basic, string usersUUID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyUUID", basic.CompanyUUID);
            parameters.Add("@UsersUUIDLoggedIn", basic.UsersUUIDLoggedIn);
            parameters.Add("@UsersUUID", usersUUID);
            parameters.Add("@ContractsUUID", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 200);
            parameters.Add("@isSuccessful", dbType: System.Data.DbType.Boolean, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@Message", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: -1);

            var result = await sql.ExecuteWithOutputsAsync("[Goals].[spContracts_Users_Latest]", parameters);

            var isSuccessful = parameters.Get<bool?>("@isSuccessful");
            var contractsUUID = parameters.Get<string>("@ContractsUUID");
            var message = parameters.Get<string>("@Message");

            return new ResultsModel { isValid = isSuccessful, UUID = contractsUUID, Message = message };
        }


        public static async Task<PerformanceReviewRequest?> GetEmployeesBYUserUUID(ISqlDataAccess sql, BasicModel basic, string usersUUID)
        {
           
            var parameters = new
            {
                UsersUUID = usersUUID
            };
            var result = await sql.LoadDataJsonAsync<PerformanceReviewRequest, dynamic>("[Goals].[spGetUsersByUUID_Results]", parameters);
            return result?.FirstOrDefault();
        }
    }
}
