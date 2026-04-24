using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.BusinessHierarchy.Jobs;

public class EmployeeJobsModel :ICloneable , ISelection
{
    public string? UUID { get; set; }
    public string? EmployeeDepartmentsUUID { get; set; }
    public string? EmployeeJobDisciplinesUUID { get; set; }
    public string? EmployeeJobsCriticalRolesUUID { get; set; }
    public string? EmployeeLevelsUUID { get; set; }
    public string? Name { get; set; }
    public string? FullName { get; set; }
    public string? Code { get; set; }
    public bool? isSelected { get; set; } = false;

    public object Clone()
    {
        return new EmployeeJobsModel
        {
            UUID = UUID,
            EmployeeDepartmentsUUID = EmployeeDepartmentsUUID,
            EmployeeJobDisciplinesUUID = EmployeeJobDisciplinesUUID,
            EmployeeJobsCriticalRolesUUID = EmployeeJobsCriticalRolesUUID,
            EmployeeLevelsUUID = EmployeeLevelsUUID,
            Name = Name,
            FullName = FullName,
            Code = Code,
            isSelected = isSelected
        };
    }
}


