using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees.Jobs
{
    public class EmployeeJobsBaseModel
    {
        public string UUID { get; set; } 
        public string Name { get; set; } 
        public string? Code { get; set; }
        public string? Level { get; set; }
        public string? Description { get; set; }
        public string? EmployeeBusinessUnitTypesUUID { get; set; }
        public string? EmployeeBusinessUnitTypes { get; set; }
        public string? EmployeeBusinessUnitsUUID { get; set; }
        public string? EmployeeBusinessUnits { get; set; }
        public string? EmployeeDepartmentsUUID { get; set; }
        public string? EmployeeDepartments { get; set; }
        public string? EmployeeJobDisciplinesUUID { get; set; }
        public string? EmployeeJobDisciplines { get; set; }
        public string? EmployeeJobsCriticalRolesUUID { get; set; }
        public string? EmployeeJobsCriticalRoles { get; set; }
        public string? EmployeeLevelsUUID { get; set; }
        public string? JobsUUID { get; set; }
        public string? JobsName { get; set; }
        public int OrderVal { get; set; }
    }
}
