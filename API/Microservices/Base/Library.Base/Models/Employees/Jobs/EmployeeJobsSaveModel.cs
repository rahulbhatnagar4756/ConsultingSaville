using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees.Jobs;

public class EmployeeJobsSaveModel
{
    public string? CompanyUUID { get; set; }
    public string? UsersUUIDLoggedIn { get; set; }
    public string? UUID { get; set; }
    public string? EmployeeDepartmentsUUID { get; set; }
    public string? EmployeeJobDisciplinesUUID { get; set; }
    public string? EmployeeJobsCriticalRolesUUID { get; set; }
    public string? EmployeeLevelsUUID { get; set; }
    public string? JobsUUID { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
}
