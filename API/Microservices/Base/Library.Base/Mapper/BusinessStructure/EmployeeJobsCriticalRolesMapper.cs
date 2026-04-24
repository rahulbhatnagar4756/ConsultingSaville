using Library.Base.Models.Employees;
using Library.Base.Models.Employees.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Mapper.BusinessStructure
{
    internal static class EmployeeJobsCriticalRolesMapper
    {
        public static EmployeeJobsCriticalRoleSaveModel ToEmployeeJobsCriticalRoleSaveModel(this EmployeeJobsCriticalRoleBaseModel criticalRole, EmployeeBasicGetModel basic)
        {
            return new EmployeeJobsCriticalRoleSaveModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = criticalRole.UUID,
                Name = criticalRole.Name ?? string.Empty,
                Description = criticalRole.Description,
                EmployeeJobsCriticalRoleLevelsid = 4 // Default level
            };
        }
    }
}
