using Library.API.Goals.Models.ContractPeriod;
using Library.Database.DAL;
using Library.Goals.DataAccess.Contracts;
using Library.Goals.Models;
using Library.Goals.Models.Contracts;

namespace Library.Goals.Services.Contracts;

/// <summary>
/// Interface for contract results service operations.
/// </summary>
public interface IContractResultsService
{
    /// <summary>
    /// Gets contract results by ContractsUUID with all nested structures.
    /// </summary>
    /// <param name="basic">Basic model containing company and user information</param>
    /// <param name="contractsUUID">The UUID of the contract to retrieve</param>
    /// <returns>Contract results response DTO with hierarchical data, or null if not found</returns>
    Task<ContractResultsResponseDto?> GetContractResults(BasicModel basic, string contractsUUID, string? selectedUsersUUID);

    Task<List<RatingPeriodsDto?>> GetRatingPeriods(BasicModel basic);
}

/// <summary>
/// Service for contract results operations.
/// </summary>
public class ContractResultsService : IContractResultsService
{
    private readonly ISqlDataAccess _sql;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractResultsService"/> class.
    /// </summary>
    /// <param name="sql">SQL data access instance</param>
    public ContractResultsService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// Gets contract results by ContractsUUID with all nested structures.
    /// </summary>
    /// <param name="basic">Basic model containing company and user information</param>
    /// <param name="selectedUsersUUID">The UUID of the User to retrieve</param>
    /// <param name="contractsUUID">The UUID of the contract to retrieve</param>
    /// <returns>Contract results response DTO with hierarchical data, or null if not found</returns>
    public async Task<ContractResultsResponseDto?> GetContractResults(BasicModel basic, string contractsUUID, string? selectedUsersUUID)
    {
        var data = await ContractsResultsDataAccess.GetContractResults(_sql, basic, contractsUUID, selectedUsersUUID);
        return data ?? default;
    }

    public async Task<List<RatingPeriodsDto?>> GetRatingPeriods(BasicModel basic)
    {
        var data = await ContractsResultsDataAccess.GetRatingPeriods(_sql, basic);
        return data ?? default;
    }
}

