using Library.Assess.Mapper.Jobs;
using Library.Assess.Models;
using Library.Assess.Models.Jobs;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Services.Jobs
{
    public interface IJobsService
    {
        Task<List<JobBaseModel>?> GetJobs(BasicGetModel basic);
        Task<ResultsModel?> SaveJob(BasicGetModel basic, JobBaseModel job);
        Task<ResultsModel?> DeleteJob(BasicGetModel basic, string uuid);
    }

    public class JobsService : IJobsService
    {
        private readonly ISqlDataAccess _sql;

        public JobsService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<JobBaseModel>?> GetJobs(BasicGetModel basic)
        {
            var data = await DataAccess.Jobs.JobsDataAccess.GetJobs(_sql, basic);
            if (data == null || !data.Any()) return null;
            return data.ToList();
        }

        public async Task<ResultsModel?> SaveJob(BasicGetModel basic, JobBaseModel job)
        {
            if (job == null) return new ResultsModel { isValid = false, Message = "Invalid job data." };

            var saveModel = job.ToJobSaveModel(basic);
            var result = await DataAccess.Jobs.JobsDataAccess.SaveJob(_sql, saveModel);
            return result?.FirstOrDefault();
        }

        public async Task<ResultsModel?> DeleteJob(BasicGetModel basic, string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return new ResultsModel { isValid = false, Message = "Invalid job selected." };

            DeleteModel delete = new DeleteModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = uuid
            };

            var result = await DataAccess.Jobs.JobsDataAccess.DeleteJob(_sql, delete);
            return result?.FirstOrDefault();
        }
    }
}
