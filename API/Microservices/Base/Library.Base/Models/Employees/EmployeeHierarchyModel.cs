using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees
{
    public class EmployeeHierarchyModel
    {
        public string? EmployeeHierarchyUUID { get; set; }
        public string? EmployeeBusinessUnitUUID { get; set; }
        public string? EmployeeBusinessUnit { get; set; }

        public string? EmployeeDepartmentUUID { get; set; }
        public string? EmployeeDepartment { get; set; }

        public string? EmployeeJobsUUID { get; set; }
        public string? EmployeeJobs { get; set; }

        public string? Level { get; set; }
        public string? CriticalRoles { get; set; }
        public string? Disciplines { get; set; }
        public string? UsersUUIDManager { get; set; }
        public string? UserManager { get; set; }

        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }

        public bool? isArchive { get; set; }
    }
}
