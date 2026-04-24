using Library.Base.Models.Employees;
using Library.Base.Models.Employees.BusinessStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Mapper.BusinessStructure
{
    internal static class EmployeeBusinessUnitTypesMapper
    {
        public static EmployeeBusinessUnitTypesSaveModel ToEmployeeBusinessUnitTypesSaveModel(this EmployeeBaseModel businessUnit, EmployeeBasicGetModel basic)
        {
            return new EmployeeBusinessUnitTypesSaveModel
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = businessUnit.UUID,
                Name = businessUnit.Name,
                Description = businessUnit.Description,
                IconsId = businessUnit.IconsId,
                IconColor = businessUnit.IconColor,
                Icon = businessUnit.Icon
            };
        }
    }
}
