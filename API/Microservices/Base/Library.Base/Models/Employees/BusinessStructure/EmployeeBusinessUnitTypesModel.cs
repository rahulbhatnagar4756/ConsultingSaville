using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees.BusinessStructure
{
    public class EmployeeBusinessUnitTypesModel
    {
        public string? UUID { get; set; }
        public string? CompanyUUID { get; set; }
        public string? Name { get; set; }
        public int? IconsId { get; set; }
        public string? IconColor { get; set; }
        public string? Icon { get; set; }
        public bool IsDeleted { get; set; }
    }
}
