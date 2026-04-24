using Library.Database.DAL;
using Library.Goals.DataAccess.ContractPeriods;
using Library.Goals.Models;
using Library.Goals.Models.ContractPeriods;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services;

public interface IContractPeriodsService
{
    Task<List<ContractPeriodDto>?> GetContractPeriodsAsync(BasicModel basic);
    Task<ResultsModel?> SaveContractPeriodAsync(BasicModel basic, ContractPeriodDto contractPeriod);
    Task<ResultsModel?> DeleteContractPeriodAsync(BasicModel basic, string contractPeriodUUID);
    Task<ContractPeriodDto?> GetContractPeriodByIdAsync(BasicModel basic, string contractPeriodUUID);
}

/// <summary>
/// Service class for handling contract period operations such as retrieving, saving, and deleting.
/// </summary>
public class ContractPeriodsService : IContractPeriodsService
{
    // Dependency injection of a SQL data access layer.
    private readonly ISqlDataAccess _sql;

    /// <summary>
    /// Constructor to initialize SQL data access dependency.
    /// </summary>
    /// <param name="sql">SQL data access implementation</param>
    public ContractPeriodsService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// Retrieves one or more contract periods from the database 
    /// based on the provided BasicModel and optional contractPeriodUUID.
    /// </summary>
    /// <param name="basic">Basic model containing tenant and user info</param>
    /// <returns>List of contract periods or null</returns>
    public async Task<List<ContractPeriodDto>?> GetContractPeriodsAsync(BasicModel basic)
    {
        // Calls the data access layer to retrieve contract period records.
        var data = await ContractPeriodsDataAccess.GetAll(_sql, basic);
        // Returns the result as a list (or null if no data).
        return data?.ToList();
    }

    /// <summary>
    /// Saves a new or updated contract period to the database.
    /// Validates the input before saving. Returns result information including success status.
    /// </summary>
    /// <param name="basic">Basic model containing tenant and user info</param>
    /// <param name="contractPeriod">Contract period DTO to save</param>
    /// <returns>Result of the save operation</returns>
    public async Task<ResultsModel?> SaveContractPeriodAsync(BasicModel basic, ContractPeriodDto contractPeriod)
    {
        // Validates that the contract period object is not null and has a valid name.
        if (contractPeriod == null || string.IsNullOrWhiteSpace(contractPeriod.Name))
            return new ResultsModel { UUID = null, isValid = false, Message = "Invalid contract period data." };
        // Converts the DTO into a save model suitable for the database layer.
        var saveModel = contractPeriod.ContractPeriodSaveModel(basic);
        // Calls the data access layer to save the contract period.
        var result = await ContractPeriodsDataAccess.Save(_sql, saveModel);
        // Returns the first result (if any) from the save operation.
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Deletes a contract period using its UUID.
    /// Validates the UUID before attempting deletion. Returns result information including success status.
    /// </summary>
    /// <param name="basic">Basic model containing tenant and user info</param>
    /// <param name="contractPeriodUUID">UUID of the contract period to delete</param>
    /// <returns>Result of the delete operation</returns>
    public async Task<ResultsModel?> DeleteContractPeriodAsync(BasicModel basic, string contractPeriodUUID)
    {
        // Validates that the UUID is not null or empty.
        if (string.IsNullOrEmpty(contractPeriodUUID))
            return new ResultsModel { UUID = null, isValid = false, Message = "Invalid contract period selected." };
        // Calls the data access layer to delete the contract period.
        var result = await ContractPeriodsDataAccess.Delete(_sql, basic, contractPeriodUUID);
        // Returns the first result (if any) from the delete operation.
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Retrieves a single contract period from the database by its UUID.
    /// </summary>
    /// <param name="basic">Basic model containing tenant and user info</param>
    /// <param name="contractPeriodUUID">UUID of the contract period to retrieve</param>
    /// <returns>Contract period DTO or null if not found</returns>
    public async Task<ContractPeriodDto?> GetContractPeriodByIdAsync(BasicModel basic, string contractPeriodUUID)
    {
        // Validate that the UUID is not null or empty
        if (string.IsNullOrWhiteSpace(contractPeriodUUID))
            return null;

        // Calls the data access layer to retrieve a specific contract period by UUID
        var data = await ContractPeriodsDataAccess.GetById(_sql, basic, contractPeriodUUID);
        // Returns the first result (should be only one) or null if not found
        return data?.FirstOrDefault();
    }
}


