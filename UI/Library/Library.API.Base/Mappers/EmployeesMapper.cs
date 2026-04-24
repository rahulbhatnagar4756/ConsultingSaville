using Library.API.Base.Models.BusinessHierarchy.EmployeeCriteria;
using Library.API.Base.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Mappers
{
    internal static class EmployeesMapper
    {
        public static void EmployeeCriteriaResultsModelToEmployeeSearchDTO(EmployeeCriteriaResultsModel? filterResults, EmployeeSearchModel search)
        {
            if (search == null && filterResults == null) return;

            if (filterResults.BusinessUnitTypes != null) search.BusinessUnitTypes = filterResults.BusinessUnitTypes.Where(x => x.isSelected == true).Select(x => x.UUID).ToList();
            if (filterResults.BusinessUnits != null) search.BusinessUnits = filterResults.BusinessUnits.Where(x => x.isSelected == true).Select(x => x.UUID).ToList();
            if (filterResults.Departments != null) search.Departments = filterResults.Departments.Where(x => x.isSelected == true).Select(x => x.UUID).ToList();
            if (filterResults.Position != null) search.Position = filterResults.Position.Where(x => x.isSelected == true).Select(x => x.UUID).ToList();
            if (filterResults.Disciplines != null) search.Disciplines = filterResults.Disciplines.Where(x => x.isSelected == true).Select(x => x.UUID).ToList();
            if (filterResults.CriticalRoles != null) search.CriticalRoles = filterResults.CriticalRoles.Where(x => x.isSelected == true).Select(x => x.UUID).ToList();
            if (filterResults.Level != null) search.Level = filterResults.Level.Where(x => x.isSelected == true).Select(x => x.UUID).ToList();
            if (filterResults.Gender != null) search.Gender = filterResults.Gender.Where(x => x.isSelected == true).Select(x => x.Name).ToList();
            if (filterResults.Ethnicity != null) search.Ethnicity = filterResults.Ethnicity.Where(x => x.isSelected == true).Select(x => x.Name).ToList();
            if (filterResults.YearsOfExperience != null) search.YearsOfExperience = filterResults.YearsOfExperience.Where(x => x.isSelected == true).Select(x => x.Name).ToList();
            if (filterResults.Age != null) search.Age = filterResults.Age.Where(x => x.isSelected == true).Select(x => x.Name).ToList();
        }




    }
}
