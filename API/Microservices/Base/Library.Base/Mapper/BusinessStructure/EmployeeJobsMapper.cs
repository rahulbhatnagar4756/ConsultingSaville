using Library.Base.Models.Employees;
using Library.Base.Models.Employees.BusinessStructure;
using Library.Base.Models.Employees.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Mapper.BusinessStructure
{
    internal static class EmployeeJobsMapper
    {
        public static EmployeeJobsSaveModel ToEmployeeJobsSaveModel(this EmployeeJobsBaseModel job, EmployeeBasicGetModel basic)
        {
            return new EmployeeJobsSaveModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = job.UUID,
                EmployeeDepartmentsUUID = job.EmployeeDepartmentsUUID,
                EmployeeJobDisciplinesUUID = job.EmployeeJobDisciplinesUUID,
                EmployeeJobsCriticalRolesUUID = job.EmployeeJobsCriticalRolesUUID,
                EmployeeLevelsUUID = job.EmployeeLevelsUUID,
                Name = job.Name,
                Code = job.Code,
                Description = job.Description,
                JobsUUID = job.JobsUUID, 
            };

        }


    }
}
