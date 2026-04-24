using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees;

public class EmployeeSearchResultsModel
{
    public List<TypesModel>? Departments { get; set; }
    public List<TypesModel>? BusinessUnits { get; set; }
    public List<TypesModel>? BusinessUnitTypes { get; set; }
    public List<EmployeeJobsModel>? Position { get; set; }
    public List<TypesModel>? Disciplines { get; set; }
    public List<TypesModel>? CriticalRoles { get; set; }
    public List<TypesModel>? Level { get; set; }
    public List<LookupBasicModel>? Gender { get; set; }
    public List<LookupBasicModel>? Ethnicity { get; set; }
    
    public List<TypesModel>? YearsOfExperience { get; set; }
    public List<TypesModel>? Age { get; set; } 
}
