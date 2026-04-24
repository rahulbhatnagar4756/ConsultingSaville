using Library.Base.DataAccess.Employees.Jobs;
using Library.Base.Mapper.BusinessStructure;
using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services.Employees.Jobs
{
    public interface IEmployeeJobDisciplinesService
    {
        Task<ResultsModel?> DeleteEmployeeJobDisciplines(EmployeeBasicGetModel basic, string uuid);
        Task<List<EmployeeJobDisciplineBaseModel>?> GetEmployeeJobDisciplines(EmployeeBasicGetModel basic);
        Task<ResultsModel?> SaveEmployeeJobDisciplines(EmployeeBasicGetModel basic, EmployeeJobDisciplineBaseModel discipline);
    }

    public class EmployeeJobDisciplinesService : IEmployeeJobDisciplinesService
    {
        private readonly ISqlDataAccess _sql;

        public EmployeeJobDisciplinesService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        //get the level data
        public async Task<List<EmployeeJobDisciplineBaseModel>?> GetEmployeeJobDisciplines(EmployeeBasicGetModel basic)
        {
            var Data = await EmployeeJobDisciplinesDataAccess.GetEmployeeJobDisciplinesBase(_sql, basic);
            if (Data == null || Data.Count() == 0) return default;
            return Data.ToList();
        }

        /// <summary>
        /// save the level
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="discipline"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> SaveEmployeeJobDisciplines(EmployeeBasicGetModel basic, EmployeeJobDisciplineBaseModel discipline)
        {
            if (discipline == null) return new ResultsModel { isValid = false, Message = "Invalid Discipline data." };

            var Save = discipline.ToEmployeeJobDisciplineSaveModel(basic);
            var result = await EmployeeJobDisciplinesDataAccess.SaveEmployeeJobDiscipline(_sql, Save);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// delete the level
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> DeleteEmployeeJobDisciplines(EmployeeBasicGetModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid Discipline selected." };
            DeleteModel delete = new DeleteModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await EmployeeJobDisciplinesDataAccess.DeleteEmployeeJobDiscipline(_sql, delete);
            return result.FirstOrDefault();
        }
    }
}
