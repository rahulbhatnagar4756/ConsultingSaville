using Library.Base.Models.Employees;
using Library.Base.Models.Employees.BusinessStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Mapper.BusinessStructure;

internal static class EmployeeDepartmentsMapper
{
    public static EmployeeDepartmentsSaveModel ToEmployeeDepartmentsSaveModel(this EmployeeDepartmentBaseModel departments, EmployeeBasicGetModel basic)
    {
        return new EmployeeDepartmentsSaveModel
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = departments.UUID,
            EmployeeBusinessUnitsUUID = departments.ParentsUUID,
            Name = departments.Name,
            Description = departments.Description,
            IconsId = departments.IconsId,
            IconColor = departments.IconColor,
            Icon = departments.Icon
        };
    }
}
