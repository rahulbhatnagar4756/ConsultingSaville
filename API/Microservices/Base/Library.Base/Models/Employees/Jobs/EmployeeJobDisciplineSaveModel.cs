using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees.Jobs
{
    internal class EmployeeJobDisciplineSaveModel
    {
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string? UUID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
