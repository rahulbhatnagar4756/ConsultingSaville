using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.Models;

public class SuccessionPlanningModel
{
    public string? EmployeeJobsGenericNamesUUID { get; set; }
    public string? JobName { get; set; }
    public string? Level { get; set; }

    public string? UsersUUID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? IDNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? Gender { get; set; }
    public string? Race { get; set; }
    public string? EmployeeBusinessUnitTypes { get; set; }
    public string? EmployeeBusinessUnits { get; set; }
    public string? EmployeeDepartments { get; set; }
    public string? EmployeeJobs { get; set; }
    public string? EmployeeLevel { get; set; }
    public string? EmployeeJobsName { get; set; }
    public string? FrogsUUID { get; set; }
    public string? FrogElementsUUID { get; set; }
    public string? ItemUUIDReadiness { get; set; }
    public string? Readiness { get; set; }
    public int? YearsOfExperienceYear { get; set; }
    public int? YearsOfExperienceMonth { get; set; }
    public int? Box9Score { get; set; }
    public string? Box9 { get; set; }
}
