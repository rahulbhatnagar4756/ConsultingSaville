using Library.Database.DAL;
using Library.Goals.DataAccess;
using Library.Goals.DataAccess.Status;
using Library.Goals.Mappers.Status;
using Library.Goals.Models;
using Library.Goals.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services
{
    public interface IStatusService
    {
        Task<List<StatusModel>?> GetStatus(BasicModel basic);
        Task<ResultsModel?> SaveStatus(BasicModel basic, StatusModel status);
        Task<ResultsModel?> DeleteStatus(BasicModel basic, string uuid);
        
        // StatusRanges methods
        Task<List<StatusRangesModel>?> GetStatusRanges(BasicModel basic, string statusUUID);
        Task<ResultsModel?> SaveStatusRange(BasicModel basic, StatusRangesModel range, string statusUUID);
        Task<ResultsModel?> DeleteStatusRange(BasicModel basic, string uuid);
    }

    public class StatusService : IStatusService
    {
        private readonly ISqlDataAccess _sql;

        public StatusService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Get all status records for a company
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <returns>List of status records</returns>
        public async Task<List<StatusModel>?> GetStatus(BasicModel basic)
        {
            var data = await StatusDataAccess.GetAll(_sql, basic);
            return data?.ToList();
        }

        /// <summary>
        /// Save status (create or update)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="status">Status model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveStatus(BasicModel basic, StatusModel status)
        {
            if (status == null || string.IsNullOrWhiteSpace(status.Name))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid status data." };

            var saveModel = status.StatusSaveModel(basic);
            var result = await StatusDataAccess.Save(_sql, saveModel);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Delete status (soft delete)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="uuid">UUID of the status to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteStatus(BasicModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid status selected." };

            var result = await StatusDataAccess.Delete(_sql, basic, uuid);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Get all status ranges for a specific status
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="statusUUID">UUID of the status</param>
        /// <returns>List of status ranges</returns>
        public async Task<List<StatusRangesModel>?> GetStatusRanges(BasicModel basic, string statusUUID)
        {
            if (string.IsNullOrEmpty(statusUUID))
                return null;

            var data = await StatusRangesDataAccess.GetAllByStatus(_sql, basic, statusUUID);
            return data?.ToList();
        }

        /// <summary>
        /// Save status range (create or update)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="range">Status range model</param>
        /// <param name="statusUUID">UUID of the status</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveStatusRange(BasicModel basic, StatusRangesModel range, string statusUUID)
        {
            if (range == null || string.IsNullOrEmpty(statusUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid status range data." };

            var saveModel = range.StatusRangesSaveModel(basic, statusUUID);
            var result = await StatusRangesDataAccess.Save(_sql, saveModel);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Delete status range (soft delete)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="uuid">UUID of the status range to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteStatusRange(BasicModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid status range selected." };

            var result = await StatusRangesDataAccess.Delete(_sql, basic, uuid);
            return result?.FirstOrDefault();
        }
    }
}
