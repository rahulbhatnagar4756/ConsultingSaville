using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.ScoringPeriod;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.ScoringPeriod
{
    /// <summary>
    /// Data access for Scoring Periods (Recurring performance scoring windows)
    /// </summary>
    public static class ScoringPeriodsDataAccess
    {
        /// <summary>
        /// Get all scoring periods for dropdown/selection or listing
        /// </summary>
        public static async Task<IEnumerable<ScoringPeriodDto>> GetAll(ISqlDataAccess sql, BasicModel basic)
        {
            return await sql.LoadDataAsync<ScoringPeriodDto, dynamic>(
                "[Goals].[spScoringPeriods_Get]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn
                }
            );
        }

        /// <summary>
        /// Save or update scoring period (for administrative setup)
        /// </summary>
        public static async Task<IEnumerable<ResultsModel>> Save(ISqlDataAccess sql, ScoringPeriodSaveModel saveModel)
        {
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spScoringPeriods_Save]",
                saveModel
            );
        }

        /// <summary>
        /// Soft delete a scoring period (deactivate + mark deleted)
        /// </summary>
        public static async Task<IEnumerable<ResultsModel>> Delete(ISqlDataAccess sql, BasicModel basic, string scoringPeriodsUUID)
        {
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spScoringPeriods_Delete]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    ScoringPeriodsUUID = scoringPeriodsUUID
                }
            );
        }
    }

}
