using Library.API.Base.Models.Employees;
using Library.API.TalentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.TalentManagement.Mapper;

public static class SearchModelMapper
{
    public static SearchModel MapperEmployeeModel(this EmployeeSearchModel employeeSearch)
    {
        return new SearchModel
        {
            Search = employeeSearch.Search,
            isIncludeTeam = employeeSearch.isIncludeTeam,
            Position = employeeSearch.Position,
            Departments = employeeSearch.Departments,
            BusinessUnits = employeeSearch.BusinessUnits,
            BusinessUnitTypes = employeeSearch.BusinessUnitTypes,
            Level = employeeSearch.Level,
            CriticalRoles = employeeSearch.CriticalRoles,
            Disciplines = employeeSearch.Disciplines,
            Gender = employeeSearch?.Gender,
            Ethnicity = employeeSearch.Ethnicity,
            YearsOfExperience = employeeSearch.YearsOfExperience,
            Age = employeeSearch.Age,
            isFilterOnlyTalentpool = employeeSearch.isFilterOnlyTalentpool,
            isFilterOnlySuccessionReadiness = employeeSearch.isFilterOnlySuccessionReadiness,
            isFilterOnly9BoxPlotted = employeeSearch.isFilterOnly9BoxPlotted,
            isFilterOnlyOutStanding = employeeSearch.isFilterOnlyOutStanding
        };
    }
}