using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.TalentManagement.Models
{
    public class SearchModel : ICloneable 
    {
        public string? Search { get; set; }
        public bool? isIncludeTeam { get; set; }
        public List<string>? UsersUUID { get; set; }
        public List<string>? Position { get; set; }
        public List<string>? Departments { get; set; }
        public List<string>? BusinessUnits { get; set; }
        public List<string>? BusinessUnitTypes { get; set; }
        public List<string>? Level { get; set; }
        public List<string>? CriticalRoles { get; set; }
        public List<string>? Disciplines { get; set; }
        public List<string>? Gender { get; set; }
        public List<string>? Ethnicity { get; set; }
        public List<string>? YearsOfExperience { get; set; }
        public List<string>? Age { get; set; }
        public bool? isOnlyAvailableJobs { get; set; } = true;
        public bool? isFilterOnlyTalentpool { get; set; }
        public bool? isFilterOnlySuccessionReadiness { get; set; }
        public bool? isFilterOnly9BoxPlotted { get; set; }
        public bool? isFilterOnlyOutStanding { get; set; }



        public object Clone()
        {
            return new SearchModel
            {
                Search = this.Search,
                isIncludeTeam = this.isIncludeTeam,
                UsersUUID = this.UsersUUID != null ? new List<string>(this.UsersUUID) : null,
                Position = this.Position != null ? new List<string>(this.Position) : null,
                Departments = this.Departments != null ? new List<string>(this.Departments) : null,
                BusinessUnits = this.BusinessUnits != null ? new List<string>(this.BusinessUnits) : null,
                BusinessUnitTypes = this.BusinessUnitTypes != null ? new List<string>(this.BusinessUnitTypes) : null,
                Level = this.Level != null ? new List<string>(this.Level) : null,
                CriticalRoles = this.CriticalRoles != null ? new List<string>(this.CriticalRoles) : null,
                Disciplines = this.Disciplines != null ? new List<string>(this.Disciplines) : null,
                Gender = this.Gender != null ? new List<string>(this.Gender) : null,
                Ethnicity = this.Ethnicity != null ? new List<string>(this.Ethnicity) : null,
                YearsOfExperience = this.YearsOfExperience != null ? new List<string>(this.YearsOfExperience) : null,
                Age = this.Age != null ? new List<string>(this.Age) : null,
                isOnlyAvailableJobs = this.isOnlyAvailableJobs,
                isFilterOnlyTalentpool = this.isFilterOnlyTalentpool,
                isFilterOnlySuccessionReadiness = this.isFilterOnlySuccessionReadiness,
                isFilterOnly9BoxPlotted = this.isFilterOnly9BoxPlotted,
                isFilterOnlyOutStanding = this.isFilterOnlyOutStanding
            };
        }
    }
}
