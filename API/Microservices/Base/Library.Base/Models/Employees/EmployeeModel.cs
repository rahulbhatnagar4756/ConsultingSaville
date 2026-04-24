using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Models.Employees
{
    public class EmployeeModel
    {
        public string? CompaniesUUID { get; set; }
        public string? UsersUUID { get; set; }
        public string? UsersUUIDManager { get; set; }
        public string? EmployeeJobsUUID { get; set; }
        public string? EmployeeDepartmentsUUID { get; set; }
        public string? EmployeeBusinessUnitsUUID { get; set; }
        public string? EmployeeBusinessUnitTypesUUID { get; set; }
        public string? EmployeeLevelsUUID { get; set; }
        public string? EmployeeBusinessUnitTypes { get; set; }
        public string? EmployeeJobsCriticalRolesUUID { get; set; }
        public string? EmployeeJobDisciplinesUUID { get; set; }
        public string? EmployeeBusinessUnits { get; set; }
        public string? EmployeeDepartments { get; set; }
        public string? EmployeeJobs { get; set; }
        public string? EmployeeLevels { get; set; }
        public string? LevelOrder { get; set; }
        public string? EmployeeJobsCriticalRoles { get; set; }
        public string? EmployeeJobDisciplines { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? IDNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? Race { get; set; }
        public string? Gender { get; set; }
        public bool? IsImage { get; set; }
        public string? Image { get; set; }
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? TandCsigned { get; set; }
        public string? FullnameManager { get; set; }
        public string? EmailManager { get; set; }
        public string? IDNumberManager { get; set; }
        public string? EmployeeNumberManager { get; set; }

        public bool? Personality { get; set; }
        public string? isPersonality { get 
            {
                if (Personality == null) return "No";
                return Personality.Value ? "Yes" : "No";
            } 
        }
        public bool? CPP { get; set; }
        public string? isCPP
        {
            get
            {
                if (CPP == null) return "No";
                return CPP.Value ? "Yes" : "No";
            }
        }

        public bool? GoalFinalScore { get; set; }
        public string? isGoalFinalScore
        {
            get
            {
                if (GoalFinalScore == null) return "No";
                return GoalFinalScore.Value ? "Yes" : "No";
            }
        }
        public double? FinalScore { get; set; }
        public DateTime? Box9ProcessDate { get; set; }
        public string? Box9Name { get; set; }
        public int? Box9Score { get; set; }
        public bool? IsDeletedUsersManager { get; set; }
    }
}
