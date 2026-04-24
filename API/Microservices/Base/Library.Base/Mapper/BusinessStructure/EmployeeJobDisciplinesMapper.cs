using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Mapper.BusinessStructure;

internal static class EmployeeJobDisciplinesMapper
{
    public static EmployeeJobDisciplineSaveModel ToEmployeeJobDisciplineSaveModel(this EmployeeJobDisciplineBaseModel job, EmployeeBasicGetModel basic)
    {
        return new EmployeeJobDisciplineSaveModel
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = job.UUID,
            Name = job.Name
        };
    }

}
