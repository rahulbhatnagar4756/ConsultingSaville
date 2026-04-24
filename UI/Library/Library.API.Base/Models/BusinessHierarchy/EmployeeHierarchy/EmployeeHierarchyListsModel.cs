using Library.API.Base.Models.BusinessHierarchy.EmployeeCriteria;
using Library.API.Base.Models.BusinessHierarchy.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.BusinessHierarchy.EmployeeHierarchy
{
    public class EmployeeHierarchyListsModel
    {
        public List<EmployeeBusinessUnitsModel>? BusinessUnits { get; set; }
        public List<EmployeeDepartmentsModel>? Departments { get; set; }
        public List<EmployeeJobsModel>? Jobs { get; set; }
        public List<EmployeeCriticalRolesModel>? CriticalRoles { get; set; }
        public List<EmployeeJobDisciplinesModel>? Disciplines { get; set; }
        public List<EmployeeLevelsModel>? Levels { get; set; }

    }
}
