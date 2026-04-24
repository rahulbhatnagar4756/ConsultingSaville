using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.BusinessHierarchy
{
    public class EmployeeDepartmentsModel
    {
        public string? UUID { get; set; }
        public string? EmployeeBusinessUnitsUUID { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Description { get; set; }

    }
}
