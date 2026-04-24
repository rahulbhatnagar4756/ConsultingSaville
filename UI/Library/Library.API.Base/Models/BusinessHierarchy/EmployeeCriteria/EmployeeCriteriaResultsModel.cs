using Library.API.Base.Models.BusinessHierarchy.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.BusinessHierarchy.EmployeeCriteria
{
    public class EmployeeCriteriaResultsModel : ICloneable 
    {
        public List<TypesModel>? Departments { get; set; }
        public List<TypesModel>? BusinessUnits { get; set; }
        public List<TypesModel>? BusinessUnitTypes { get; set; }
        public List<EmployeeJobsModel>? Position { get; set; }
        public List<TypesModel>? Disciplines { get; set; }
        public List<TypesModel>? CriticalRoles { get; set; }
        public List<TypesModel>? Level { get; set; }
        public List<TypesModel>? Gender { get; set; }
        public List<TypesModel>? Ethnicity { get; set; }
        public List<TypesModel>? YearsOfExperience { get; set; }
        public List<TypesModel>? Age { get; set; }

        public object Clone()
        {
            return new EmployeeCriteriaResultsModel
            {
                Departments = Departments?.Select(d => (TypesModel)d.Clone()).ToList(),
                BusinessUnits = BusinessUnits?.Select(b => (TypesModel)b.Clone()).ToList(),
                BusinessUnitTypes = BusinessUnitTypes?.Select(b => (TypesModel)b.Clone()).ToList(),
                Position = Position?.Select(p => (EmployeeJobsModel)p.Clone()).ToList(),
                Disciplines = Disciplines?.Select(d => (TypesModel)d.Clone()).ToList(),
                CriticalRoles = CriticalRoles?.Select(c => (TypesModel)c.Clone()).ToList(),
                Level = Level?.Select(l => (TypesModel)l.Clone()).ToList(),
                Gender = Gender?.Select(g => (TypesModel)g.Clone()).ToList(),
                Ethnicity = Ethnicity?.Select(e => (TypesModel)e.Clone()).ToList(),
                YearsOfExperience = YearsOfExperience?.Select(y => (TypesModel)y.Clone()).ToList(),
                Age = Age?.Select(a => (TypesModel)a.Clone()).ToList()
            };
        }

     


    }
}
