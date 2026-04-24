using Library.Database.DAL;
using Library.Goals.DataAccess.ScoringPeriod;
using Library.Goals.Models;
using Library.Goals.Models.ScoringPeriod;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services;

public interface IScoringPeriodsService
{
    // Administrative APIs
    Task<List<ScoringPeriodDto>?> GetScoringPeriodsAsync(BasicModel basic);
    Task<ResultsModel?> CreateScoringPeriodAsync(BasicModel basic, ScoringPeriodDto scoringPeriod);
    Task<ResultsModel?> DeleteScoringPeriodAsync(BasicModel basic, string scoringPeriodUUID);
}

/// <summary>
/// Service for Scoring Periods (managing recurring scoring windows like Annual, Mid-Year)
/// </summary>
public class ScoringPeriodsService : IScoringPeriodsService
{
    private readonly ISqlDataAccess _sql;

    public ScoringPeriodsService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// Get all scoring periods for administrative configuration and dropdowns
    /// </summary>
    public async Task<List<ScoringPeriodDto>?> GetScoringPeriodsAsync(BasicModel basic)
    {
        var data = await ScoringPeriodsDataAccess.GetAll(_sql, basic);
        return data?.ToList();
    }

    /// <summary>
    /// Create or update a scoring period (Start/End dates, Name, Description)
    /// </summary>
    public async Task<ResultsModel?> CreateScoringPeriodAsync(BasicModel basic, ScoringPeriodDto scoringPeriod)
    {
        if (scoringPeriod == null || string.IsNullOrWhiteSpace(scoringPeriod.Name))
            return new ResultsModel { UUID = null, isValid = false, Message = "Scoring period name is required." };

        var saveModel = scoringPeriod.ToSaveModel(basic);
        var result = await ScoringPeriodsDataAccess.Save(_sql, saveModel);
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Soft-delete a scoring period by UUID
    /// </summary>
    public async Task<ResultsModel?> DeleteScoringPeriodAsync(BasicModel basic, string scoringPeriodUUID)
    {
        if (string.IsNullOrWhiteSpace(scoringPeriodUUID))
            return new ResultsModel { UUID = null, isValid = false, Message = "Scoring period UUID is required for deletion." };

        var result = await ScoringPeriodsDataAccess.Delete(_sql, basic, scoringPeriodUUID);
        return result?.FirstOrDefault();
    }
}
