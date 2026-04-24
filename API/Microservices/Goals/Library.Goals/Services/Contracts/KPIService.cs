using Library.Database.DAL;
using Library.Goals.DataAccess.Contracts;
using Library.Goals.Models;
using Library.Goals.Models.KPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services.Contracts;
 
    public interface IKPIService
    {
        Task<ResultsModel?> Save(KPISaveModel saveModel);
        Task<ResultsModel?> Delete(BasicModel basic, string uuid);
    }

    internal class KPIService : IKPIService
    {
        private readonly ISqlDataAccess _sql;

        public KPIService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Save a KPI (create or update)
        /// </summary>
        /// <param name="saveModel">KPI save model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> Save(KPISaveModel saveModel)
        {
            try
            {
                var results = await KPIDataAccess.Save(_sql, saveModel);
                return results?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel
                {
                    UUID = null,
                    isValid = false,
                    Message = $"Error saving KPI: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Delete a KPI
        /// </summary>
        /// <param name="basic">Basic model with company and user information</param>
        /// <param name="uuid">UUID of the KPI to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> Delete(BasicModel basic, string uuid)
        {
            try
            {
                var results = await KPIDataAccess.Delete(_sql, basic, uuid);
                return results?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new ResultsModel
                {
                    UUID = null,
                    isValid = false,
                    Message = $"Error deleting KPI: {ex.Message}"
                };
            }
        }
    }
