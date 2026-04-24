using Library.Database.DAL;
using Library.Goals.DataAccess.RatingPeriod;
using Library.Goals.Models;
using Library.Goals.Models.RatingPeriod;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services;

public interface IRatingPeriodsService
{
    // Administrative APIs
    Task<List<RatingPeriodDto>?> GetRatingPeriodTypesAsync(BasicModel basic);
    Task<ResultsModel?> CreateRatingPeriodTypeAsync(BasicModel basic, RatingPeriodDto ratingPeriod);
    Task<ResultsModel?> DeleteRatingPeriodTypeAsync(BasicModel basic, string ratingPeriodUUID);

    // Period Management APIs
    Task<List<RatingPeriodDateDto>?> GetRatingPeriodDatesAsync(BasicModel basic, string? ratingPeriodTypeUUID = null);
    Task<ResultsModel?> CreateRatingPeriodDateAsync(BasicModel basic, RatingPeriodDateDto ratingPeriodDate);
    Task<ResultsModel?> DeleteRatingPeriodDateAsync(BasicModel basic, string ratingPeriodDateUUID);
    Task<ResultsModel?> UpdateRatingPeriodDateIsActiveAsync(BasicModel basic, string ratingPeriodDateUUID, bool isActive);



    // KPI Rating Access Control API
    Task<RatingAccessCheckDto> CheckRatingAccessAsync(BasicModel basic, string ratingPeriodDateUUID);
    Task<List<RatingPeriodModel>> GetRatingPeriods(BasicModel basic);
}

/// <summary>
/// Service for Rating Periods - Focused on Documentation Requirements
/// </summary>
public class RatingPeriodsService : IRatingPeriodsService
{
    private readonly ISqlDataAccess _sql;

    public RatingPeriodsService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<List<RatingPeriodModel>> GetRatingPeriods(BasicModel basic)
    {
        var Data = await Library.Goals.DataAccess.RatingPeriod.RatingPeriodsDataAccess.GetRatingPeriods(_sql, basic);
        return Data?.ToList() ?? default;
    }

    /// <summary>
    /// Get all rating period types (Month, Quarter, Year) for setup/configuration
    /// </summary>
    public async Task<List<RatingPeriodDto>?> GetRatingPeriodTypesAsync(BasicModel basic)
    {
        var data = await RatingPeriodsDataAccess.GetAll(_sql, basic);
        return data?.ToList();
    }

    /// <summary>
    /// Create new rating period type (e.g., define "Quarter" with display name "Qtr")
    /// Administrative function for initial setup
    /// </summary>
    public async Task<ResultsModel?> CreateRatingPeriodTypeAsync(BasicModel basic, RatingPeriodDto ratingPeriod)
    {
        if (ratingPeriod == null || string.IsNullOrWhiteSpace(ratingPeriod.Name) || string.IsNullOrWhiteSpace(ratingPeriod.DisplayName))
            return new ResultsModel { UUID = null, isValid = false, Message = "Rating period type name and display name are required." };

        var saveModel = ratingPeriod.RatingPeriodSaveModel(basic);
        var result = await RatingPeriodsDataAccess.Save(_sql, saveModel);
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Delete rating period type (soft delete)
    /// </summary>
    public async Task<ResultsModel?> DeleteRatingPeriodTypeAsync(BasicModel basic, string ratingPeriodUUID)
    {
        if (string.IsNullOrWhiteSpace(ratingPeriodUUID))
            return new ResultsModel { UUID = null, isValid = false, Message = "Rating period UUID is required for deletion." };
        var result = await RatingPeriodsDataAccess.Delete(_sql, basic, ratingPeriodUUID);
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Get all rating period dates, optionally filtered by period type
    /// Used for administrative management of specific periods (Q1 2025, Jan 2025, etc.)
    /// </summary>
    public async Task<List<RatingPeriodDateDto>?> GetRatingPeriodDatesAsync(BasicModel basic, string? ratingPeriodTypeUUID = null)
    {
        var data = await RatingPeriodDatesDataAccess.GetAll(_sql, basic, ratingPeriodTypeUUID);
        return data?.ToList();
    }

    /// <summary>
    /// Create/Update specific rating period date (e.g., "Q1 2025" with specific date ranges)
    /// Implements business rule: Auto-toggle isActive based on DateClose vs current date
    /// </summary>
    public async Task<ResultsModel?> CreateRatingPeriodDateAsync(BasicModel basic, RatingPeriodDateDto ratingPeriodDate)
    {
        if (ratingPeriodDate == null || string.IsNullOrWhiteSpace(ratingPeriodDate.Name) ||
            string.IsNullOrWhiteSpace(ratingPeriodDate.RatingPeriodsUUID))
            return new ResultsModel { UUID = null, isValid = false, Message = "Rating period date requires name and parent rating period." };

        // Business Rule: Validate date logic
        if (ratingPeriodDate.DateEnd.HasValue && ratingPeriodDate.DateStart >= ratingPeriodDate.DateEnd)
            return new ResultsModel { UUID = null, isValid = false, Message = "Start date must be before end date." };

        if (ratingPeriodDate.DateClose.HasValue && ratingPeriodDate.DateOpen >= ratingPeriodDate.DateClose)
            return new ResultsModel { UUID = null, isValid = false, Message = "Open date must be before close date." };

        // Business Rule: Auto-toggle isActive based on DateClose
        if (ratingPeriodDate.DateClose.HasValue)
        {
            ratingPeriodDate.isActive = ratingPeriodDate.DateClose.Value > DateTime.Now;
        }

        var saveModel = ratingPeriodDate.RatingPeriodDateSaveModel(basic);
        var result = await RatingPeriodDatesDataAccess.Save(_sql, saveModel);
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Check if KPI rating is currently accessible - Core business function
    /// Used by KPI rating system to validate if employees/managers can rate
    /// Implements: Current date between DateOpen and DateClose + isActive = 1
    /// </summary>
    public async Task<RatingAccessCheckDto> CheckRatingAccessAsync(BasicModel basic, string ratingPeriodDateUUID)
    {
        if (string.IsNullOrWhiteSpace(ratingPeriodDateUUID))
            return new RatingAccessCheckDto
            {
                IsAccessible = false,
                Message = "Invalid rating period."
            };

        var data = await RatingPeriodDatesDataAccess.GetById(_sql, basic, ratingPeriodDateUUID);
        var ratingPeriodDate = data?.FirstOrDefault();

        if (ratingPeriodDate == null)
            return new RatingAccessCheckDto
            {
                IsAccessible = false,
                Message = "Rating period not found."
            };

        var currentDate = DateTime.Now;
        var isWithinDateRange = currentDate >= ratingPeriodDate.DateOpen &&
                              (!ratingPeriodDate.DateClose.HasValue || currentDate <= ratingPeriodDate.DateClose.Value);
        var isAccessible = ratingPeriodDate.isActive && isWithinDateRange;

        string message = "";
        if (!ratingPeriodDate.isActive)
            message = "Rating period is currently closed.";
        else if (currentDate < ratingPeriodDate.DateOpen)
            message = $"Rating opens on {ratingPeriodDate.DateOpen:yyyy-MM-dd}.";
        else if (ratingPeriodDate.DateClose.HasValue && currentDate > ratingPeriodDate.DateClose.Value)
            message = $"Rating closed on {ratingPeriodDate.DateClose.Value:yyyy-MM-dd}.";
        else if (isAccessible)
            message = "Rating is currently open.";

        return new RatingAccessCheckDto
        {
            RatingPeriodDateUUID = ratingPeriodDateUUID,
            IsAccessible = isAccessible,
            Message = message,
            DateOpen = ratingPeriodDate.DateOpen,
            DateClose = ratingPeriodDate.DateClose,
            IsActive = ratingPeriodDate.isActive
        };
    } 

    /// <summary>
    /// Delete rating period date (soft delete)
    /// </summary>
    public async Task<ResultsModel?> DeleteRatingPeriodDateAsync(BasicModel basic, string ratingPeriodDateUUID)
    {
        if (string.IsNullOrWhiteSpace(ratingPeriodDateUUID))
            return new ResultsModel { UUID = null, isValid = false, Message = "Rating period date UUID is required for deletion." };
        var result = await RatingPeriodDatesDataAccess.DeletePeriod(_sql, basic, ratingPeriodDateUUID);
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Activate or deactivate a rating period date by toggling isActive
    /// </summary>
    public async Task<ResultsModel?> UpdateRatingPeriodDateIsActiveAsync(BasicModel basic, string ratingPeriodDateUUID, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(ratingPeriodDateUUID))
            return new ResultsModel
            {
                UUID = null,
                isValid = false,
                Message = "Rating period date UUID is required for updating isActive."
            };

        var result = await RatingPeriodDatesDataAccess.UpdateIsActive(_sql, basic, ratingPeriodDateUUID, isActive);
        return result?.FirstOrDefault();
    }

}


