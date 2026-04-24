using Library.Database.DAL;
using Library.Goals.DataAccess.Contracts;
using Library.Goals.Models;
using Library.Goals.Models.Contracts;

namespace Library.Goals.Services.Contracts;

#region Interface
public interface IContractsDataService
{
    /// <summary>
    /// Retrieves contract data based on a unique contract UUID and basic model information.
    /// </summary>
    /// <param name="contractsUUID">The unique identifier for the contract.</param>
    /// <param name="basic">The basic model containing metadata or user context.</param>
    /// <returns>A task that represents the asynchronous operation. 
    /// The task result contains the <see cref="ContractsModel"/> if found; otherwise, null.</returns>
    Task<ContractsModel?> GetContractData(string contractsUUID, BasicModel basic);
}
#endregion

#region ContractsDataService
public class ContractsDataService : IContractsDataService
{
    private readonly ISqlDataAccess _sql;

    /// <summary>
    /// Constructor that injects the SQL data access dependency.
    /// </summary>
    /// <param name="sql">An instance of <see cref="ISqlDataAccess"/> used for database operations.</param>
    public ContractsDataService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// Retrieves contract data by calling the data access layer method.
    /// </summary>
    /// <param name="contractsUUID">The unique identifier for the contract.</param>
    /// <param name="basic">The basic model containing metadata or user context.</param>
    /// <returns>A task that represents the asynchronous operation. 
    /// The task result contains the <see cref="ContractsModel"/> if found; otherwise, null.</returns>
    public async Task<ContractsModel?> GetContractData(string contractsUUID, BasicModel basic)
    {
        // Call DAL method 
        return await ContractsDataAccess.GetContract(_sql, basic, contractsUUID);
    }
}
#endregion