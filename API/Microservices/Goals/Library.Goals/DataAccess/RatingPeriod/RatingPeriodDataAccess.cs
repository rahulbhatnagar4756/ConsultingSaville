using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.RatingPeriod;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.RatingPeriod
{
    /// <summary>
    /// Data access for Rating Period Types (Month, Quarter, Year definitions)
    /// </summary>
    public static class RatingPeriodsDataAccess
    {

        public static async Task<IEnumerable<RatingPeriodModel>> GetRatingPeriods(ISqlDataAccess sql, BasicModel basic) =>
            await sql.LoadDataAsync<RatingPeriodModel, dynamic>("[Goals].[spRatingPeriods]", basic);


        /// <summary>
        /// Get all rating period types for dropdown/selection purposes
        /// </summary>
        public static async Task<IEnumerable<RatingPeriodDto>> GetAll(ISqlDataAccess sql, BasicModel basic)
        {
            return await sql.LoadDataAsync<RatingPeriodDto, dynamic>(
                "[Goals].[spRatingPeriods_Get]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn
                }
            );
        }

        /// <summary>
        /// Save rating period type (for administrative setup)
        /// </summary>
        public static async Task<IEnumerable<ResultsModel>> Save(ISqlDataAccess sql, RatingPeriodSaveModel saveModel)
        {
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spRatingPeriods_Save]",
                saveModel
            );
        }

        /// <summary>
        /// Delete rating period type (soft delete)
        /// </summary>
        public static async Task<IEnumerable<ResultsModel>> Delete(ISqlDataAccess sql, BasicModel basic, string ratingPeriodsUUID)
        {
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spRatingPeriods_Delete]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    RatingPeriodsUUID = ratingPeriodsUUID
                }
            );
        }
    }
}
