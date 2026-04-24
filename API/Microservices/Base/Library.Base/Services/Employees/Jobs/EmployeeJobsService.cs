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
    public interface IEmployeeJobsService
    {
        Task<ResultsModel?> DeleteEmployeeJobs(EmployeeBasicGetModel basic, string uuid);
        Task<List<EmployeeJobsBaseModel>?> GetEmployeeJobs(EmployeeBasicGetModel basic);
        Task<ResultsModel?> SaveEmployeeJobs(EmployeeBasicGetModel basic, EmployeeJobsBaseModel model);
    }

    public class EmployeeJobsService : IEmployeeJobsService
    {
        private readonly ISqlDataAccess _sql;
        public EmployeeJobsService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// get the employee jobs
        /// </summary>
        /// <param name="basic"></param>
        /// <returns></returns>
        public async Task<List<EmployeeJobsBaseModel>?> GetEmployeeJobs(EmployeeBasicGetModel basic)
        {
            var Data = await EmployeeJobsDataAccess.GetEmployeeBaseJobs(_sql, basic);
            if (Data == null || Data.Count() == 0) return default;
            return Data.ToList();
        }

        /// <summary>
        /// save the employee jobs
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> SaveEmployeeJobs(EmployeeBasicGetModel basic, EmployeeJobsBaseModel model)
        {
            if (model == null) return new ResultsModel { isValid = false, Message = "Invalid job data." };

            var save = model.ToEmployeeJobsSaveModel(basic);
            var result = await EmployeeJobsDataAccess.SaveEmployeeBaseJobs(_sql, save);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// delete employee jobs
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<ResultsModel?> DeleteEmployeeJobs(EmployeeBasicGetModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid job selected." };
            DeleteModel delete = new DeleteModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };
            var result = await EmployeeJobsDataAccess.DeleteEmployeeBaseJobs(_sql, delete);
            return result.FirstOrDefault();
        }


    }
}
