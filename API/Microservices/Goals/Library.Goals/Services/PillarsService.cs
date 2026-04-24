using Library.Database.DAL;
using Library.Goals.DataAccess;
using Library.Goals.Mappers; 
using Library.Goals.Models;
using Library.Goals.Models.Pillars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services
{
    public interface IPillarsService
    {
        Task<List<PillarsModel>?> GetPillars(BasicModel basic);
        Task<ResultsModel?> SavePillar(BasicModel basic, PillarsModel pillar);
        Task<ResultsModel?> DeletePillar(BasicModel basic, string uuid);
    }

    public class PillarsService : IPillarsService
    {
        private readonly ISqlDataAccess _sql;

        public PillarsService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Get all pillars for a company
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <returns>List of pillars</returns>
        public async Task<List<PillarsModel>?> GetPillars(BasicModel basic)
        {
            var data = await PillarsDataAccess.GetAll(_sql, basic);
            return data?.ToList();
        }

        /// <summary>
        /// Save pillar (create or update)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="pillar">Pillar model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SavePillar(BasicModel basic, PillarsModel pillar)
        {
            if (pillar == null || string.IsNullOrWhiteSpace(pillar.Name))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid pillar data." };

            var saveModel = pillar.ToPillarsSaveModel(basic);
            var result = await PillarsDataAccess.Save(_sql, saveModel);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Delete pillar (soft delete)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="uuid">UUID of the pillar to delete</param>
        /// <returns>Result of the delete operation</returns>
        public async Task<ResultsModel?> DeletePillar(BasicModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid pillar selected." };

            var result = await PillarsDataAccess.Delete(_sql, basic, uuid);
            return result?.FirstOrDefault();
        }
    }
}
