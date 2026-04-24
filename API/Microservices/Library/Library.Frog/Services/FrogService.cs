using Library.Database.DAL;
using Library.Frog.DTO;
using Library.Frog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.Services
{
    public interface IFrogService
    {
        Task<ResultModel?> ChangeStatus(FrogChangeStatusModel elementResult);
        Task<List<FrogElementModel>?> GetElements(FormSetsAccessModel access);
        Task<FrogElementModel?> GetElements(FormSetsAccessModel access, string elementUUID);
        Task<List<FrogLookupModel>?> GetLookups(int lookupTypeId);
        Task<List<FrogLookupModel>?> GetLookupsByStoredProcedure(string companyUUID, string usersUUIDLoggedIn, string FrogsUUID, string storedProcedureUUID);
        Task<ResultModel?> Save(FrogElementSaveModel elementResult);
        Task<ResultModel?> Save(FrogElementSaveLogModel elementResult);
    }

    public class FrogService : IFrogService
    {
        ISqlDataAccess _sql;

        public FrogService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<Models.FrogElementModel>?> GetElements(Models.FormSetsAccessModel access)
        {
            var results = await DataAccess.FrogElementsDataAccess.GetElements(_sql, access);
            return results?.ToList() ?? null;
        }

        public async Task<Models.FrogElementModel?> GetElements(Models.FormSetsAccessModel access, string elementUUID)
        {
            var results = await DataAccess.FrogElementsDataAccess.GetElements(_sql, access);
            if (results == null) return default;
            var result = results.FirstOrDefault(x => x.FrogElementsUUID == elementUUID);
            return result;
        }

        /// <summary>
        /// get the lookup for the selected lookup type
        /// </summary>
        /// <param name="lookupTypeId"></param>
        /// <returns></returns>
        public async Task<List<Models.FrogLookupModel>?> GetLookups(int lookupTypeId)
        {
            if (lookupTypeId == 0) return null;
            IEnumerable<Models.FrogLookupModel> results = await DataAccess.FrogLookupDataAccess.GetLookups(_sql, lookupTypeId);
            return results?.ToList() ?? null;
        }

        public async Task<List<Models.FrogLookupModel>?> GetLookupsByStoredProcedure(string companyUUID, string usersUUIDLoggedIn, string FrogsUUID, string storedProcedureUUID)
        {
            FrogSQLStoredProceduresParameterTypesDTO Parameters = new FrogSQLStoredProceduresParameterTypesDTO
            {
                CompanyUUID = companyUUID,
                UsersUUIDLoggedIn = usersUUIDLoggedIn,
                FrogsUUID = FrogsUUID,
                FrogSQLStoredProceduresUUID = storedProcedureUUID
            };

            Helpers.IFrogSQLStoredProceduresBuilder builder = new Helpers.FrogSQLStoredProceduresBuilder(_sql);
            return await builder.GenerateData(Parameters);
        }

        public async Task<Models.ResultModel?> Save(Models.FrogElementSaveModel elementResult)
        {
            if (elementResult == null) return new ResultModel { isSuccess = false, Message = "Failed to save data" };
            IEnumerable<Models.ResultModel> results = await DataAccess.FrogElementsDataAccess.Save(_sql, elementResult);
            if (results ==null || results.Count() == 0) return new ResultModel { isSuccess = false, Message="Failed to save data" };
            return results.FirstOrDefault(); 
        }

        public async Task<Models.ResultModel?> Save(Models.FrogElementSaveLogModel elementResult)
        {
            if (elementResult == null) return new ResultModel { isSuccess = false, Message = "Failed to save data" };
            IEnumerable<Models.ResultModel> results = await DataAccess.FrogElementsDataAccess.Save(_sql, elementResult);
            if (results == null || results.Count() == 0) return new ResultModel { isSuccess = false, Message = "Failed to save data" };
            return results.FirstOrDefault();
        }

        public async Task<Models.ResultModel?> ChangeStatus(Models.FrogChangeStatusModel elementResult)
        {
            if (elementResult == null) return new ResultModel { isSuccess = false, Message = "Failed to save data" };
            IEnumerable<Models.ResultModel> results = await DataAccess.FrogDataAccess.ChangeStatus(_sql, elementResult);
            if (results == null || results.Count() == 0) return new ResultModel { isSuccess = false, Message = "Failed to save data" };
            return results.FirstOrDefault();
        }
    }
}
