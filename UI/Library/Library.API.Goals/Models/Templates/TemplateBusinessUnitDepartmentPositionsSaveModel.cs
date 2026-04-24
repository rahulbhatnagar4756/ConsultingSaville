using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Templates;

public class TemplateBusinessUnitDepartmentPositionsSaveModel
{
    public string? UUID { get; set; }
    public string? TemplatesUUID { get; set; }
    public string? EmployeeBusinessUnitsUUID { get; set; }
    public string? EmployeeDepartmentsUUID { get; set; }
    public string? EmployeeJobsUUID { get; set; }
}
