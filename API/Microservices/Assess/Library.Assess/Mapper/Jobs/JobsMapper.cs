using Library.Assess.Models;
using Library.Assess.Models.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Mapper.Jobs
{
    internal static class JobsMapper
    {
        public static JobSaveModel ToJobSaveModel(this JobBaseModel job, BasicGetModel basic)
        {
            return new JobSaveModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = job.UUID,
                JobName = job.Name ?? string.Empty,
                JobType = job.JobType,
                isCritical = job.isCritical ?? 0
            };
        }
    }
}
