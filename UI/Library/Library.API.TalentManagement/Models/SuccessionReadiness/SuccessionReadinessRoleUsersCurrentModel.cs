namespace Library.API.TalentManagement.Models.SuccessionReadiness;

public class SuccessionReadinessRoleUsersCurrentModel
{
    public string? UsersUUID { get; set; } = "";
    public string? EmployeeBusinessUnitsUUID { get; set; } = "";
    public string? EmployeeDepartmentsUUID { get; set; } = "";
    public string? EmployeeJobsUUID { get; set; } = "";
    public string? FirstName { get; set; } = "";
    public string? LastName { get; set; } = "";
    public string? IDNumber { get; set; } = "";
    public string? Email { get; set; } = "";
    public string? Mobile { get; set; } = "";
    public string? Gender { get; set; } = "";
    public string? Ethnicity { get; set; } = "";
    public DateTime? DateOfBirth { get; set; }
    public string? BusinessUnits { get; set; } = "";
    public string? Department { get; set; } = "";
    public string? EmployeeJobs { get; set; } = "";
    public string? Level { get; set; } = "";
    public string? RequirementsForNextRole { get; set; } = "";
    public int? CurrentPositionYears { get; set; } = 0;
    public int? CurrentPositionMonths { get; set; } = 0;
    public string? AvatarPath { get; set; } = "";
}
