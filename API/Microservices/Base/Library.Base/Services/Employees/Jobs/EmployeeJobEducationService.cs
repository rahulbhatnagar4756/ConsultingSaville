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
    public interface IEmployeeJobEducationService
    {
        Task<List<EmployeeJobEducationBaseModel>?> GetEmployeeJobEducationByJobUUID(EmployeeBasicGetModel basic, string employeeJobsUUID);
        Task<ResultsModel?> SaveEmployeeJobEducation(EmployeeBasicGetModel basic, EmployeeJobEducationBaseModel education);
        Task<ResultsModel?> DeleteEmployeeJobEducation(EmployeeBasicGetModel basic, string uuid);
    }

    public class EmployeeJobEducationService : IEmployeeJobEducationService
    {
        private readonly ISqlDataAccess _sql;

        public EmployeeJobEducationService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Get employee job education records by specific job UUID
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="employeeJobsUUID"></param>
        /// <returns></returns>
        public async Task<List<EmployeeJobEducationBaseModel>?> GetEmployeeJobEducationByJobUUID(EmployeeBasicGetModel basic, string employeeJobsUUID)
        {
            if (string.IsNullOrEmpty(employeeJobsUUID)) return null;

            var data = await EmployeeJobEducationDataAccess.GetEmployeeJobEducationByJobUUID(_sql, basic, employeeJobsUUID);
            if (data == null || !data.Any()) return null;
            return data.ToList();
        }

        /// <summary>
        /// Save employee job education record
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="education"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> SaveEmployeeJobEducation(EmployeeBasicGetModel basic, EmployeeJobEducationBaseModel education)
        {
            if (education == null) return new ResultsModel { isValid = false, Message = "Invalid education data." };

            var saveModel = education.ToEmployeeJobEducationSaveModel(basic);
            var result = await EmployeeJobEducationDataAccess.SaveEmployeeJobEducation(_sql, saveModel);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Delete employee job education record
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> DeleteEmployeeJobEducation(EmployeeBasicGetModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid education record selected." };

            DeleteModel delete = new DeleteModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await EmployeeJobEducationDataAccess.DeleteEmployeeJobEducation(_sql, delete);
            return result?.FirstOrDefault();
        }
    }
}