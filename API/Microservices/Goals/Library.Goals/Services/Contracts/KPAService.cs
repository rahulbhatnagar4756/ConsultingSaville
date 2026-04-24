using Library.Database.DAL;
using Library.Goals.DataAccess.Contracts;
using Library.Goals.Models;
using Library.Goals.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services.Contracts
{
    public interface IKPAService
    {
        Task<ResultsModel?> Save(ContractKPASaveModel saveModel);
        Task<ResultsModel?> DeleteKPA(BasicModel basic, string uuid);
    }

    public class KPAService : IKPAService
    {
        private readonly ISqlDataAccess _sql;

        public KPAService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Save a Contract KPA (create or update)
        /// </summary>
        /// <param name="saveModel">KPA save model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> Save(ContractKPASaveModel saveModel)
        {
            if (saveModel == null || string.IsNullOrWhiteSpace(saveModel.Name))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPA data." };

            if (string.IsNullOrWhiteSpace(saveModel.CompanyUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Company UUID is required." };

            if (string.IsNullOrWhiteSpace(saveModel.UsersUUIDLoggedIn))
                return new ResultsModel { UUID = null, isValid = false, Message = "User UUID is required." };

            if (string.IsNullOrWhiteSpace(saveModel.ContractPillarsUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Contract Pillar UUID is required." };

            if (string.IsNullOrWhiteSpace(saveModel.StatusUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Status UUID is required." };

            if (saveModel.Weight < 0 || saveModel.Weight > 100)
                return new ResultsModel { UUID = null, isValid = false, Message = "Weight must be between 0 and 100." };

            try
            {
                var result = await KPADataAccess.Save(_sql, saveModel);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error saving KPA: {ex.Message}" };
            }
        }

        /// <summary>
        /// Delete a Contract KPA (soft delete)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="uuid">UUID of the KPA to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeleteKPA(BasicModel basic, string uuid)
        {
            if (basic == null)
                return new ResultsModel { UUID = null, isValid = false, Message = "Basic model is required." };

            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid KPA selected." };

            if (string.IsNullOrWhiteSpace(basic.CompanyUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Company UUID is required." };

            if (string.IsNullOrWhiteSpace(basic.UsersUUIDLoggedIn))
                return new ResultsModel { UUID = null, isValid = false, Message = "User UUID is required." };

            try
            {
                var result = await KPADataAccess.Delete(_sql, basic, uuid);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel { UUID = null, isValid = false, Message = $"Error deleting KPA: {ex.Message}" };
            }
        }
    }
}
