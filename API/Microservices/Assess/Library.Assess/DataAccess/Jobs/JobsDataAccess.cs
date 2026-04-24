using Library.Assess.Models;
using Library.Assess.Models.Jobs;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.DataAccess.Jobs
{
    internal static class JobsDataAccess
    {
        public static async Task<IEnumerable<JobBaseModel>> GetJobs(ISqlDataAccess sql, BasicGetModel basic) =>
            await sql.LoadDataAsync<JobBaseModel, dynamic>("[Assess].[spJobs]", basic);

        public static async Task<IEnumerable<ResultsModel>> SaveJob(ISqlDataAccess sql, JobSaveModel model) =>
            await sql.LoadDataAsync<ResultsModel, JobSaveModel>("[Assess].[spJobsSave]", model);

        public static async Task<IEnumerable<ResultsModel>> DeleteJob(ISqlDataAccess sql, DeleteModel model) =>
            await sql.LoadDataAsync<ResultsModel, DeleteModel>("[Assess].[spJobsDelete]", model);
    }
}
