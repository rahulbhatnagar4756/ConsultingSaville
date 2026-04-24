using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees
{
    public class EmployeeSearchModel
    {
        public string? Search { get; set; }
        public bool isTeam { get; set; }
        public List<string>? UsersUUID { get; set; }
        public List<string>? Departments { get; set; }
        public List<string>? BusinessUnits { get; set; }
        public List<string>? BusinessUnitTypes { get; set; }
        public List<string>? Position { get; set; }
        public List<string>? Disciplines { get; set; }
        public List<string>? CriticalRoles { get; set; }
        public List<string>? Level { get; set; }
        public List<string>? Gender { get; set; }
        public List<string>? Ethnicity { get; set; }
        public List<string>? YearsOfExperience { get; set; }
        public List<string>? Age { get; set; }
        public List<int>? Box9 { get; set; }
        public bool? isCPP { get; set; }
        public bool? isGoalFinalScore { get; set; }
        public bool? isPersonality { get; set; }

        public bool? isFilterOnlyTalentpool { get; set; }
        public bool? isFilterOnlySuccessionReadiness { get; set; }
        public bool? isFilterOnly9BoxPlotted { get; set; }
        public bool? isFilterOnlyOutStanding { get; set; }
    }
}
