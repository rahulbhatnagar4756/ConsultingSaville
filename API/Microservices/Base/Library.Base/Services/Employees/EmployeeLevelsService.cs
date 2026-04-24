using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Base.Mapper.BusinessStructure;
using Library.Base.DataAccess.Employees.Jobs;

namespace Library.Base.Services.Employees
{
    public interface IEmployeeLevelsService
    {
        Task<ResultsModel?> DeleteEmployeeLevels(EmployeeBasicGetModel basic, string uuid);
        Task<List<EmployeeLevelBaseModel>?> GetEmployeeLevels(EmployeeBasicGetModel basic);
        Task<ResultsModel?> SaveEmployeeLevels(EmployeeBasicGetModel basic, EmployeeLevelBaseModel level);
    }

    public class EmployeeLevelsService : IEmployeeLevelsService
    {
        private readonly ISqlDataAccess _sql;

        public EmployeeLevelsService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        //get the level data
        public async Task<List<EmployeeLevelBaseModel>?> GetEmployeeLevels(Models.Employees.EmployeeBasicGetModel basic)
        {
            var Data = await EmployeeLevelsDataAccess.GetEmployeeLevelsBase(_sql, basic);
            if (Data == null || Data.Count() == 0) return default;
            return Data.ToList();
        }

        /// <summary>
        /// save the level
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> SaveEmployeeLevels(Models.Employees.EmployeeBasicGetModel basic, EmployeeLevelBaseModel level)
        {
            if (level == null) return new ResultsModel { isValid = false, Message = "Invalid level data." };

            var Save = level.ToEmployeeLevelSaveModel(basic);
            var result = await EmployeeLevelsDataAccess.SaveEmployeeLevels(_sql, Save);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// delete the level
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> DeleteEmployeeLevels(Models.Employees.EmployeeBasicGetModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid level selected." };
            DeleteModel delete = new DeleteModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await EmployeeLevelsDataAccess.DeleteEmployeeLevels(_sql, delete);
            return result.FirstOrDefault();
        }

    }
}
