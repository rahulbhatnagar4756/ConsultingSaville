using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models.Employees;

public class EmployeesModel : CheckableModel, ICloneable
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
    public string? Fullname => $"{FirstName} {LastName}";
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
    public string? isPersonality { get; set; }
    public bool? CPP { get; set; }
    public string? isCPP { get; set; }
    public bool? GoalFinalScore { get; set; }
    public string? isGoalFinalScore { get; set; }
    public double? FinalScore { get; set; }
    public DateTime? Box9ProcessDate { get; set; }
    public string? Box9Name { get; set; }
    public int? Box9Score { get; set; }
    public string? FormsUUIDPDP { get; set; }
    public string? FormsUUIDSuccessionPlanning { get; set; }
    public bool? IsDeletedUsersManager { get; set; }

    public object Clone()
    {
        return new EmployeesModel
        {
            CompaniesUUID = CompaniesUUID,
            UsersUUID = UsersUUID,
            UsersUUIDManager = UsersUUIDManager,
            EmployeeJobsUUID = EmployeeJobsUUID,
            EmployeeDepartmentsUUID = EmployeeDepartmentsUUID,
            EmployeeBusinessUnitsUUID = EmployeeBusinessUnitsUUID,
            EmployeeBusinessUnitTypesUUID = EmployeeBusinessUnitTypesUUID,
            EmployeeLevelsUUID = EmployeeLevelsUUID,
            EmployeeBusinessUnitTypes = EmployeeBusinessUnitTypes,
            EmployeeJobsCriticalRolesUUID = EmployeeJobsCriticalRolesUUID,
            EmployeeJobDisciplinesUUID = EmployeeJobDisciplinesUUID,
            EmployeeBusinessUnits = EmployeeBusinessUnits,
            EmployeeDepartments = EmployeeDepartments,
            EmployeeJobs = EmployeeJobs,
            EmployeeLevels = EmployeeLevels,
            EmployeeJobsCriticalRoles = EmployeeJobsCriticalRoles,
            EmployeeJobDisciplines = EmployeeJobDisciplines,
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            Email = Email,
            Mobile = Mobile,
            IDNumber = IDNumber,
            EmployeeNumber = EmployeeNumber,
            Race = Race,
            Gender = Gender,
            IsImage = IsImage,
            Image = Image,
            Age = Age,
            DateOfBirth = DateOfBirth,
            TandCsigned = TandCsigned,
            FullnameManager = FullnameManager,
            EmailManager = EmailManager,
            IDNumberManager = IDNumberManager,
            EmployeeNumberManager = EmployeeNumberManager,
            Personality = Personality,
            isPersonality = isPersonality,
            CPP = CPP,
            isCPP = isCPP,
            GoalFinalScore = GoalFinalScore,
            isGoalFinalScore = isGoalFinalScore,
            FinalScore = FinalScore,
            Box9ProcessDate = Box9ProcessDate,
            Box9Name = Box9Name,
            Box9Score = Box9Score,
            FormsUUIDPDP = FormsUUIDPDP,
            FormsUUIDSuccessionPlanning = FormsUUIDSuccessionPlanning,
            IsDeletedUsersManager = IsDeletedUsersManager
        };
    }
}
