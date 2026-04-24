using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees.BusinessStructure
{
    public class EmployeeBusinessUnitsSaveModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? UUID { get; set; }
        public string? EmployeeBusinessUnitTypesUUID { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; }
        public int? IconsId { get; set; }
        public string? IconColor { get; set; }
        public string? Icon { get; set; } 
    }
}
