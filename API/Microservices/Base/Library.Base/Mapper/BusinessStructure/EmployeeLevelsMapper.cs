using Library.Base.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Mapper.BusinessStructure;

internal static class EmployeeLevelsMapper
{
    public static EmployeeLevelSaveModel ToEmployeeLevelSaveModel(this EmployeeLevelBaseModel level, EmployeeBasicGetModel basic)
    {
        return new EmployeeLevelSaveModel
        {
            CompanyUUID = basic.CompanyUUID,
            UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
            UUID = level.UUID,
            Name = level.Name,
            Description = level.Description,
            LevelOrder = level.LevelOrder, 
        };
    }
}
