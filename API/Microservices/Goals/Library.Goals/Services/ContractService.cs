using Library.API.Goals.Models.PerformanceReview;
using Library.Database.DAL;
using Library.Goals.DataAccess;
using Library.Goals.Models;
using Library.Goals.Models.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services;

public interface IContractService
{
    Task<List<ResultsModel>?> CloneContract(string CompanyUUID, string UsersUUID, ContractsCloneModel contractsClone);
    Task<ContractsModel?> GetContract(BasicModel basic, string contractsUUID);
    Task<ContractsModel?> GetContractByUserContractPeriod(BasicModel basic, ContractUsersPeriodsModel? cup);
    Task<ResultsModel?> GetOrCreateLatestUserContract(BasicModel basic, string usersUUID);
    Task<PerformanceReviewRequest?> GetEmployeesBYUserUUID(BasicModel basic, string usersUUID);
}

public class ContractService : IContractService
{
    private ISqlDataAccess _sql;

    public ContractService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// Get contract information by ContractsUUID
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="basic"></param>
    /// <param name="contractsUUID"></param>
    /// <returns>List of ContractsModel from JSON stored procedure</returns>
    public async Task<ContractsModel?> GetContract(BasicModel basic, string contractsUUID)
    {
        var Data = await Library.Goals.DataAccess.Contracts.ContractsDataAccess.GetContract(_sql, basic, contractsUUID);
        return Data ?? default;
    }

    /// <summary>
    /// Clone the selected contract, and assign it to the selected users
    /// </summary>
    /// <param name="contractsClone"></param>
    /// <returns></returns>
    public async Task<List<ResultsModel>?> CloneContract(string CompanyUUID, string UsersUUID, ContractsCloneModel contractsClone)
    {
        var Data = await ContractsDataAccess.CloneContract(_sql, CompanyUUID, UsersUUID, contractsClone);
        return Data?.ToList() ?? default;
    }

    public async Task<ContractsModel?> GetContractByUserContractPeriod(BasicModel basic, ContractUsersPeriodsModel? cup)
    {
        var Data = await Library.Goals.DataAccess.Contracts.ContractsDataAccess.GetContractByUserContractPeriod(_sql, basic, cup);
        return Data ?? default;
    }

    /// <summary>
    /// Gets or creates the latest contract for the specified user
    /// </summary>
    /// <param name="basic">Basic model containing company and logged-in user info</param>
    /// <param name="usersUUID">UUID of the user to get/create contract for</param>
    /// <returns>Contracts UUID if successful, null otherwise</returns>
    public async Task<ResultsModel?> GetOrCreateLatestUserContract(BasicModel basic, string usersUUID)
    {
        var Data = await Library.Goals.DataAccess.Contracts.ContractsDataAccess.GetOrCreateLatestUserContract(_sql, basic, usersUUID);
        return Data;
    }

    public async Task<PerformanceReviewRequest?> GetEmployeesBYUserUUID(BasicModel basic, string? usersUUID)
    {
        var Data = await Library.Goals.DataAccess.Contracts.ContractsDataAccess.GetEmployeesBYUserUUID(_sql, basic, usersUUID);
        return Data;
    }
}
