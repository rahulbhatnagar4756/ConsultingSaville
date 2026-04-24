namespace Library.Base.Models.Employees.Jobs;

public class EmployeeJobsCriticalRoleSaveModel
{
    public string? CompanyUUID { get; set; }
    public string? UsersUUIDLoggedIn { get; set; }
    public string? UUID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int EmployeeJobsCriticalRoleLevelsid { get; set; } = 4;
}