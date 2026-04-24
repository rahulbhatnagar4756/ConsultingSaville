using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees
{
    public class EmployeeHierarchyBasicModel
    {
        public string? EmployeeHierarchyUUID { get; set; }
        public string? UsersUUID { get; set; }
        public string? UsersUUIDManager { get; set; }
        public string? Position { get; set; }
        public DateTime? DateStartedAtPosition { get; set; }
    }
}
