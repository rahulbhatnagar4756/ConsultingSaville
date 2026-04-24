using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.RatingPeriod;

namespace Library.Goals.DataAccess.RatingPeriod
{
    /// <summary>
    /// Data access for specific Rating Period Date instances
    /// </summary>
    public static class RatingPeriodDatesDataAccess
    {
        /// <summary>
        /// Get all rating period dates, optionally filtered by rating period type
        /// </summary>
        public static async Task<IEnumerable<RatingPeriodDateDto>> GetAll(ISqlDataAccess sql, BasicModel basic, string? ratingPeriodsUUID = null)
        {
            return await sql.LoadDataAsync<RatingPeriodDateDto, dynamic>(
                "[Goals].[spRatingPeriodDates_Get]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    RatingPeriodDatesUUID = (string?)null,
                    RatingPeriodsUUID = ratingPeriodsUUID
                }
            );
        }

        /// <summary>
        /// Save rating period date instance (create/update specific periods)
        /// </summary>
        public static async Task<IEnumerable<ResultsModel>> Save(ISqlDataAccess sql, RatingPeriodDateSaveModel saveModel)
        {
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spRatingPeriodDates_Save]",
                saveModel
            );
        }

        /// <summary>
        /// Get specific rating period date for KPI access validation
        /// </summary>
        public static async Task<IEnumerable<RatingPeriodDateDto>> GetById(ISqlDataAccess sql, BasicModel basic, string ratingPeriodDateUUID)
        {
            return await sql.LoadDataAsync<RatingPeriodDateDto, dynamic>(
                "[Goals].[spRatingPeriodDates_Get]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    RatingPeriodDatesUUID = ratingPeriodDateUUID,
                    RatingPeriodsUUID = (string?)null
                }
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

        /// <summary>
        /// Delete rating period date (soft delete)
        /// </summary>
        public static async Task<IEnumerable<ResultsModel>> DeletePeriod(ISqlDataAccess sql, BasicModel basic, string ratingPeriodDatesUUID)
        {
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spRatingPeriodDates_Delete]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    RatingPeriodDatesUUID = ratingPeriodDatesUUID
                }
            );
        }

        /// <summary>
        /// Update isActive flag for Rating Period Date (activate or deactivate)
        /// </summary>
        public static async Task<IEnumerable<ResultsModel>> UpdateIsActive(
            ISqlDataAccess sql,
            BasicModel basic,
            string ratingPeriodDatesUUID,
            bool isActive)
        {
            return await sql.LoadDataAsync<ResultsModel, dynamic>(
                "[Goals].[spRatingPeriodDates_UpdateIsActive]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    RatingPeriodDatesUUID = ratingPeriodDatesUUID,
                    IsActive = isActive
                }
            );
        }


    }
}
