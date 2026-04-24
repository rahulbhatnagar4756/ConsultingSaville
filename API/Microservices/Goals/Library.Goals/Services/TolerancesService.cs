using Library.Database.DAL;
using Library.Goals.DataAccess.Tolerances;
using Library.Goals.Mappers.Tolerances;
using Library.Goals.Models;
using Library.Goals.Models.Tolerances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services
{
    public interface ITolerancesService
    {
        Task<ResultsModel?> DeleteToleranceSet(BasicModel basic, string uuid);
        Task<List<ToleranceSetsModel>?> GetToleranceSets(BasicModel basic);
        Task<ResultsModel?> SaveToleranceSet(BasicModel bases, ToleranceSetsModel tolerance);
        
        // ToleranceRanges methods
        Task<List<ToleranceRangesModel>?> GetToleranceRanges(BasicModel basic, string toleranceSetsUUID);
        Task<ResultsModel?> SaveToleranceRange(BasicModel basic, ToleranceRangesModel range);
        Task<ResultsModel?> DeleteToleranceRange(BasicModel basic, string uuid);
    }

    public class TolerancesService : ITolerancesService
    {
        private readonly ISqlDataAccess _sql;

        public TolerancesService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Get all tolerance sets for a company
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <returns>List of tolerance sets</returns>
        public async Task<List<ToleranceSetsModel>?> GetToleranceSets(BasicModel basic)
        {
            var data = await ToleranceSetsDataAccess.GetAll(_sql, basic);
            return data?.ToList();
        }

        /// <summary>
        /// Save tolerance set (create or update)
        /// </summary>
        /// <param name="saveModel">Tolerance set save model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveToleranceSet(BasicModel bases, ToleranceSetsModel tolerance)
        {
            if (tolerance == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid tolerance set data." };

            var SaveModel = tolerance.ToleranceSetsSaveModel(bases);

            var result = await ToleranceSetsDataAccess.Save(_sql, SaveModel);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Delete tolerance set (soft delete)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="uuid">UUID of the tolerance set to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteToleranceSet(BasicModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid tolerance set selected." };

            var result = await ToleranceSetsDataAccess.Delete(_sql, basic, uuid);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Get all tolerance ranges for a specific tolerance set
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="toleranceSetsUUID">UUID of the tolerance set</param>
        /// <returns>List of tolerance ranges</returns>
        public async Task<List<ToleranceRangesModel>?> GetToleranceRanges(BasicModel basic, string toleranceSetsUUID)
        {
            if (string.IsNullOrEmpty(toleranceSetsUUID))
                return null;

            var data = await ToleranceRangesDataAccess.GetAllByToleranceSet(_sql, basic, toleranceSetsUUID);
            return data?.ToList();
        }

        /// <summary>
        /// Save tolerance range (create or update)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="range">Tolerance range model</param>
        /// <param name="toleranceSetsUUID">UUID of the tolerance set</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveToleranceRange(BasicModel basic, ToleranceRangesModel range)
        {
            if (range == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid tolerance range data." };

            var saveModel = range.ToleranceRangesSaveModel(basic);
            var result = await ToleranceRangesDataAccess.Save(_sql, saveModel);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Delete tolerance range (soft delete)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="uuid">UUID of the tolerance range to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteToleranceRange(BasicModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid tolerance range selected." };

            var result = await ToleranceRangesDataAccess.Delete(_sql, basic, uuid);
            return result?.FirstOrDefault();
        }
    }
}
