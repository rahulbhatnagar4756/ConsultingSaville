using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Validators
{
    public static class Employees
    {
        public static bool CompareEmployeeSearchModels(this Models.Employees.EmployeeSearchModel Original, Models.Employees.EmployeeSearchModel Compare)
        {
            if (Original == null && Compare == null) return true;
            if (Original == null) return false;
            if (Compare == null) return false;
            if (Original.Search != Compare.Search) return false;
            if (Original.isIncludeTeam != Compare.isIncludeTeam) return false;
            if (Original.Position == null && Compare.Position != null) return false;
            if (Original.Position != null && Compare.Position == null) return false;
            if (Original.Position != null && Compare.Position != null && !Original.Position.SequenceEqual(Compare.Position)) return false;
            if (Original.Departments == null && Compare.Departments != null) return false;
            if (Original.Departments != null && Compare.Departments == null) return false;
            if (Original.Departments != null && Compare.Departments != null && !Original.Departments.SequenceEqual(Compare.Departments)) return false;
            if (Original.BusinessUnits == null && Compare.BusinessUnits != null) return false;
            if (Original.BusinessUnits != null && Compare.BusinessUnits == null) return false;
            if (Original.BusinessUnits != null && Compare.BusinessUnits != null && !Original.BusinessUnits.SequenceEqual(Compare.BusinessUnits)) return false;
            if (Original.BusinessUnitTypes == null && Compare.BusinessUnitTypes != null) return false;
            if (Original.BusinessUnitTypes != null && Compare.BusinessUnitTypes == null) return false;
            if (Original.BusinessUnitTypes != null && Compare.BusinessUnitTypes != null && !Original.BusinessUnitTypes.SequenceEqual(Compare.BusinessUnitTypes)) return false;
            if (Original.Level == null && Compare.Level != null) return false;
            if (Original.Level != null && Compare.Level == null) return false;
            if (Original.Level != null && Compare.Level != null && !Original.Level.SequenceEqual(Compare.Level)) return false;
            if (Original.CriticalRoles == null && Compare.CriticalRoles != null) return false;
            if (Original.CriticalRoles != null && Compare.CriticalRoles == null) return false;
            if (Original.CriticalRoles != null && Compare.CriticalRoles != null && !Original.CriticalRoles.SequenceEqual(Compare.CriticalRoles)) return false;
            if (Original.Disciplines == null && Compare.Disciplines != null) return false;
            if (Original.Disciplines != null && Compare.Disciplines == null) return false;
            if (Original.Disciplines != null && Compare.Disciplines != null && !Original.Disciplines.SequenceEqual(Compare.Disciplines)) return false;
            if (Original.YearsOfExperience == null && Compare.YearsOfExperience != null) return false;
            if (Original.YearsOfExperience != null && Compare.YearsOfExperience == null) return false;
            if (Original.YearsOfExperience != null && Compare.YearsOfExperience != null && !Original.YearsOfExperience.SequenceEqual(Compare.YearsOfExperience)) return false;

            if (Original.Gender == null && Compare.Gender != null) return false;
            if (Original.Gender != null && Compare.Gender == null) return false;
            if (Original.Gender != null && Compare.Gender != null && !Original.Gender.SequenceEqual(Compare.Gender)) return false;

            if (Original.Ethnicity == null && Compare.Ethnicity != null) return false;
            if (Original.Ethnicity != null && Compare.Ethnicity == null) return false;
            if (Original.Ethnicity != null && Compare.Ethnicity != null && !Original.Ethnicity.SequenceEqual(Compare.Ethnicity)) return false;

            if (Original.Age == null && Compare.Age != null) return false;
            if (Original.Age != null && Compare.Age == null) return false;
            if (Original.Age != null && Compare.Age != null && !Original.Age.SequenceEqual(Compare.Age)) return false;
            if (Original.Box9 == null && Compare.Box9 != null) return false;
            if (Original.Box9 != null && Compare.Box9 == null) return false;
            if (Original.Box9 != null && Compare.Box9 != null && !Original.Box9.SequenceEqual(Compare.Box9)) return false;
            if (Original.isCPP != Compare.isCPP) return false;
            if (Original.isGoalFinalScore != Compare.isGoalFinalScore) return false;
            if (Original.isPersonality != Compare.isPersonality) return false;

            return true;
        }
        


    }
}
