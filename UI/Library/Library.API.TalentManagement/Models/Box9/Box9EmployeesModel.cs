using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.TalentManagement.Models.Box9
{
    public class Box9EmployeesModel
    {
        public string? Box9 { get; set; }
        public string? Level { get; set; }
        public string? Order { get; set; } = 200.ToString();
        public List<Library.API.Base.Models.Employees.EmployeesModel>? Employees { get; set; }
    }
}
