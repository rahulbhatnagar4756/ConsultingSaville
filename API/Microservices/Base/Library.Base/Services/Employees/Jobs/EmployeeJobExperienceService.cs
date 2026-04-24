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
    public interface IEmployeeJobExperienceService
    {
        Task<List<EmployeeJobExperienceBaseModel>?> GetEmployeeJobExperienceByJobUUID(EmployeeBasicGetModel basic, string employeeJobsUUID);
        Task<ResultsModel?> SaveEmployeeJobExperience(EmployeeBasicGetModel basic, EmployeeJobExperienceBaseModel experience);
        Task<ResultsModel?> DeleteEmployeeJobExperience(EmployeeBasicGetModel basic, string uuid);
    }

    public class EmployeeJobExperienceService : IEmployeeJobExperienceService
    {
        private readonly ISqlDataAccess _sql;

        public EmployeeJobExperienceService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Get employee job experience records by specific job UUID
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="employeeJobsUUID"></param>
        /// <returns></returns>
        public async Task<List<EmployeeJobExperienceBaseModel>?> GetEmployeeJobExperienceByJobUUID(EmployeeBasicGetModel basic, string employeeJobsUUID)
        {
            if (string.IsNullOrEmpty(employeeJobsUUID)) return null;

            var data = await EmployeeJobExperienceDataAccess.GetEmployeeJobExperienceByJobUUID(_sql, basic, employeeJobsUUID);
            if (data == null || !data.Any()) return null;
            return data.ToList();
        }

        /// <summary>
        /// Save employee job experience record
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="experience"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> SaveEmployeeJobExperience(EmployeeBasicGetModel basic, EmployeeJobExperienceBaseModel experience)
        {
            if (experience == null) return new ResultsModel { isValid = false, Message = "Invalid experience data." };

            var saveModel = experience.ToEmployeeJobExperienceSaveModel(basic);
            var result = await EmployeeJobExperienceDataAccess.SaveEmployeeJobExperience(_sql, saveModel);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Delete employee job experience record
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> DeleteEmployeeJobExperience(EmployeeBasicGetModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid experience record selected." };

            DeleteModel delete = new DeleteModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await EmployeeJobExperienceDataAccess.DeleteEmployeeJobExperience(_sql, delete);
            return result?.FirstOrDefault();
        }
    }
}
